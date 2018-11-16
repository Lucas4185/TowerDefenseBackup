using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tilePrefab;

    [SerializeField]
    private CameraMovement cameraMovement;

    private Point spawnBegin ,endSpawn;

    [SerializeField]
    private GameObject beginPortal;

    [SerializeField]
    private GameObject endPortal;

    public Portal BeginPortal { get; set; }

    [SerializeField]
    private Transform map;

    public Dictionary<Point, TileScript> Tiles { get; set; }
    public int xPos;
    public int yPos;

    private Stack<Node> finalPath;

    public Stack<Node> Path
    {
        get
        {
            if (finalPath == null)
            {
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(finalPath));
        }
    }


    public LevelManager spawner { get; set; }

    private Point mapSize;


    public float TileSize
    {
        get { return tilePrefab[0].GetComponent<SpriteRenderer>().bounds.size.x; }
    }

    // Use this for initialization
    void Start()
    {
        mapMaker();
    }


    // Update is called once per frame
    void Update()
    {

    }




    private void mapMaker()
    {

        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapXsize = mapData[0].ToCharArray().Length;
        int mapYsize = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        //zorgt er voor dat de grid goed in de camera spawned
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = yPos; y < mapYsize; y++)
        {

            char[] newTiles = mapData[y].ToCharArray();
            for (int x = xPos; x < mapXsize; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        maxTile = Tiles[new Point(mapXsize - 1, mapYsize - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPortals();
    }

    private void SpawnPortals()
    {
        spawnBegin = new Point(0, 4);
        
        GameObject tmp = (GameObject)Instantiate(beginPortal,Tiles[spawnBegin].transform.position,Quaternion.identity);
        BeginPortal = tmp.GetComponent<Portal>();
        BeginPortal.name = "beginPortal";


        endSpawn = new Point(11, 6);

        Instantiate(endPortal, Tiles[endSpawn].transform.position, Quaternion.identity);

    }

    //dit maakt de tile en zet hem op de goede plek
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefab[tileIndex]).GetComponent<TileScript>();

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);



    }


    //deze functie zorgt er voor dat de Level.txt in de Resources folder geladen word
    private string[] ReadLevelText()
    {
        TextAsset data = Resources.Load("Level") as TextAsset;

        string tmpData = data.text.Replace(Environment.NewLine, string.Empty);

        return tmpData.Split('-');
    }


    public bool InBounds(Point position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < mapSize.x && position.y < mapSize.y;
    }

    public void GeneratePath()
    {
        finalPath = Astar.GetPath(spawnBegin,endSpawn);
    }
    
}
