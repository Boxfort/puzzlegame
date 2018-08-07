using System;
using System.Collections.Generic;

// Sets all broken tiles back to thier default tile.
public class FixAllSpell : ISpellBehaviour
{
    public void Execute()
    {
        BoardManager boardManager = BoardManager.GetInstance();
		List<BoardTile> tiles = boardManager.Tiles;
        foreach(BoardTile b in tiles)
        {
			if (b.Tile.id == 20)
			    b.SetTile(b.DefaultTile);
        }
    }
}
