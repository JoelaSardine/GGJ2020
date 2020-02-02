using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Machine : Interactable
{
    public MiniGameType repairGame;
    public float repairArgument;
    public UnityEvent OnRepairFinished;
    public int brokenChance = 3;

    public ItemType ItemRequired;
    public bool broken;

    public virtual void Start()
    {
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

        ChangeName(this.gameObject.name);
    }

    IEnumerator CheckForBroken()
    {
        while(true)
        {
            if (Random.Range(0, 100) > 100-brokenChance)
            {
                broken = true;
                ChangeName("Broken");
            }

            yield return new WaitForSeconds(1f);
        }
        
    }
}
