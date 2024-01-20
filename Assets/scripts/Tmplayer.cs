using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tmplayer : MonoBehaviour
{
    public string sortingLayerName;
    public int sortingOrder;

    private void Start()
    {
        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.sortingLayerName = sortingLayerName;
        mesh.sortingOrder = sortingOrder;
    }
}
