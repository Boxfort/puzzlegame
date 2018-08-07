using System.Collections.Generic;

public interface ITargetableSpell
{
    Point Target { get; set; }
    List<int> TargetableTiles { get; }
}

