using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using AIStrategies;
using System.Xml;
using System.Linq;

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
    public GameObject counter = null;

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
    private ResourceSpawner resourceSpawner;

    private Count counterScript;
    private Transform playerTransform;
    private bool entranceTeleporterPlaced = false;
    private bool exitTeleporterPlaced = false;
    private List<ObjectTeleportation> listOfScripts;
    private List<Vector3> listOfRandomPositions;
    private EnemySpawner spawner;
    private Vector3 randomPosition = Vector3.zero;
    private List<GameObject> allRooms = null;
    private Vector3 rewardPosition = Vector3.zero;
    private Transform lastRoomTransform = null;
    private bool allRoomsEmpty = false;
    private Vector3 potionPosition;
    public GameObject potion;

	private int roomsLeftToClear = 0;

    [field: SerializeField]
    private int minEnemyPerRoom;
    [field: SerializeField]
    private int maxEnemyPerRoom;

	[field: SerializeField]
	private int minRoomSize;
	[field: SerializeField]
	private int maxRoomSize;
	
	GameObject dungeonReward_;
	GameObject activeReward_;
    struct Square
    {
        public Vector3 position;
    }

    List<Square> squares = new List<Square>();
    
	public void SetDungeonGenerator(int difficulty, GameObject reward){
		//Generating several rooms next to each other
		//Note: Only use un-even numbers for the size of the room as it messes up the tile allignment for the filling
		numberOfRooms = UnityEngine.Random.Range(2, 4) + difficulty * 2;
		minEnemyPerRoom = minEnemyPerRoom + difficulty;
		maxEnemyPerRoom = maxEnemyPerRoom + difficulty * 2;
		maxRoomSize = maxRoomSize + difficulty;
		if(maxRoomSize % 2 == 1) maxRoomSize++;
		
		tileMap = GameObject.Find("MapBackground").GetComponent<Tilemap>();
		wallParent = GameObject.Find("MapForeground");
		wallParentTf_ = wallParent.transform;
		player = GameObject.Find("Player");
		listOfScripts = new List<ObjectTeleportation>();
		listOfRandomPositions = new List<Vector3>();
		listOfTeleporters = new List<Transform>();
		spawner = GetComponent<EnemySpawner>();
		allRooms = new List<GameObject>();
		rewardPosition = new Vector3(32f, 0.6f, 0f);
        potionPosition = new Vector3(0f, 0f, 0f);
		resourceSpawner = GetComponent<ResourceSpawner>();
		
		spawner.SetDifficulty(difficulty);
		spawner.SetAIStrategy(new AIStrategies.StrategyScanForPlayer());
		dungeonReward_ = reward;
		SpawnSquaresNextToEachOther(numberOfRooms, sizeOfRooms);
	}
	
	void OnRoomCleared(){
		roomsLeftToClear--;
		if(roomsLeftToClear == 0){
			Debug.Log("Room cleared! Rooms left: " + roomsLeftToClear);
			SpawnReward();	
		}
	}
	
	void SpawnReward(){
		lastRoomTransform = allRooms[allRooms.Count - 1].transform;
		Instantiate(potion, potionPosition, Quaternion.identity, allRooms[allRooms.Count - 1].transform);
		listOfScripts[listOfScripts.Count-1].DisableTeleportation();
		activeReward_ = Instantiate(dungeonReward_, rewardPosition, Quaternion.identity, allRooms[allRooms.Count - 1].transform);
		var notifier = activeReward_.AddComponent<NotifyOnPickup>();
		notifier.pickup += EnableFinalTp;
	}
	
	void EnableFinalTp(){
		listOfScripts[listOfScripts.Count-1].EnableTeleportation();
	}
	
	void CreateRoom(Vector3 position, float size, bool singleTeleporter = false)
    {
        playerTransform = player.transform;
        float acc = 0.5f;
        GameObject room = new GameObject("Room " + roomNumber);
        room.AddComponent<Count>();
        allRooms.Add(room);
        counterScript = room.transform.GetComponent<Count>();
	    counterScript.setId(roomNumber);
	    counterScript.roomEmpty += OnRoomCleared;
	    roomsLeftToClear++;
        GameObject initialTeleporter = null;
        //if (!entranceTeleporterPlaced)
        //{
        //    //Create the outer teleporter that we want to enter the dungeon in
        //    //Currently hard-coded but we can change that for the dungeon entrance later on when integrating
	    //    // initialTeleporter = PlaceTeleporter(-99999f, -9999f, 0f, doorPrefab, room);
        //    listOfTeleporters.Add(initialTeleporter.transform);
        //    entranceTeleporterPlaced = true;
        //}
	    room.transform.parent = wallParentTf_;
        
	    int roomSize = UnityEngine.Random.Range(minRoomSize, maxRoomSize);
	    if(roomSize % 2 == 0) roomSize++;
	    float halfSize = roomSize / 2f;

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
                if (isOuterBorder)
                {
                    if ((Mathf.Approximately(posX, position.x - halfSize) && Mathf.Approximately(posY, position.y - halfSize)))
                    {
                        PlaceWall(posX + 1f, posY, posZ, wallTileCornerBottomLeftPrefab[Random.Range(0, wallTileCornerBottomLeftPrefab.Length)], room);
                    }
                    else if ((Mathf.Approximately(posX, position.x + halfSize) && Mathf.Approximately(posY, position.y - halfSize)))
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
                    else if (Mathf.Approximately(posX, position.x - halfSize))
                    {
                        PlaceWall(posX + 1f, posY, posZ, wallTilePrefab[Random.Range(0, wallTilePrefab.Length)], room);
                    }
                    else if (Mathf.Approximately(posX, position.x + halfSize))
                    {
                        PlaceWall(posX - 1f, posY, posZ, wallTileRightPrefab[Random.Range(0, wallTileRightPrefab.Length)], room);
                    }
                    else if (Mathf.Approximately(posY, position.y - halfSize))
                    {
                        PlaceWall(posX, posY, posZ, wallTileBottomPrefab[Random.Range(0, wallTileBottomPrefab.Length)], room);
                    }
                    else if (Mathf.Approximately(posY, position.y + halfSize))
                    {
                        PlaceWall(posX, posY, posZ, wallTileTopPrefab[Random.Range(0, wallTileTopPrefab.Length)], room);
                    }

	                if (acc == roomSize / 2f && posY > 0)
                    {
                        GameObject teleporter = PlaceTeleporter(posX, posY - 1.5f, posZ, doorPrefab, room);
                        listOfTeleporters.Add(teleporter.transform);
                        var tpScript = teleporter.GetComponent<ObjectTeleportation>();
                        counterScript.AddPortalScript(tpScript);
                    }
	                if (!singleTeleporter && acc == roomSize / 2f && posY < 0)
                    {
                        GameObject teleporter = PlaceTeleporter(posX, posY + 1.3f, posZ, doorPrefab, room);
                        listOfTeleporters.Add(teleporter.transform);
                        var tpScript = teleporter.GetComponent<ObjectTeleportation>();
                        counterScript.AddPortalScript(tpScript);
                    }
                }
                else
                {
                    //Else, place filling for the room
                    PlaceTile((int)(posX - 0.5), (int)(posY - 0.5), (int)(posZ - 0.5), tileToPlace[Random.Range(0, tileToPlace.Length)]);
                }
            }
            acc++;
        }

        //Get random places within the dungeon to parse to the spawner
	    int randomValue = Random.Range(minEnemyPerRoom, maxEnemyPerRoom + 1);
        List<Vector3> randomPositions = new List<Vector3>();
        float offset = 1f;
        for (int i = 0; i < randomValue; i++)
        {
            float MinX = position.x - halfSize + offset;
            float MaxX = position.x + halfSize - offset;
            float MinY = position.y - halfSize + offset;
            float MaxY = position.y + halfSize - offset;

            float RandomX = Random.Range(MinX, MaxX);
            float RandomY = Random.Range(MinY, MaxY);
            randomPositions.Add(new Vector3(RandomX, RandomY, 0));
        }

        for (int g = 0; g < randomPositions.Count; g++)
        {
            var enemy = spawner.Spawn(randomPositions[g], room.transform);
            enemy.GetComponent<DeathController>().objectDied += counterScript.OnEnemyDeath;
        }
        counterScript.setEnemyCount(randomPositions.Count);
        if(roomNumber == numberOfRooms)
        {
        	Debug.Log(randomPositions.Count);
	        if(randomPositions.Count == 1){
		        potionPosition = randomPositions[randomPositions.Count - 1];
		        rewardPosition = randomPositions[randomPositions.Count - 1];
	        }
	        else{
		        potionPosition = randomPositions[randomPositions.Count - 2];
		        rewardPosition = randomPositions[randomPositions.Count - 1];
	        }
        }
		
		

        //Do the connection of the teleporters here
        //Second receiver <- First Sender
        //i <- i+1 (j)
        roomNumber++;
     
        for (int i = 0; i < listOfTeleporters.Count; i++)
        {

            listOfScripts[i].setPlayer(player);
            listOfScripts[i].setObjectToTeleport(playerTransform);
            if (i % 2 == 0)
            {
                // Even-numbered teleporters teleport into rooms (forward)
                listOfScripts[i].setDestination(listOfTeleporters[(i + 1) % listOfTeleporters.Count]);
            }
            else
            {
                // Odd-numbered teleporters teleport backwards
                int prevIndex = (i == 0) ? listOfTeleporters.Count - 1 : i - 1;
                listOfScripts[i].setDestination(listOfTeleporters[prevIndex]);
            }
        }

        // Handle the last teleporter
        int lastIndex = listOfTeleporters.Count - 1;
        listOfScripts[lastIndex].setPlayer(player);
        listOfScripts[lastIndex].setObjectToTeleport(playerTransform);
        if (lastIndex % 2 == 0)
        {
            // Last teleporter is even-numbered, teleports into the first room
            listOfScripts[lastIndex].setDestination(listOfTeleporters[0]);
        }
        else
        {
            // Last teleporter is odd-numbered, teleports backwards to the previous room
            int prevIndex = (lastIndex == 0) ? listOfTeleporters.Count - 1 : lastIndex - 1;
            listOfScripts[lastIndex].setDestination(listOfTeleporters[prevIndex]);
        }
        

        //Spawn random ressources into the dungeon for pickup
        List<Vector3> randomRessourcePositions = new List<Vector3>();
        int value = 0;
	    if(roomSize < 15)
        {
            value = 3;
        }
	    if(15 < roomSize && roomSize < 25)
        {
            value = 5;
        }
	    if(roomSize > 35)
        {
            value = 8;
        }
        for (int i = 0; i < value; i++)
        {
            float MinX = position.x - halfSize + offset;
            float MaxX = position.x + halfSize - offset;
            float MinY = position.y - halfSize + offset;
            float MaxY = position.y + halfSize - offset;

            float RandomX = Random.Range(MinX, MaxX);
            float RandomY = Random.Range(MinY, MaxY);
           
            randomRessourcePositions.Add(new Vector3(RandomX, RandomY, 0));
        }
        for (int j = 0; j < randomRessourcePositions.Count(); j++)
        {
            resourceSpawner.Spawn(randomRessourcePositions[j], room.transform);
        }

		
        //I cant be asked dealing with the first two teleporters, so we move them to narnia!
	    //listOfTeleporters[0].transform.position = new Vector3(99999f, 99999f, 0f);
	    //listOfTeleporters[1].transform.position = new Vector3(99999f, 99999f, 0f);
    }

    void SpawnSquaresNextToEachOther(int amount, float size)
    {
	    float gap = 30f;

        for (int i = 0; i < amount; i++)
        {
        	Debug.Log("Room number: " + i.ToString());
            Vector3 position = new Vector3(i * (size + gap), 0, 0);
	        squares.Add(new Square { position = position });
	        bool singleTeleporter = false;
	        if(i == 0) singleTeleporter = true;
	        CreateRoom(position, size, singleTeleporter);
        }
	    var finaltp = listOfScripts[listOfScripts.Count -1];
	    finaltp.setFinalTeleporter();
	    finaltp.DisableTeleportation();
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
