using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOneShot : Item
{
    public override void Used()
    {
        Drop();
        Destroy(this.gameObject);
    }
}
