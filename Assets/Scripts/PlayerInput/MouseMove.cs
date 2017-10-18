using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMove : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] private int mouseMoveSpeed = 5;
    [SerializeField] private int distanceFromBoundary = 0;
    [SerializeField] bool shouldUseMouseMove = true;
    [SerializeField] bool shouldUseWASDMove = true;

    [Header("Mouse Scrolling")]
    [SerializeField] private int scrollMoveSpeed = 10;
    [SerializeField] private int scrollMinDistance = 5;
    [SerializeField] private int scrollMaxDistance = 20;

    [Header("Map Boundary")]
    [SerializeField] bool shouldUseMapBoundary = true;
    [SerializeField] private float nonPassibleBorderWidth = 10;
    [SerializeField] private Terrain mapTerrain;
    private Vector3 mapMinBounds;
    private Vector3 mapMaxBounds;

    private int screenWidth;
    private int screenHeight;

    private void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        //calculate Terrain bounds
        //mapMinBounds = new Vector3(mapTerrain.transform.position.x, 0, mapTerrain.transform.position.z);
        //mapMaxBounds += mapMinBounds + new Vector3(mapTerrain.terrainData.size.x, 0, mapTerrain.terrainData.size.z);

        //apply any border edging spce clamping
        mapMinBounds.x += nonPassibleBorderWidth;
        mapMinBounds.z += nonPassibleBorderWidth;
        mapMaxBounds.x -= nonPassibleBorderWidth;
        mapMaxBounds.z -= nonPassibleBorderWidth;
    }

    private void Update()
    {
        BoundaryMove();
        MouseScrollMove();
        if (shouldUseMapBoundary)
            KeepCameraWithinMap();
    }

    // Movement for mouse touching boundaries of screen
    private void BoundaryMove()
    {
        Vector3 mousePosition = Input.mousePosition;
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");
        // If to the right or left of screen
        if (mousePosition.x > screenWidth - distanceFromBoundary && shouldUseMouseMove || horizontalAxis > 0.0f && shouldUseWASDMove)
        {
            transform.position += new Vector3(mouseMoveSpeed * Time.deltaTime, 0, 0);
        }
        if (mousePosition.x < 0 + distanceFromBoundary && shouldUseMouseMove || horizontalAxis < 0.0f && shouldUseWASDMove)
        {
            transform.position -= new Vector3(mouseMoveSpeed * Time.deltaTime, 0, 0);
        }

        // If above or below of screen
        if (mousePosition.y > screenHeight - distanceFromBoundary && shouldUseMouseMove || verticalAxis > 0.0f && shouldUseWASDMove)
        {
            transform.position += new Vector3(0, 0, mouseMoveSpeed * Time.deltaTime);
        }
        if (mousePosition.y < 0 + distanceFromBoundary && shouldUseMouseMove || verticalAxis < 0.0f && shouldUseWASDMove)
        {
            transform.position -= new Vector3(0, 0, mouseMoveSpeed * Time.deltaTime);
        }
    }

    // Movement for mouse scrolling
    private void MouseScrollMove()
    {
        float MouseAxis = Input.GetAxis("Mouse ScrollWheel") * scrollMoveSpeed;
        if (MouseAxis != 0)
        {
            transform.Translate(0, MouseAxis * Time.deltaTime, -MouseAxis * Time.deltaTime);    
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, scrollMinDistance, scrollMaxDistance), transform.position.z);
        }
    }

    private void KeepCameraWithinMap()
    {
        if (transform.position.x > mapMaxBounds.x)
        {
            transform.position = new Vector3(mapMaxBounds.x, transform.position.y, transform.position.z);
        }
        if (transform.position.z > mapMaxBounds.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, mapMaxBounds.z);
        }
        if (transform.position.x < mapMinBounds.x)
        {
            transform.position = new Vector3(mapMinBounds.x, transform.position.y, transform.position.z);
        }
        if (transform.position.z < mapMinBounds.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, mapMinBounds.z);
        }
    }
}
