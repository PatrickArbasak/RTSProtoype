using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    public Transform NavPoint;
    private bool isOccupied = false;

    [SerializeField] private Material occupiedMaterial;
    private Material originalMaterial;
    private Renderer tileRenderer;

    public bool IsOccupied
    {
        get { return isOccupied; }
        set
        {
            tileRenderer.material = value ? occupiedMaterial : originalMaterial;
            isOccupied = value;
        }
    }

    private void Start()
    {
        tileRenderer = gameObject.GetComponent<Renderer>();
        originalMaterial = tileRenderer.material;
    }
}
