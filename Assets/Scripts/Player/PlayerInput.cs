using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    public delegate void SpawnBaseInput();
    public static event SpawnBaseInput OnSpawnBaseInput;

    public delegate void MouseClick();
    public static event MouseClick OnMouseClick;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0) && OnMouseClick != null)
        {
            OnMouseClick();
        }
        if (Input.GetKeyDown(KeyCode.G) && OnSpawnBaseInput != null)
        {
            OnSpawnBaseInput();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
