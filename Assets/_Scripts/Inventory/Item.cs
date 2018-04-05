using System;
using UnityEngine;

public enum Rarity
{
    Common, Uncommon, Rare, Epic, Ultra
}

public class Item
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Value { get; set; }
    public Rarity Rarity { get; set; }
    public Sprite Icon { get; set; }
}


