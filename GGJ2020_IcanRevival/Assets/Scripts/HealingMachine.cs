using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingMachine : Interactable
{
    public ItemType cure;

    public override void InteractWithItem(Interactable itemHolded)
    {
        Patient patient = itemHolded as Patient;
        print("lel");
        if (patient?.sickness.TryCure(cure) ?? false)
        {
            print("lal");
            patient.ChangeHealth(3);
        }
    }
}
