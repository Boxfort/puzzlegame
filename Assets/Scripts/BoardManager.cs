using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class BoardManager : MonoBehaviour 
{
#region singleton
    static BoardManager instance;
    public static BoardManager GetInstance() 
    {
        if (instance == null)
            throw new System.Exception("No BoardManager exists in scene.");

        return instance; 
    }
#endregion

    const string TILEDATA_PATH = "Data/tiles";
    const string TILEPREFAB_PATH = "Prefabs/Tile";

    public const int BOARD_WIDTH  = 4;
    public const int BOARD_HEIGHT = 4;
    GameObject tile;
    public List<TileData> tilesData;
    public List<BoardTile> tiles;

    List<int> baseTiles;

    void Awake ()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        tile = Resources.Load<GameObject>(TILEPREFAB_PATH);
        JsonData tilesJson = JsonHelper.LoadJsonResource(TILEDATA_PATH);
        tiles = new List<BoardTile>();
        tilesData = new List<TileData>();

        for (int i = 0; i < tilesJson.Count; i++)
        {
            tilesData.Add((TileData)tilesJson[i]);
        }

        // TODO: Load these from a file.
        baseTiles = new List<int>()
        {
            0,
            3,
            7,
            11,
            15,
            19
        };

        ConstructBoard();
      
        // Add callbacks
        CardDrag.OnPlaceCard += OnPlaceCard;
        CardDrag.OnCardEnter += OnHoverCard;
        CardDrag.OnCardExit += OnCardExit;
    }

    // Update is called once per frame
    void Update () 
    {
        
    }
    
    void ConstructBoard () 
    {
        System.Random random = new System.Random();

        for (int i = 0; i < BOARD_WIDTH * BOARD_HEIGHT; i++) 
        {
            GameObject tileInstance = Instantiate(tile, transform);
            tileInstance.gameObject.name = i.ToString();
            tiles.Add(tileInstance.GetComponent<BoardTile>());
            int y = i / BOARD_WIDTH;
            int x = i - (y * BOARD_WIDTH);
            tiles[i].SetTile(tilesData[baseTiles[random.Next(baseTiles.Count)]]);
            tiles[i].GetComponent<BoardTile>().Pos = new Point(x, y);
        }
    }
    
    // TODO: Decide what to do on hovering
    //       Maybe come up with different method of highlighting
    void OnHoverCard(Card card, Point atTile)
    {      
        if(IsValidPlacement(card.Shape, atTile))
        {
            foreach(Point p in card.Shape)
            {
                int pos = PointsToIndex(p, atTile);
                tiles[pos].GetComponent<Outline>().enabled = true;
            }
        }
    }

    bool OnPlaceCard(Card card, Point atTile)
    {
        if (IsValidPlacement(card.Shape, atTile))
        {
            for (int i = 0; i < card.Shape.Count; i++)
            {
                int pos = PointsToIndex(card.Shape[i], atTile);
                int next = tiles[pos].GetNextTile(card.Items[i].id);

                // TODO: Find a better solution for default actions
                if (next == -1)
                    next = tilesData.Count - 1; // If no interaction found just use broken tile.
                                        
                tiles[pos].SetTile(tilesData[next]);
            }
            return true;
        }
        
        return false;
    }
    
    void OnCardExit(Card card, Point atTile)
    {
        if (IsValidPlacement(card.Shape, atTile))
        {
            foreach (Point p in card.Shape)
            {
                int pos = PointsToIndex(p, atTile);
                tiles[pos].GetComponent<Outline>().enabled = false;
            }
        }
    }

    // Converts card point and tile point to board index.
    int PointsToIndex(Point p, Point atTile)
    {
        return (p.x + atTile.x + ((-p.y + atTile.y) * BOARD_WIDTH));
    }

    bool IsValidPlacement(List<Point> shape, Point pos)
    {
        foreach(Point p in shape)
        {
            // Translate points to board positions
            int x = p.x + pos.x;
            int y = -p.y + pos.y;
         
            // If outside board bounds
            if(x < 0 || x >= BOARD_WIDTH || y < 0 || y >= BOARD_HEIGHT)
            {
                // ya fucked up
                return false;
            }
        }
        return true;
    }
}
