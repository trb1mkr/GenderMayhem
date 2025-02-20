using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDebug : MonoBehaviour
{
    [SerializeField]
    private bool Debug;
    void Start()
    {
        var Markers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        if (!Debug)
        {
            foreach (var Marker in Markers)
            {
                Marker.enabled = false;
            }
        }
    }
}
