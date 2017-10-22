using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {

    public int floorDimensionNum = 20;
    public GameObject FloorTile;
    float tileHeight;
    float tileWidth;

    private void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        tileHeight = FloorTile.transform.lossyScale.z;
        tileWidth = FloorTile.transform.lossyScale.x;
        for (int i = 1; i <= floorDimensionNum; i++)
        {
            for (int j = 1; j <= floorDimensionNum; j++)
            {
                float xPosition = tileWidth * j;
                float zPosition = tileHeight * i;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);
                GameObject tempRoomObject = Instantiate(FloorTile, newPosition, Quaternion.identity) as GameObject;
                tempRoomObject.transform.parent = transform;
                tempRoomObject.name = "tile_" + j + ", " + i;
            }
        }
    }
}
