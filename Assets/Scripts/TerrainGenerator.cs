using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public FloorTile floorTile;
    public int rows;
    public int columns;

    private float tileHeight;
    private float tileWidth;
    private List<FloorTile> tiles = new List<FloorTile>();

    private void Start()
    {
        GenerateTerrain();
        ConnectTileDirections();
    }

    private void GenerateTerrain()
    {
        tileHeight = floorTile.transform.lossyScale.z;
        tileWidth = floorTile.transform.lossyScale.x;
        for (int i = 1; i <= rows; i++)
        {
            for (int j = 1; j <= columns; j++)
            {
                float xPosition = tileWidth * j;
                float zPosition = tileHeight * i;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);
                FloorTile tempRoomObject = Instantiate(floorTile, newPosition, Quaternion.identity);
                tempRoomObject.transform.parent = transform;
                tempRoomObject.name = "tile_" + j + ", " + i;
                tiles.Add(tempRoomObject);
            }
        }
    }

    private void ConnectTileDirections()
    {
        for (int i = 0; i <= tiles.Count - 1; i++)
        {
            FloorTile currentTile = tiles[i];
            
			// Set up North Room
			if (i <= (tiles.Count - 1) - columns)
            {
				currentTile.connnectedTiles.northTile = tiles [i + columns];
			}
			else
            {
				currentTile.connnectedTiles.northTile = null;
			}
			
			// Set up South Room
			if (i >= columns)
            {
				currentTile.connnectedTiles.southTile = tiles [i - columns];
            }
            else
            {
				currentTile.connnectedTiles.southTile = null;
			}

            // Set up West Room
			if (i % columns != 0)
            {
				currentTile.connnectedTiles.westTile = tiles [i - 1];
            }
            else
            {
				currentTile.connnectedTiles.westTile = null;
			}

            // Set up East Room
			if (i % columns != columns - 1)
            {
				currentTile.connnectedTiles.eastTile = tiles [i + 1];
            }
            else
            {
				currentTile.connnectedTiles.eastTile = null;
			}
        }
    }
}
