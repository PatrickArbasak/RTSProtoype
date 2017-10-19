using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public delegate void MouseClick();
    public static event MouseClick OnMouseClick;

    void Update ()
    {
        if (Input.GetMouseButtonDown(0) && OnMouseClick != null)
        {
            OnMouseClick();
        }
	}
}
