using UnityEngine;


[System.Serializable]
public class Item
{
    public ItemType Type;
    public ItemCategory Category;
    public Sprite Sprite;
    public float ItemScore;


    public Item(ItemType type, ItemCategory category, Sprite sprite)
    {
        this.Type = type;
        this.Sprite = sprite;
        this.Category = category;

    }
}
