using UnityEngine;

public class ItemSetDemo : MonoBehaviour
{
    private RadialMenu radialMenu;
    [SerializeField] private RadialMenuItemData[] itemsData;

    private void Start()
    {
        radialMenu = FindAnyObjectByType<RadialMenu>(FindObjectsInactive.Include);

        for (int i = 0; i < itemsData.Length; i++)
            radialMenu.AddItem(itemsData[i]);
    }
}
