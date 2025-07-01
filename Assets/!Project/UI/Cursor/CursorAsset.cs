using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "CursorAsset", menuName = "ScriptableObjects/CursorAsset")]
public class CursorAsset : ScriptableObject
{
    public Texture2D Cursor;
    public Vector2 HotSpot;
}