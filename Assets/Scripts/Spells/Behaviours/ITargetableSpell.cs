using System.Collections.Generic;

// HACK: Maybe this interface shouldnt exist and behaviour should be entirely defined by each spell ?
public interface ITargetableSpell
{
    Point Target { get; set; }
    List<int> TargetableTiles { get; }
}

