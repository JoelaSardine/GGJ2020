using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniGameTransformeur : Machine
{
    public MiniGameType game;
    public float argument;
    public ItemType newItemType;
    public Material newItemMat;
    public string newItemName;

    public UnityEvent OnFinished;

    private Item item;

    public override void Start()
    {
        base.Start();

        OnFinished.AddListener(TransformItem);
    }

    public override void InteractWithItem(Interactable itemHolded)
    {
        if (broken == false)
        {
            item = itemHolded as Item;
            if (item.type == ItemRequired)
            {
                PlayerController player = itemHolded.holder;
                player.miniGame.SetMiniGame(OnFinished, game, argument );
            }
        }
        else
        {
            item = itemHolded as Item;
            if (item.type == ItemType.ClefAMolette)
            {
                PlayerController player = itemHolded.holder;
                player.miniGame.SetMiniGame(OnRepairFinished, repairGame, repairArgument);
            }
        }
        
    }

    private void TransformItem()
    {
        item.type = newItemType;
        item.GetComponentInChildren<MaterialChanger>().ChangeAsset(newItemMat);
        item.ChangeName(newItemName);
    }
}
