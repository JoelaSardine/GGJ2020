using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBehaviour : Machine
{
    public override void InteractWithItem(Interactable item)
    {
        base.InteractWithItem(item);

        if(item is Patient)
        {
            item.transform.parent = this.transform;
            item.transform.localPosition = Vector3.zero;
        }
    }
}
