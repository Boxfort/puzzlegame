using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour
{
	public delegate void OnTileChangedCallback(int id);

	public static event OnTileChangedCallback OnTileChanged;

	TileData tileData;
	Image tileImage;
	Outline ol;
	Point pos;
    
	public Point Pos { get { return pos; } }

	void Awake () 
	{
		ol = GetComponent<Outline>();
		tileImage = transform.GetChild(0).GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
    
	public void SetPosition(Point atPos) 
	{
		pos = atPos;
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
