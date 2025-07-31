using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : Character
{
    public List<SplineContainer> PatrolPaths;
    public bool IsRandomPath;

}
