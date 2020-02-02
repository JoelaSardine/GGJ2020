using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AutoSyncLabel : MonoBehaviour
{
    public GameObject NameHolder;

    private TextMeshProUGUI label;
    
    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();
        label.text = NameHolder.name;
    }
}
