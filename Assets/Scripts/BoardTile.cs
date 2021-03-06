﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour
{
    public delegate void OnTileChangedCallback(TileData tile);

    public static event OnTileChangedCallback OnTileChanged;

    TileData tileData;
    TileData defaultTile;
    Image tileImage;
    Outline ol;
    Point pos;
    
    public Point Pos { get; set; }
    public TileData Tile { get { return tileData; } }
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
        Debug.Log("oldTileImage " + tileImage.sprite);
        tileData = tile;
        tileImage.sprite = tile.sprite;
        Debug.Log("newTileImage " + tileImage.sprite);
        OnTileChanged(tile);
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
