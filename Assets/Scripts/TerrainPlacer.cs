using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPlacer : MonoBehaviour {

    public int terrainDimensionNum = 20;
    public GameObject TerrainPiecePrefab;
    //[SerializeField] List<GameObject> terrainPieces = new List<GameObject>();
    float terraingHeight;
    float terraingWidth;

    private void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        terraingHeight = TerrainPiecePrefab.transform.lossyScale.z;
        terraingWidth = TerrainPiecePrefab.transform.lossyScale.x;
        for (int i = 1; i <= terrainDimensionNum; i++)
        {
            for (int j = 1; j <= terrainDimensionNum; j++)
            {
                float xPosition = terraingWidth * j;
                float zPosition = terraingHeight * i;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);
                GameObject tempRoomObject = Instantiate(TerrainPiecePrefab, newPosition, Quaternion.identity) as GameObject;
                tempRoomObject.transform.parent = transform;
                tempRoomObject.name = "terrain_" + j + ", " + i;
            }
        }
    }
}
