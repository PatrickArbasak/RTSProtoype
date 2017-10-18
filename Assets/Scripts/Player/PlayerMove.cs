using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    //This is a repeated object in the Base class. PlayerInput should place a TerrainPlacable class, that both base and PlayerMove inherit from.
    private TerrainPiece occupiedTerrainPiece;
    public TerrainPiece OccupiedTerrainPiece { get { return occupiedTerrainPiece; } set { occupiedTerrainPiece = value; } }

}
