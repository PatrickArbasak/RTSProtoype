using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{

    private TerrainPiece occupiedTerrainPiece;
    public TerrainPiece OccupiedTerrainPiece
    {
        get { return occupiedTerrainPiece; }
        set
        {
            value.isOccupied = (value != null) ? true : false;
            occupiedTerrainPiece = value;
        }
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }
}
