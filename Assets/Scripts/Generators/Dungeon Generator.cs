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
    private Transform wallParentTf_;
    public GameObject wallParent;


    struct Square
    {
        public Vector3 position;
    }

    List<Square> squares = new List<Square>();

    void Awake() {
        wallParentTf_ = wallParent.transform;
    }
    void Start()
    {
        //Generating several rooms next to each other
        //Note: Only use un-even numbers for the size of the room as it messes up the tile allignment for the filling
        SpawnSquaresNextToEachOther(5, 19f);
    }

    //Size is hardcoded atm but we can change it later to a parameter
    void CreateRoom(Vector3 position, float size)
    {
        float acc = 0.5f;
        bool teleporterPlaced = false;
        GameObject room = new GameObject("Room " + roomNumber);
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
                    if(acc == size/2f && !teleporterPlaced && posY > 0) {
                        PlaceTeleporterAndReceiver(posX, posY - 1, posZ, doorPrefab, room);
                        teleporterPlaced = true;
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
        roomNumber++;
        //Need to make it so that this is not hardcoded anymore
        //PlaceTeleporter(0.5f, 8.5f, 0f, doorPrefab, room);
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

    void PlaceTeleporterAndReceiver(float posX, float posY, float posZ, GameObject prefab, GameObject room)
    {
        //Place Sender
        Vector3 spawnPosition = new Vector3(posX, posY, posZ);
        GameObject sender = Instantiate(prefab, spawnPosition, Quaternion.identity, room.transform);

        //Attach collider to the door
        BoxCollider2D collider = sender.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

        //Place Receiver
        GameObject receiver = Instantiate(prefab, new Vector3(10.5f, 1.5f, 0), Quaternion.identity);

        //Attach the teleporter script
        ObjectTeleportation teleportScript = sender.AddComponent<ObjectTeleportation>();

        //Get the player
        GameObject player = GameObject.Find("Player");
        Transform playerToTeleport = player.transform;

        //Get the objects for the script
        Transform destination = receiver.transform;

        //Attaching the objects to the script
        teleportScript.setPlayer(player);
        teleportScript.setDestination(destination);
        teleportScript.setObjectToTeleport(playerToTeleport);
    }  
}
