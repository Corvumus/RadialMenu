using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RadialMenuSlotUI : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Transform iconObjTransform;
    [SerializeField] private Image icon;

    private Image background;

    public RadialMenuItemData Data { get; private set; }
    public string Description => Data != null ? Data.description : "";


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

    public void SetFillAmount(float fillAmount)
    {
        fillAmount = Mathf.Clamp01(fillAmount);

        background.fillAmount = fillAmount;
    }

    public void Select()
    {
        background.color = hoverColor;
    }

    public void Deselect()
    {
        background.color = baseColor;
    }

    public void SetData(RadialMenuItemData itemData)
    {
        Data = itemData;

        icon.sprite = itemData.icon;
        icon.gameObject.SetActive(true);
    }

    public void Apply()
    {
        if (Data == null) return;

        //Что-то делаем
        Debug.Log(Data.description);
    }

    public void RemoveData()
    {
        Data = null;
        icon.gameObject.SetActive(false);
    }
}
