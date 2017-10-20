using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseInput : MonoBehaviour {

    [SerializeField] private LayerMask terrainPieceLayerMask;
    [SerializeField] private float selectionMaxDistance;

    private Placeable baseCurrentlySpawning;
    private Placeable baseToSpawn;

    private bool isSpawning = false;
    private bool isHoldingMouse = false;

    private TerrainPiece selectedTerrainPiece = null;

    private ISelectable currentSelecteable = null;


    private List<TerrainPiece> terrainPiecePath = new List<TerrainPiece>();

    public GameObject TESTGAMEOBJECT;
    private List<GameObject> TESTGAMEOBJECTS = new List<GameObject>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseClickInput();
            isHoldingMouse = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHoldingMouse = false;
            terrainPiecePath.Clear();

            //TEST CODE
            foreach (GameObject gm in TESTGAMEOBJECTS)
                Destroy(gm);
            TESTGAMEOBJECTS.Clear();
            //END TEST CODE
        }

        // If Spawning, handle the creation and movement of a spawning Placeable.
        if (isSpawning) 
        {
            RaycastHit hit = new RaycastHit();
            bool hitTerrain = RaycastOnlyOnTerrain(ref hit);

            if (baseCurrentlySpawning == null)
            {
                if (hitTerrain)
                    baseCurrentlySpawning = Instantiate(baseToSpawn, hit.point, Quaternion.identity);
            }
            if (hitTerrain && baseCurrentlySpawning != null)
            {
                if (selectedTerrainPiece = hit.collider.gameObject.GetComponent<TerrainPiece>())
                {
                    baseCurrentlySpawning.OccupiedTerrainPiece = selectedTerrainPiece;
                    float heightFromTerrain = baseCurrentlySpawning.GetComponent<Renderer>().bounds.extents.y;
                    Vector3 hightlightPosition = new Vector3(selectedTerrainPiece.NavPoint.position.x, heightFromTerrain, selectedTerrainPiece.NavPoint.position.z);
                    baseCurrentlySpawning.transform.position = hightlightPosition;
                    baseCurrentlySpawning.transform.rotation = Quaternion.LookRotation(-transform.forward);
                }
            }
        }
        else if (isHoldingMouse && !EventSystem.current.IsPointerOverGameObject())
        {
            // Raycast and check if terrian was hit.
            RaycastHit hit = new RaycastHit();
            bool hitTerrain = RaycastOnlyOnTerrain(ref hit);
            if (hitTerrain)
            {
                if (selectedTerrainPiece = hit.collider.gameObject.GetComponent<TerrainPiece>())
                {
                    // Make sure this terrain piece can be added to path.
                    if (selectedTerrainPiece != null && selectedTerrainPiece.isOccupied == false && terrainPiecePath.Contains(selectedTerrainPiece) == false)
                    {
                        terrainPiecePath.Add(selectedTerrainPiece);

                        //TEST CODE
                        GameObject gm = Instantiate(TESTGAMEOBJECT, terrainPiecePath[terrainPiecePath.Count - 1].NavPoint.transform);
                        TESTGAMEOBJECTS.Add(gm);
                        //END TEST CODE

                    }
                }
            }
        }
    }

    bool RaycastOnlyOnTerrain(ref RaycastHit hit)
    {
        return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, selectionMaxDistance, terrainPieceLayerMask);
    }

    // Called from UI Button's OnClick event.
    public void StartSearching(Placeable placeable)
    {
        if (!isSpawning)
        {
            baseToSpawn = placeable;
            isSpawning = true;
        }
    }

    void MouseClickInput()
    {
        // Spawn a base when currently searching.
        if (isSpawning && selectedTerrainPiece != null && selectedTerrainPiece.isOccupied == false)
        {
            isSpawning = false;
            baseCurrentlySpawning.OccupiedTerrainPiece = selectedTerrainPiece;
            baseCurrentlySpawning.Spawn();

            baseCurrentlySpawning = baseToSpawn = null;
        }
        else if (baseCurrentlySpawning == null)
            HandleSelectable();
    }

    private void HandleSelectable()
    {
        // If we hit a selectable, select it, and deselect the current selectable, if applicable.
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
