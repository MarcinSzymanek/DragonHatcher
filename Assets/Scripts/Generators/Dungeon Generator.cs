using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using AIStrategies;

public class DungeonGenerator : MonoBehaviour
{

    //   Overall TODO:
    // - Add variety to walls and fill-tiles
    // - Select random locations for potential spawning
    
    public GameObject[] wallTilePrefab = null;
    public GameObject[] wallTileRightPrefab = null;
    public GameObject[] wallTileBottomPrefab = null;
    public GameObject[] wallTileTopPrefab = null;
    public GameObject[] wallTileCornerBottomLeftPrefab = null;
    public GameObject[] wallTileCornerBottomRightPrefab = null;
    public GameObject[] wallTileCornerTopRightPrefab = null;
    public GameObject[] wallTileCornerTopLeftPrefab = null;

    public GameObject doorPrefab = null;
    public int numberOfRooms = 0;
    public float sizeOfRooms = 0;
    private Tilemap tileMap = null;
    public TileBase[] tileToPlace = null;
    private int roomNumber = 1;

    private List<Transform> listOfTeleporters;
    private Transform wallParentTf_;
    private GameObject wallParent;
    private GameObject player;

    private Transform playerTransform;
    private bool entranceTeleporterPlaced = false;
    private bool exitTeleporterPlaced = false;
    private List<ObjectTeleportation> listOfScripts;
    private List <Vector3> listOfRandomPositions;
    private EnemySpawner spawner;
    private Vector3 randomPosition = Vector3.zero;

    struct Square
    {
        public Vector3 position;
    }

    List<Square> squares = new List<Square>();


    void Awake() {
        tileMap = GameObject.Find("MapBackground").GetComponent<Tilemap>();
        wallParent = GameObject.Find("MapForeground");
        wallParentTf_ = wallParent.transform;
        player = GameObject.Find("Player");
        listOfScripts = new List<ObjectTeleportation>();
        listOfRandomPositions = new List<Vector3>();
        listOfTeleporters = new List<Transform>();
	    spawner = GetComponent<EnemySpawner>();
	    spawner.SetAIStrategy(new AIStrategies.StrategyScanForPlayer());
    }
    void Start()
    {
        //Generating several rooms next to each other
        //Note: Only use un-even numbers for the size of the room as it messes up the tile allignment for the filling
        SpawnSquaresNextToEachOther(numberOfRooms, sizeOfRooms);
    }

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
                bool isOuterBorder = Mathf.Abs(i) >= halfSize || Mathf.Abs(j) >= halfSize - padding;

                //If not, place a wall
                if(isOuterBorder) {
                    Vector3 test = new Vector3(posX, posY, posZ);
                    Debug.Log(test);
                    if ((Mathf.Approximately(posX, position.x - halfSize) && Mathf.Approximately(posY, position.y - halfSize)))
                    {
                        PlaceWall(posX + 1f, posY, posZ, wallTileCornerBottomLeftPrefab[Random.Range(0, wallTileCornerBottomLeftPrefab.Length)], room);
                    }
                    else if((Mathf.Approximately(posX, position.x + halfSize) && Mathf.Approximately(posY, position.y - halfSize)))
                    {
                        PlaceWall(posX - 1f, posY, posZ, wallTileCornerBottomRightPrefab[Random.Range(0, wallTileCornerBottomRightPrefab.Length)], room);
                    }
                    if (Mathf.Approximately(posX, position.x - halfSize) && Mathf.Approximately(posY, position.y + halfSize))
                    {
                        PlaceWall(posX + 1f, posY, posZ, wallTileCornerTopLeftPrefab[Random.Range(0, wallTileCornerTopLeftPrefab.Length)], room);
                    }
                    else if (Mathf.Approximately(posX, position.x + halfSize) && Mathf.Approximately(posY, position.y + halfSize))
                    {
                        PlaceWall(posX - 1f, posY, posZ, wallTileCornerTopRightPrefab[Random.Range(0, wallTileCornerTopRightPrefab.Length)], room);
                    }
                    else if(Mathf.Approximately(posX, position.x - halfSize))
                    {
                        PlaceWall(posX + 1f, posY, posZ, wallTilePrefab[Random.Range(0, wallTilePrefab.Length)], room);
                    }
                    else if(Mathf.Approximately(posX, position.x + halfSize))
                    {
                        PlaceWall(posX - 1f, posY, posZ, wallTileRightPrefab[Random.Range(0, wallTileRightPrefab.Length)], room);
                    }
                    else if(Mathf.Approximately(posY, position.y - halfSize))
                    {
                        PlaceWall(posX, posY, posZ, wallTileBottomPrefab[Random.Range(0, wallTileBottomPrefab.Length)], room);
                    }
                    else if(Mathf.Approximately(posY, position.y + halfSize))
                    {
                        PlaceWall(posX, posY, posZ, wallTileTopPrefab[Random.Range(0, wallTileTopPrefab.Length)], room);
                    }

                    if (acc == size/2f && posY > 0) {
                        GameObject teleporter = PlaceTeleporter(posX, posY - 1.5f, posZ, doorPrefab, room);
                        listOfTeleporters.Add(teleporter.transform);
                    }
                    if(acc == size/2f && posY < 0) {
                        GameObject teleporter = PlaceTeleporter(posX, posY + 1.3f, posZ, doorPrefab, room); 
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

        //Get random places within the dungeon to parse to the spawner

        float MinX = position.x - halfSize + 0.5f;
        float MaxX = position.x + halfSize - 0.5f;
        float MinY = position.y - halfSize + 0.5f;
        float MaxY = position.y + halfSize - 0.5f;

        float RandomX = Random.Range(MinX, MaxX);
        float RandomY = Random.Range(MinY, MaxY);

        int random = Random.Range(3, 10);
        Vector3 randomPosition = new Vector3(RandomX, RandomY, 0f);
        
        for (int g = 0; g < random; g++ )
        {
            spawner.Spawn(randomPosition, room.transform);
        }
        

        //Do the connection of the teleporters here
        //Second receiver <- First Sender
        //i <- i+1 (j)
        roomNumber++;
        if (listOfTeleporters.Count >= 2) {
        for (int i = 0; i < listOfTeleporters.Count - 1; i++) {
            listOfScripts[i].setPlayer(player);
            listOfScripts[i].setObjectToTeleport(playerTransform);
            if (i % 2 == 0) {
                // Even-numbered teleporters teleport into rooms (forward)
                listOfScripts[i].setDestination(listOfTeleporters[(i + 1) % listOfTeleporters.Count]);
            } else {
                // Odd-numbered teleporters teleport backwards
                int prevIndex = (i == 0) ? listOfTeleporters.Count - 1 : i - 1;
                listOfScripts[i].setDestination(listOfTeleporters[prevIndex]);
            }
        }

        // Handle the last teleporter
        int lastIndex = listOfTeleporters.Count - 1;
        listOfScripts[lastIndex].setPlayer(player);
        listOfScripts[lastIndex].setObjectToTeleport(playerTransform);
        if (lastIndex % 2 == 0) {
            // Last teleporter is even-numbered, teleports into the first room
            listOfScripts[lastIndex].setDestination(listOfTeleporters[0]);
        } else {
            // Last teleporter is odd-numbered, teleports backwards to the previous room
            int prevIndex = (lastIndex == 0) ? listOfTeleporters.Count - 1 : lastIndex - 1;
            listOfScripts[lastIndex].setDestination(listOfTeleporters[prevIndex]);
            }
        }
    }

    void SpawnSquaresNextToEachOther(int amount, float size)
    {   
        float gap = 20f;

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
