using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{

    //   Overall TODO:
    // - Add variety to walls and fill-tiles
    // - Select random locations for potential spawning
    // - Refactor code so that we parameterize most stuff such as size and room-count
    
    public GameObject wallTilePrefab = null;
    public GameObject emptyTilePrefab = null;
    public Tilemap tileMap = null;
    public TileBase[] tileToPlace = null;
    

    struct Square
    {
        public Vector3 position;
    }

    List<Square> squares = new List<Square>();

    void Start()
    {
        //Generating several rooms next to each other
        //Note: Only use un-even numbers for the size of the room as it messes up the tile allignment for the filling
        SpawnSquaresNextToEachOther(5, 19f);
    }

    //Size is hardcoded atm but we can change it later to a parameter
    void CreateSquare(Vector3 position, float size)
    {
        float halfSize = size / 2f;
        // Padding to leave some space from the outer border
        float padding = 0.5f; 

        for (float i = -halfSize; i <= halfSize; i++)
        {
            for (float j = -halfSize; j <= halfSize; j++)
            {
                float posX = position.x + i;
                float posY = position.y + j;
                float posZ = position.z;

                // Check if the current position is on the outer border
                bool isOuterBorder = Mathf.Abs(i) >= halfSize - padding || Mathf.Abs(j) >= halfSize - padding;

                //Place wallTilePrefab on outer border, else place filling-tiles
                //Adjusting room position is not neccesary as we can simply block out all the other rooms from the player view and have them all next to each other
                //GameObject prefabToPlace = isOuterBorder ? wallTilePrefab : emptyTilePrefab;
                if(isOuterBorder) {
                    PlaceWall(posX, posY, posZ, wallTilePrefab);
                }
                else 
                {
                    //Need to convert here, -0.5 is needed because else the placement is messy
                    PlaceTile((int)(posX-0.5), (int)(posY-0.5), (int)(posZ-0.5), tileToPlace[Random.Range(0,tileToPlace.Length)]);
                }
            }
        }
    }

    void SpawnSquaresNextToEachOther(int amount, float size)
    {
        float gap = 5f;

        for (int i = 0; i < amount; i++)
        {
            Vector3 position = new Vector3(i * (size + gap), 0, 0);
            squares.Add(new Square { position = position });
            CreateSquare(position, size);
        }
    }


    void PlaceWall(float posX, float posY, float posZ, GameObject prefab)
    {
        Vector3 spawnPosition = new Vector3(posX, posY, posZ);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }

     void PlaceTile(int posX, int posY, int posZ, TileBase tile)
    {
        //Using instantiate instead breaks everything
        tileMap.SetTile(new Vector3Int(posX, posY, posZ), tile);
    }   
}
