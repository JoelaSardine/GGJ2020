using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Interactable
{
    public ItemType ItemRequired;

    public override void InteractWithItem(Interactable itemHolded)
    {
        Item item = itemHolded as Item;

        if (item != null)
        {
            if (ItemRequired == item.type)
            {
                Debug.Log("Type equal = true");
            }
            else
            {
                Debug.Log("Type equal = false");
            }
        }
        else if (ItemRequired == ItemType.None)
        {
            Debug.Log("Type equal = true");
        }
        else
        {
            Debug.Log("Type equal = false");
        }
    }

    public override void Interact(PlayerController player)
    {

    }
}
