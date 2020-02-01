﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    None,
    Wrench,
    Seringe,
    Patient,
}

public class Item : Interactable
{
    public ItemType type;
}
