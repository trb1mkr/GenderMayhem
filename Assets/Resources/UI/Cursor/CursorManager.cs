using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public void SetCursor2(CursorAsset asset) =>
        Cursor.SetCursor(asset.Cursor, asset.HotSpot, CursorMode.Auto);
}