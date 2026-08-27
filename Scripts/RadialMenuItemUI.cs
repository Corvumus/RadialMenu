using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadialMenuItemUI : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color baseColor;
    [SerializeField] private Image icon;
    [SerializeField] private Transform iconObjTransform;

    private Image background;
    public RadialMenuItemData Data { get; private set; }

    public bool IsHaveData => data != null;
    public string Description => data != null ? data.description : "";

    private void Awake()
    {
        background = GetComponent<Image>();
        background.color = baseColor;
        icon.gameObject.SetActive(false);
    }

    private void Start()
    {
        float angle = -360f * background.fillAmount * 0.5f;
        iconObjTransform.Rotate(0, 0, angle);
        icon.transform.rotation = Quaternion.identity;
    }

    public void Select()
    {
        background.color = hoverColor;
    }

    public void Deselect()
    {
        background.color = baseColor;
    }

    public void Apply()
    {
        if (data == null) return;
        
        //Что-то делаем
        Debug.Log(data.description);
    }

    public void SetData(RadialMenuItemData itemData)
    {
        data = itemData;

        icon.sprite = data.icon;
        icon.gameObject.SetActive(true);
    }

    public bool IsEqual(RadialMenuItemData itemData)
    { 
        return data == itemData;
    }

    public void RemoveData()
    { 
        data = null;
        icon.gameObject.SetActive(false);
    }
}
