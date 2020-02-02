using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemResetable : Item
{
    public ItemType newItemType;
    public Sprite newItemSprite;
    public string newItemName;

    public void ResetItem()
    {
        type = newItemType;
        GetComponentInChildren<SpriteRenderer>().sprite = newItemSprite;
        ChangeName(newItemName);
    }
}
