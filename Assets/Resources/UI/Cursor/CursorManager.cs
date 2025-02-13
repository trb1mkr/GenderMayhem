using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    Vector2 mousePosition;

    void Start()
    {
        #if UNITY_EDITOR == false
            Cursor.visible = false;
        #endif
    }

    void Update()
    {
        gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 1));
    }

    void OnCrosshair(InputValue position)
    {
        mousePosition = position.Get<Vector2>();
    }
}