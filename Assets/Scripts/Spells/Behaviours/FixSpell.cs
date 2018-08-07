using System;
using System.Collections.Generic;

// TODO: Investigate if theres a better way of implementing targetableTiles
public class FixSpell : ISpellBehaviour, ITargetableSpell
{
    Point target = new Point(0,0);
	List<int> targetableTiles = new List<int> { 20 };
    
    public Point Target { get; set; }
    public List<int> TargetableTiles { get { return targetableTiles; } }

    // TODO: Should behaviour access BoardManager directly?
	public void Execute()
	{
		BoardManager boardManager = BoardManager.GetInstance();
        boardManager.SetTileAt(Target, boardManager.GetTileAt(Target).DefaultTile.id);
	}
}
