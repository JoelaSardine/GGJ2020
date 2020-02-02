using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    None,
    Medication,
    Seringe,
    SupositoirChimioterapique,
    Defibrilateur,
    Sang,
    PocheVide,
    SeringeVide,
    ClefAMolette,
}

public class Item : Interactable
{
    public ItemType type;
}
