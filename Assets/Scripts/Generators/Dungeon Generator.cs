using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject wallTilePrefab = null;
    public GameObject emptyTilePrefab = null;
    public Tilemap tileMap = null;
    public TileBase tileToPlace = null;

    struct Square
    {
        public Vector3 position;
        public bool hasSingleEntrance;
    }

    List<Square> squares = new List<Square>();

    void Start()
    {
        // Call the function to generate squares
        //GenerateSquares(5);
        // Try to spawn squares next to each other for now
        SpawnSquaresNextToEachOther(5, 5f);
    }

    void GenerateSquares(int amount)
    {
        float size = 5f;
        float gap = 1f; // Gap between squares

        // Generate x-amount squares positioned randomly
        for (int i = 0; i < amount; i++)
        {
            bool roomPlaced = false;
            Vector3 position = Vector3.zero;

            // Try placing the room until a non-overlapping position is found
            while (!roomPlaced)
            {

                float x = Random.Range(-15f, 15f); // Random x position within a range
                float y = Random.Range(-15f, 15f); // Random y position within a range
                position = new Vector3(x, y, 0);

                // Check for collisions with existing rooms
                bool collisionDetected = false;
                foreach (var square in squares)
                {
                    if (Mathf.Abs(position.x - square.position.x) < size + gap && Mathf.Abs(position.y - square.position.y) < size + gap)
                    {
                        collisionDetected = true;
                        break;
                    }
                }

                // If no collision, place the room
                if (!collisionDetected)
                {
                    roomPlaced = true;
                }
            }

            squares.Add(new Square { position = position });
            CreateSquare(position);
        }
    }

    //Size is hardcoded atm but we can change it later to a parameter
    void CreateSquare(Vector3 position)
    {
        float size = 6f;
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

                // Place wallTilePrefab on outer border, else place emptyTilePrefab
                //GameObject prefabToPlace = isOuterBorder ? wallTilePrefab : emptyTilePrefab;
                if(isOuterBorder) {
                    PlaceWall(posX, posY, posZ, wallTilePrefab);
                }
                else 
                {
                    //Need to convert here, -0.5 is needed because else the placement is messy
                    PlaceTile((int)posX, (int)posY, (int)posZ, tileToPlace);
                }
                //This is where we would usually call the placeWall function but simply with the empty prefab
                //New Error: Some tiles simply dont get placed even though they should
            }
        }
    }

void SpawnSquaresNextToEachOther(int amount, float size)
{
    float gap = 1f;

    for (int i = 0; i < amount; i++)
    {
        Vector3 position = new Vector3(i * (size + gap), 0, 0);
        squares.Add(new Square { position = position });
        CreateSquare(position);
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
        tileMap.SetTile(new Vector3Int(posX, posY, posZ), tileToPlace);
    }   
}
