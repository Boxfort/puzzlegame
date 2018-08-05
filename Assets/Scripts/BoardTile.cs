using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour
{
    public delegate void OnTileChangedCallback(int id);

    public static event OnTileChangedCallback OnTileChanged;

    TileData tileData;
    TileData defaultTile;
    Image tileImage;
    Outline ol;
    Point pos;
    
    public Point Pos { get; set; }
    public TileData DefaultTile { get; set; }

    void Awake () 
    {
        ol = GetComponent<Outline>();
        tileImage = transform.GetChild(0).GetComponent<Image>();
    }
    
    // Update is called once per frame
    void Update () 
    {
        
    }

    public void SetTile(TileData tile)
    {
        tileData = tile;
        tileImage.sprite = tile.sprite;
        OnTileChanged(tile.id);
    }

    public int GetNextTile(int item)
    {
        int next;
        if(tileData.transitions.TryGetValue(item, out next))
        {
            return next;
        }

        return -1;
    }   
   
}
