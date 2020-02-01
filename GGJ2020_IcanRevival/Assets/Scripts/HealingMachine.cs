using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingMachine : Interactable
{
    public ItemType cure;

    public override void InteractWithItem(Interactable itemHolded)
    {
        Patient patient = itemHolded as Patient;
        if(patient?.enabled ?? false) patient?.sickness.TryCure(cure, itemHolded.holder);
    }
}
