using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : Interactable
{
    public ItemType ItemRequired;

    public virtual void InteractWithItem(Item itemHolded)
    {
        if (itemHolded != null)
        {
            if (ItemRequired == itemHolded.type)
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
}
