using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Machine : Interactable
{
    public MiniGameType repairGame;
    public float repairArgument;
    public UnityEvent OnRepairFinished;

    public ItemType ItemRequired;
    public bool broken;

    private string pastName;

    public virtual void Start()
    {
        pastName = gameObject.name;
        OnRepairFinished.AddListener(RepairEvent);
        StartCoroutine(CheckForBroken());
    }


    public override void InteractWithItem(Interactable itemHolded)
    {
        
    }

    public override void Interact(PlayerController player)
    {

    }

    public void RepairEvent()
    {
        broken = false;

        ChangeName(pastName);
    }

    IEnumerator CheckForBroken()
    {
        while(true)
        {
            if (Random.Range(0, 100) > 97)
            {
                broken = true;
                ChangeName("Broken");
            }

            yield return new WaitForSeconds(1f);
        }
        
    }
}
