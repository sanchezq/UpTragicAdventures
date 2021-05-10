using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D cursorArrow;
    void Start()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);

    }

    void Update()
    {
        /*Vector2 CursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = CursorPos;*/
    }
}
