using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemResetable : Item
{
    public ItemType newItemType;
    public Material newItemMat;
    public string newItemName;

    public override void Used()
    {
        type = newItemType;
        GetComponentInChildren<MaterialChanger>().ChangeAsset(newItemMat);
        ChangeName(newItemName);
    }
}
