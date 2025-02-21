using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PC2DPaths", menuName = "ScriptableObjects/PC2DPaths")]
public class PC2DPaths : ScriptableObject
{
    public List<PC2DPathPoints> Paths = new List<PC2DPathPoints>();
}

