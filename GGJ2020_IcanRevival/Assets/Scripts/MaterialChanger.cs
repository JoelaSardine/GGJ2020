using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    public MeshRenderer renderer;
    public int materialId;

    public void ChangeAsset(Material newItemMat)
    {
        Material[] mats = renderer.materials;

        mats[materialId] = newItemMat;

        renderer.materials = mats;
    }
}
