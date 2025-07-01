using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void SetCursor2(CursorAsset asset) =>
        Cursor.SetCursor(asset.Cursor, asset.HotSpot, CursorMode.Auto); //CursorMode.ForceSoftware плохо работает
}