using UnityEngine;

public abstract class Item : Interactable
{
    public ItemInfomation itemInfo;

    public string itemName => itemInfo.itemName;
    public int itemID => itemInfo.itemID;
    public Sprite itemIcon => itemInfo.itemIcon;
    public GameObject meshPrefab => itemInfo.meshPrefab;

    protected virtual void Setup()
    {
        if(itemInfo != null)
        {
            gameObject.name = itemName;
            gameObject.tag = "Item";  
        }
        else
        {
            Debug.LogWarning("ItemInfomation is not assigned for " + gameObject.name);
        }
    }
}