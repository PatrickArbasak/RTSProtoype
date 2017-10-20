using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    public enum SpawnedState { spawning, spawned};

    private TerrainPiece occupiedTerrainPiece;
    public TerrainPiece OccupiedTerrainPiece { get { return occupiedTerrainPiece; } set { occupiedTerrainPiece = value; } }

    private SpawnedState currentSpawnedState;
    public SpawnedState CurrentSpawnedState { get { return currentSpawnedState; } set { currentSpawnedState = value; } }

    [SerializeField] private Material highLightMaterial;
    private Renderer palceableRenderer;
    private Material originalMaterial;

    protected virtual void Start()
    {
        currentSpawnedState = SpawnedState.spawning;
        palceableRenderer = GetComponent<Renderer>();
        originalMaterial = palceableRenderer.material;
        palceableRenderer.material = highLightMaterial;
    }

    protected virtual void Update()
    {
        if (currentSpawnedState == SpawnedState.spawning)
        {
            if (!occupiedTerrainPiece.isOccupied)
                palceableRenderer.material.color = new Color(0, 0, 1, 0.5f);
            else
                palceableRenderer.material.color = new Color(1, 0, 0, 0.5f);
        }
    }

    public void Spawn()
    {
        palceableRenderer.material = originalMaterial;
        currentSpawnedState = SpawnedState.spawned;
        occupiedTerrainPiece.isOccupied = true;
    }
}
