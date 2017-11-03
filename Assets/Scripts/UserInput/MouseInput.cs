using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseInput : MonoBehaviour {

    [SerializeField] private LayerMask floorTileLayerMask;
    [SerializeField] private float selectionMaxDistance;

    private BoardPiece pieceCurrentlySpawning;
    private BoardPiece pieceToSpawn;

    private bool isSpawning = false;
    private bool isHoldingMouse = false;
    private bool isSelectingSelectable = false;

    private FloorTile selectedFloorTile = null;

    private ISelectable currentSelecteable = null;

    private List<FloorTile> floorTilePath = new List<FloorTile>();

    public bool GetIsSelectingUI() { return EventSystem.current.IsPointerOverGameObject(); }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseClickInput();
            isHoldingMouse = true;
        }
        if (Input.GetMouseButtonUp(0) && !GetIsSelectingUI())
        {
            isHoldingMouse = false;
            HandleFinishedSelectingSelectable();
        }

        // If Spawning, handle the creation and movement of a spawning Placeable.
        if (isSpawning) 
        {
            RaycastHit hit = new RaycastHit();
            bool hitTerrain = RaycastOnlyOnTerrain(ref hit);

            if (pieceCurrentlySpawning == null)
            {
                if (hitTerrain)
                    pieceCurrentlySpawning = Instantiate(pieceToSpawn, hit.point, Quaternion.identity);
            }
            if (hitTerrain && pieceCurrentlySpawning != null)
            {
                if (selectedFloorTile = hit.collider.gameObject.GetComponent<FloorTile>())
                {
                    pieceCurrentlySpawning.OccupiedFloorTile = selectedFloorTile;
                    Vector3 hightlightPosition = new Vector3(selectedFloorTile.NavPoint.position.x, pieceCurrentlySpawning.HeightFromFloor, selectedFloorTile.NavPoint.position.z);
                    pieceCurrentlySpawning.transform.position = hightlightPosition;
                    pieceCurrentlySpawning.transform.rotation = Quaternion.LookRotation(-transform.forward);
                }
            }
        }
        // Holding mouse while selecting.
        else if (isHoldingMouse && isSelectingSelectable && currentSelecteable != null && !GetIsSelectingUI())
        {
            // Raycast and check if terrian was hit.
            RaycastHit hit = new RaycastHit();
            bool hitTerrain = RaycastOnlyOnTerrain(ref hit);
            if (hitTerrain)
            {
                if (selectedFloorTile = hit.collider.gameObject.GetComponent<FloorTile>())
                {
                    FloorTile lastTileInPath = null;
                    if (floorTilePath.Count > 0) // if this is not the first tile in path.
                        lastTileInPath = floorTilePath[floorTilePath.Count - 1];
                    else
                    {
                        MonoBehaviour mg = currentSelecteable as MonoBehaviour;
                        BoardPiece boardPiece;
                        if (boardPiece = mg.gameObject.GetComponent<BoardPiece>())
                        {
                            lastTileInPath = boardPiece.OccupiedFloorTile;
                        }
                    }
                    // Make sure this terrain piece can be added to path.
                    bool tileNotOnPathOrOccupied = selectedFloorTile.CurrentTileState != FloorTile.TileState.OnPath && selectedFloorTile.CurrentTileState != FloorTile.TileState.Occupied;
                    if (tileNotOnPathOrOccupied && !floorTilePath.Contains(selectedFloorTile) && lastTileInPath.IsTileConnected(selectedFloorTile))
                    {
                        selectedFloorTile.CurrentTileState = FloorTile.TileState.OnPath;
                        floorTilePath.Add(selectedFloorTile);
                    }
                }
            }
        }
    }

    bool RaycastOnlyOnTerrain(ref RaycastHit hit)
    {
        return Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, selectionMaxDistance, floorTileLayerMask);
    }

    // Called from UI Button's OnClick event.
    public void StartSpawningBoardPiece(BoardPiece boardPiece)
    {
        if (!isSpawning)
        {
            pieceToSpawn = boardPiece;
            isSpawning = true;
        }
    }

    void MouseClickInput()
    {
        // Spawn a base when currently searching.
        if (isSpawning && selectedFloorTile != null && selectedFloorTile.CurrentTileState != FloorTile.TileState.Occupied)
        {
            isSpawning = false;
            pieceCurrentlySpawning.OccupiedFloorTile = selectedFloorTile;
            pieceCurrentlySpawning.Spawn();

            pieceCurrentlySpawning = pieceToSpawn = null;
        }
        else if (pieceCurrentlySpawning == null)
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
                isSelectingSelectable = true;
            }
        }
    }

    private void HandleFinishedSelectingSelectable()
    {
        if (isSelectingSelectable && floorTilePath.Count > 0)
        {
            MonoBehaviour mg = currentSelecteable as MonoBehaviour;
            MovingBoardPiece movingPlaceable;
            if (movingPlaceable = mg.gameObject.GetComponent<MovingBoardPiece>())
            {
                movingPlaceable.OccupiedFloorTile.CurrentTileState = FloorTile.TileState.Idle;
                movingPlaceable.GivePath(floorTilePath);
            }
            //foreach (FloorTile tile in floorTilePath)
            //{
            //    tile.CurrentTileState = FloorTile.TileState.Idle;
            //}
            floorTilePath.Clear();
        }
        isSelectingSelectable = false;
    }
}
