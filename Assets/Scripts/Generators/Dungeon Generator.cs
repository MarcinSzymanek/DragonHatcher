using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{

    //   Overall TODO:
    // - Add variety to walls and fill-tiles
    // - Select random locations for potential spawning
    
    public GameObject wallTilePrefab = null;

    public GameObject doorPrefab = null;
    public Tilemap tileMap = null;
    public TileBase[] tileToPlace = null;
    private int roomNumber = 1;

    public List<Transform> listOfTeleporters;
    private Transform wallParentTf_;
    public GameObject wallParent;
    private GameObject player;

    private Transform playerTransform;
    private bool entranceTeleporterPlaced = false;
    public List<ObjectTeleportation> listOfScripts;


    struct Square
    {
        public Vector3 position;
    }

    List<Square> squares = new List<Square>();


    void Awake() {
        wallParentTf_ = wallParent.transform;
        player = GameObject.Find("Player");
        listOfScripts = new List<ObjectTeleportation>();
        listOfTeleporters = new List<Transform>();
    }
    void Start()
    {
        //Generating several rooms next to each other
        //Note: Only use un-even numbers for the size of the room as it messes up the tile allignment for the filling
        SpawnSquaresNextToEachOther(5, 19f);
    }

    //Size is hardcoded atm but we can change it later to a parameter
    //Teleportation logic:
    //- When a room is created we need to create a sender and receiver teleporter at each entrance/exti
    //- The first entrance is semi hardcoded as its the entrance into the actual dungeon itself
    //- The First room exit needs to connect into the next room and also place the correct amount of teleporters
    //- A teleporter and a receiver on each entrance and exit where they link respectively themselves

    void CreateRoom(Vector3 position, float size)
    {
        playerTransform = player.transform;
        float acc = 0.5f;
        GameObject room = new GameObject("Room " + roomNumber);
        GameObject initialTeleporter = null;

        if(!entranceTeleporterPlaced) {
            //Create the outer teleporter that we want to enter the dungeon in
            //Currently hard-coded but we can change that for the dungeon entrance later on when integrating
            initialTeleporter = PlaceTeleporter(-0.5f, -10.5f, 0f, doorPrefab, room);
            listOfTeleporters.Add(initialTeleporter.transform);
            entranceTeleporterPlaced = true;
        }
        room.transform.parent = wallParentTf_;
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

                //If not, place a wall
                if(isOuterBorder) {
                    PlaceWall(posX, posY, posZ, wallTilePrefab, room);
                     
                    if(acc == size/2f && posY > 0) {
                        GameObject teleporter = PlaceTeleporter(posX, posY - 1, posZ, doorPrefab, room);
                        listOfTeleporters.Add(teleporter.transform);
                    }
                    if(acc == size/2f && posY < 0) {
                        GameObject teleporter = PlaceTeleporter(posX, posY + 1, posZ, doorPrefab, room); 
                        listOfTeleporters.Add(teleporter.transform);       
                    }
                }
                else 
                {
                    //Else, place filling for the room
                    PlaceTile((int)(posX-0.5), (int)(posY-0.5), (int)(posZ-0.5), tileToPlace[Random.Range(0,tileToPlace.Length)]);
                }
            }
            acc++;
        }
        //Do the connection of the teleporters here
        //Second receiver <- First Sender
        //i <- i+1 (j)
        roomNumber++;
        for(int i = 0; i < listOfScripts.Count - 1; i++) {
            listOfScripts[i].setPlayer(player);
            listOfScripts[i].setObjectToTeleport(playerTransform);
            if (i == 0 || i == 1) {
                listOfScripts[i].setDestination(listOfTeleporters[1]); 
                listOfScripts[1].setDestination(listOfTeleporters[0]);
            }
            else {
                listOfScripts[i].setDestination(listOfTeleporters[i+1]);
                listOfScripts[i-1].setDestination(listOfTeleporters[i]); 
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
        CreateRoom(position, size);
        }
    }


    void PlaceWall(float posX, float posY, float posZ, GameObject prefab, GameObject room)
    {
        Vector3 spawnPosition = new Vector3(posX, posY, posZ);
        Instantiate(prefab, spawnPosition, Quaternion.identity, room.transform);
    }

     void PlaceTile(int posX, int posY, int posZ, TileBase tile)
    {
        //Using instantiate instead breaks everything
        tileMap.SetTile(new Vector3Int(posX, posY, posZ), tile);
    }

    GameObject PlaceTeleporter(float posX, float posY, float posZ, GameObject prefab, GameObject room)
    {
        //Place Receiver
        Vector3 spawnPosition = new Vector3(posX, posY, posZ);
        GameObject teleporter = Instantiate(prefab, spawnPosition, Quaternion.identity, room.transform);

        listOfScripts.Add(teleporter.GetComponent<ObjectTeleportation>());
        return teleporter;
    }
}
