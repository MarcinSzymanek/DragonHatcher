using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject wallTilePrefab = null;
    public GameObject emptyTilePrefab = null;

    struct Square
    {
        public Vector3 position;
        public bool hasSingleEntrance;
    }

    List<Square> squares = new List<Square>();

    void Start()
    {
        // Call the function to generate squares
        GenerateSquares(5);
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

    void CreateSquare(Vector3 position)
    {
        float size = 5f;
        float halfSize = size / 2f;
        float padding = 0.5f; // Padding to leave some space from the outer border

        for (float i = -halfSize; i <= halfSize; i++)
        {
            for (float j = -halfSize; j <= halfSize; j++)
            {
                float posX = position.x + i;
                float posY = position.y + j;
                loat posZ = position.z;

                // Check if the current position is on the outer border
                bool isOuterBorder = Mathf.Abs(i) >= halfSize - padding || Mathf.Abs(j) >= halfSize - padding;

                // Place wallTilePrefab on outer border, else place emptyTilePrefab
                GameObject prefabToPlace = isOuterBorder ? wallTilePrefab : emptyTilePrefab;

                PlaceTile(posX, posY, posZ, prefabToPlace);
            }
        }
    }


    void PlaceTile(float posX, float posY, float posZ, GameObject prefab)
    {
        Vector3 spawnPosition = new Vector3(posX, posY, posZ);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }   
}
