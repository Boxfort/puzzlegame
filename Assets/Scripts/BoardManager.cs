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
    
    GameObject tilePrefab;
    List<TileData> tilesData;
    List<BoardTile> tiles;
    List<int> baseTiles;

    void Awake ()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        tilePrefab = Resources.Load<GameObject>(TILEPREFAB_PATH);
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
            GameObject tileInstance = Instantiate(tilePrefab, transform);
            tileInstance.gameObject.name = i.ToString();
            tiles.Add(tileInstance.GetComponent<BoardTile>());
            int y = i / BOARD_WIDTH;
            int x = i - (y * BOARD_WIDTH);
            tiles[i].SetTile(tilesData[baseTiles[random.Next(baseTiles.Count)]]);
            tiles[i].DefaultTile = tiles[i].Tile;
            tiles[i].Pos = new Point(x, y);
        }
    }
    
    // TODO: DELETE THIS
    public void DUMP_BOARD_STATE()
    {
        foreach(BoardTile b in tiles)
        {
           Debug.Log("DATA : " +
                        "\n name=" + b.Tile.name +
                        "\n id=" + b.Tile.id +
                        "\n sprite=" + b.Tile.sprite);
        }
    }
    
    // TODO: DELETE THIS
    public void DUMP_TILEDATA()
    {
        foreach(TileData t in tilesData)
        {
           Debug.Log(
                        "\n name=" + t.name +
                        "\n id=" + t.id +
                        "\n sprite=" + t.sprite);
        }
    }
    
    public void SetTileAt(Point pos, int tileId)
    {
        if (!IsPointOnBoard(pos))
            return;

        int index = pos.x + (pos.y * BOARD_HEIGHT);
        Debug.Log("setting index " + index);
        
        tiles[index].SetTile(tilesData[tileId]);
    }
    
    public BoardTile GetTileAt(Point pos)
    {
        if (!IsPointOnBoard(pos))
            return null;
    
        int index = pos.x + (pos.y * BOARD_HEIGHT);
        return tiles[index];
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

            return IsPointOnBoard(new Point(x, y));
        }
        return true;
    }
    
    bool IsPointOnBoard(Point point)
    {
        if(point.x < 0 || point.x >= BOARD_WIDTH || point.y < 0 || point.y >= BOARD_HEIGHT)
        {
            return false;
        }

        return true;
    }
}
