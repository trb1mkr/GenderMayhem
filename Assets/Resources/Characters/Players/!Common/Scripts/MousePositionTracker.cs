using UnityEngine;

public class MousePositionTracker : MonoBehaviour
{
    void Update()
    {
        if (!Application.isFocused) return;
        var mousePositionScreen = transform.root.GetComponentInChildren<PlayerControls>().MousePosition;
        transform.position = Camera.main.ScreenToWorldPoint((Vector3)mousePositionScreen);
        transform.position.Set(transform.position.x, transform.position.y, -1);
    }
}