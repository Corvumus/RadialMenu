using TMPro;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int itemCount;
    [SerializeField] private float offset = -90;

    [Header("Prefabs")]
    [SerializeField] private RadialMenuSlotUI slotPrefab;
    [SerializeField] private Transform linePrefab;

    [Header("Containers")]
    [SerializeField] private Transform slotsContainer;
    [SerializeField] private Transform linesContainer;

    [Header("Description")]
    [SerializeField] private TextMeshProUGUI itemDescriptionTextMesh;

    private RadialMenuInput radialMenuInput;

    private RadialMenuSlotUI[] slots;
    private Transform[] lines;

    private RadialMenuSlotUI selectedSlot;
    private RadialMenuSlotUI previousSlot;

    private void Awake()
    {
        radialMenuInput = GetComponentInParent<RadialMenuInput>();

        slots = new RadialMenuSlotUI[itemCount];
        lines = new Transform[itemCount];

        CreateSlots();

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (selectedSlot != null)
            selectedSlot.Apply();
    }

    private void Update()
    {
        SelectSlot();
    }

    private void CreateSlots()
    {
        if (itemCount <= 0)
            return;

        float angle = 360f / itemCount;

        for (int i = 0; i < itemCount; i++)
        {
            Vector3 rotation = new(0, 0, -angle * i);
            Quaternion rotationQuaternion = Quaternion.Euler(rotation);

            RadialMenuSlotUI newItem = Instantiate(slotPrefab, transform.position, rotationQuaternion, slotsContainer);
            newItem.SetFillAmount(1f / itemCount);
            slots[i] = newItem;

            lines[i] = Instantiate(linePrefab, transform.position, rotationQuaternion, linesContainer);
        }
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
        selectedSlot = slots[index];

        if (selectedSlot != previousSlot)
        {
            if (previousSlot != null)
                previousSlot.Deselect();

            selectedSlot.Select();
            itemDescriptionTextMesh.text = selectedSlot.Description;
            previousSlot = selectedSlot;
        }
    }

    public void AddItem(RadialMenuItemData itemData)
    {
        for (int i = 0; i < itemCount; i++)
            if (slots[i].Data == null)
            {
                slots[i].SetData(itemData);
                return;
            }
    }

    public void RemoveItem(RadialMenuItemData itemData)
    {
        for (int i = 0; i < itemCount; i++)
            if (slots[i].Data == itemData)
            {
                slots[i].RemoveData();
                return;
            }
    }

    public void RemoveItemAt(int index)
    {
        if (index >= itemCount) return;

        if (slots[index].Data != null)
            slots[index].RemoveData();
    }
}
