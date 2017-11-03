using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece : MonoBehaviour
{
    public enum SpawnedState { spawning, spawned};

    private FloorTile occupiedFloorTile;
    public FloorTile OccupiedFloorTile { get { return occupiedFloorTile; } set { occupiedFloorTile = value; } }

    private SpawnedState currentSpawnedState;
    public SpawnedState CurrentSpawnedState { get { return currentSpawnedState; } set { currentSpawnedState = value; } }

    [SerializeField] private Material highLightMaterial = null;
    private Renderer boardPieceRenderer;
    private Material originalMaterial;

    private float heightFromFloor;
    public float HeightFromFloor{get{return heightFromFloor;}private set{heightFromFloor = value;}}


    protected virtual void Start()
    {
        currentSpawnedState = SpawnedState.spawning;
        boardPieceRenderer = GetComponent<Renderer>();
        HeightFromFloor = boardPieceRenderer.bounds.extents.y;
        originalMaterial = boardPieceRenderer.material;
        boardPieceRenderer.material = highLightMaterial;
    }

    protected virtual void Update()
    {
        if (currentSpawnedState == SpawnedState.spawning)
        {
            if (occupiedFloorTile.CurrentTileState != FloorTile.TileState.Occupied)
                boardPieceRenderer.material.color = new Color(0, 0, 1, 0.5f);
            else
                boardPieceRenderer.material.color = new Color(1, 0, 0, 0.5f);
        }
    }

    public void Spawn()
    {
        boardPieceRenderer.material = originalMaterial;
        currentSpawnedState = SpawnedState.spawned;
        occupiedFloorTile.CurrentTileState = FloorTile.TileState.Occupied;
    }
}
