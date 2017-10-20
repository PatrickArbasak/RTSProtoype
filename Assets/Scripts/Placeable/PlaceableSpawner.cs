using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlaceableSpawner : MonoBehaviour {

    [SerializeField] private LayerMask terrainPieceLayerMask;
    [SerializeField] private float selectionMaxDistance;

    [SerializeField] private Material highLightMaterial;
    private Material originalMaterial;
    private Placeable baseCurrentlySpawning;
    private Renderer baseCurrentlySpawningRenderer;
    private Placeable baseToSpawn;

    private bool isSpawning;
    TerrainPiece selectedTerrainPiece = null;

    ISelectable currentSelecteable = null;

    private void Start()
    {
        isSpawning = false;
    }

    void OnEnable()
    {
        PlayerInput.OnMouseClick += MouseClickInput;
    }

    void OnDisable()
    {
        PlayerInput.OnMouseClick -= MouseClickInput;
    }

    // Called from UI Button's OnClick event.
    public void StartSearching(Placeable placeable)
    {
        if (!isSpawning)
        {
            baseToSpawn = placeable;
            isSpawning = true;
            StartCoroutine(SearchingForSpawnPoint());
        }
    }

    void MouseClickInput()
    {
        // Spawn a base when currently searching.
        if (isSpawning && selectedTerrainPiece != null && selectedTerrainPiece.isOccupied == false)
        {
            StopCoroutine(SearchingForSpawnPoint());
            baseCurrentlySpawningRenderer.material = originalMaterial;
            baseCurrentlySpawning.OccupiedTerrainPiece = selectedTerrainPiece;

            //Base aBase = null; 
            //if (aBase = baseCurrentlySpawning.GetComponent<Base>())
            //    aBase.Damage(50);
        
            baseCurrentlySpawningRenderer = null;
            originalMaterial = null;
            baseCurrentlySpawning = null;
            isSpawning = false;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                ISelectable selecteable = hit.collider.GetComponent<ISelectable>();
                if (currentSelecteable != null)
                    currentSelecteable.UnSelected();
                if (selecteable != null)
                {
                    currentSelecteable = selecteable;
                    currentSelecteable.Selected();
                }
            }
        }
    }

    IEnumerator SearchingForSpawnPoint()
    {
        while (isSpawning)
        {
            SpawnBaseToSpawn();
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, terrainPieceLayerMask) && baseCurrentlySpawning != null)
            {
                if (selectedTerrainPiece = hit.collider.gameObject.GetComponent<TerrainPiece>())
                {
                    Vector3 hightlightPosition = new Vector3(selectedTerrainPiece.NavPoint.position.x, baseCurrentlySpawningRenderer.bounds.extents.y, selectedTerrainPiece.NavPoint.position.z);
                    baseCurrentlySpawning.transform.position = hightlightPosition;
                    baseCurrentlySpawning.transform.rotation = Quaternion.LookRotation(-transform.forward);
                    if (!selectedTerrainPiece.isOccupied)
                        baseCurrentlySpawningRenderer.material.color = new Color(0, 0, 1, 0.5f);
                    else
                        baseCurrentlySpawningRenderer.material.color = new Color(1, 0, 0, 0.5f);
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void SpawnBaseToSpawn()
    {
        if (baseCurrentlySpawning == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, terrainPieceLayerMask))
            {
                baseCurrentlySpawning = Instantiate(baseToSpawn, hit.point, Quaternion.identity);
                baseCurrentlySpawningRenderer = baseCurrentlySpawning.GetComponent<Renderer>();
                originalMaterial = baseCurrentlySpawningRenderer.material;
                baseCurrentlySpawningRenderer.material = highLightMaterial;
            }
        }
    }
}
