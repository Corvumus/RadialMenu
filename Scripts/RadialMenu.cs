using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int itemCount;
    [SerializeField] private float offset = -90;

    [Header("Prefabs")]
    [SerializeField] private RadialMenuItemUI itemPrefab;
    [SerializeField] private Transform linePrefab;

    [Header("Containers")]
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private Transform linesContainer;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI itemDescriptionTextMesh;

    private RadialMenuInput radialMenuInput;

    private RadialMenuItemUI[] items;
    private Transform[] lines;

    private RadialMenuItemUI selectedItem;
    private RadialMenuItemUI previousItem;

    private void Awake()
    {
        radialMenuInput = GetComponentInParent<RadialMenuInput>();

        items = new RadialMenuItemUI[itemCount];
        lines = new Transform[itemCount];

        Init();

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (selectedItem != null)
            selectedItem.Apply();
    }

    private void Update()
    {
        SelectSlot();
    }

    private void SelectSlot()
    {
        if (itemCount <= 0)
            return;

        Vector2 normalizedPos = radialMenuInput.GetNormalizedPosition();

        //Ничего не выбираем, если стик геймпада стоит на месте
        if (normalizedPos == Vector2.zero)
            return;

        float currentAngle = Mathf.Atan2(normalizedPos.y, normalizedPos.x) * Mathf.Rad2Deg;

        //Угол всегде в диапазоне от 0 до 360
        currentAngle = Mathf.Repeat(-(currentAngle + offset), 360f);
        //Вычисляем индекс
        int index = Mathf.FloorToInt(currentAngle / (360f / itemCount));
        //Ограничиваем индекс в пределах допустимого значения
        index = Mathf.Clamp(index, 0, itemCount - 1);
        selectedItem = items[index];

        if (selectedItem != previousItem)
        {
            if (previousItem != null)
                previousItem.Deselect();

            selectedItem.Select();
            itemDescriptionTextMesh.text = selectedItem.Description;
            previousItem = selectedItem;
        }
    }

    private void Init()
    {
        if (itemCount <= 0)
            return;

        float angle = 360f / itemCount; 

        for (int i = 0; i < itemCount; i++)
        {
            Vector3 rotation = new(0, 0, -angle * i);
            Quaternion rotationQuaternion = Quaternion.Euler(rotation);
            //Создаём слоты
            RadialMenuItemUI newItem = Instantiate(itemPrefab, transform.position, rotationQuaternion, itemsContainer);
            newItem.GetComponent<Image>().fillAmount = 1f / itemCount;
            items[i] = newItem;
            //Создаём разделяющие линии
            lines[i] = Instantiate(linePrefab, transform.position, rotationQuaternion, linesContainer);
        }
    }

    public void AddItem(RadialMenuItemData itemData)
    {
        for (int i = 0; i < itemCount; i++)
            if (!items[i].IsHaveData)
            { 
                items[i].SetData(itemData);
                return;
            }
    }

    public void RemoveItem(RadialMenuItemData itemData)
    {
        for (int i = 0; i < itemCount; i++)
            if (items[i].IsEqual(itemData))
            {
                items[i].RemoveData();
                return;
            }
    }
}
