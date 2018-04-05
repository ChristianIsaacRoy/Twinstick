using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/New Item")]
public class ItemBlueprint : ScriptableObject
{
    public new string name;
    [TextArea(2,5)]
    public string     description;
    public int        value;
    public Rarity     rarity;
    public Sprite     icon;

    public Item GetItem()
    {
        return new Item()
        {
            Name = name,
            Description = description,
            Value = value,
            Rarity = rarity,
            Icon = icon
        };
    }
}