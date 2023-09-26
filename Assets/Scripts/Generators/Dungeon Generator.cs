using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{

    public GameObject wallTilePrefab = null;
    // Start is called before the first frame update
    void Start()
    {
        //Call the CreateSquare function
        CreateSquare(10, 10);
    }


    
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Simple function for placing tiles
    void PlaceTile(float pos_x, float pos_y, float pos_z) {
        Vector3 spawnPosition = new Vector3(pos_x, pos_y, pos_z);
        GameObject newWallTile = Instantiate(wallTilePrefab, spawnPosition, Quaternion.identity);
    }

    //Write a function that can generate a square/rectangle for now made of tiles
    void CreateSquare(float x_length, float y_length) {
        for (float i = 0; i <= x_length; i++) {
            //Need to figure out a smart way that finds a proper position for a given dungeon room to be created
            PlaceTile(5f + i, 5f, 5f);
            PlaceTile(5f, 15f + i, 5f);
            PlaceTile(15f, 5f + i, 5f);
        } 
    }

}
