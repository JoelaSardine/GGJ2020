using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBehaviour : Machine
{
    public Patient currentPatient;

    public override void InteractWithItem(Interactable item)
    {
        if(item is Patient)
        {
            item.transform.parent = this.transform;
            item.transform.localPosition = Vector3.zero;
            currentPatient = item as Patient;
            item.Drop();
            currentPatient.collider.enabled = false;
        }

        
    }

    public override void Interact(PlayerController player)
    {
        if (currentPatient != null)
        {
            this.transform.DetachChildren();
            player.Grab(currentPatient);
            currentPatient.collider.enabled = true;
            currentPatient = null;
        }
    }
}
