using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    public struct ConnectedTiles
    {
        public FloorTile northTile;
		public FloorTile southTile;
		public FloorTile eastTile;
		public FloorTile westTile;
    };

    public enum TileState {Idle, Occupied, OnPath};
    private TileState currentTileState;

    public ConnectedTiles connnectedTiles;
    public Transform NavPoint;

    [SerializeField] private Material occupiedMaterial;
    [SerializeField] private Material pathMaterial;
    private Material originalMaterial;
    private Renderer tileRenderer;

    public TileState CurrentTileState
    {
        get{return currentTileState; }
        set
        {
            switch (value)
            {
                case TileState.Occupied:
                    tileRenderer.material = occupiedMaterial;
                    break;
                case TileState.OnPath:
                    tileRenderer.material = pathMaterial;
                    break;
                case TileState.Idle:
                    tileRenderer.material = originalMaterial;
                    break;
            }
            currentTileState = value;
        }
    }

    private void Start()
    {
        tileRenderer = gameObject.GetComponent<Renderer>();
        originalMaterial = tileRenderer.material;
        currentTileState = TileState.Idle;
    }

    public bool IsTileConnected(FloorTile tile)
    {
        return (tile == connnectedTiles.northTile || tile == connnectedTiles.westTile || tile == connnectedTiles.eastTile || tile == connnectedTiles.southTile);

    }
}
