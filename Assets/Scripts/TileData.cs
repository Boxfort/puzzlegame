using System.Collections.Generic;
using LitJson;
using UnityEngine;

public struct TileData
{
    public int id;
    public string name;
    public Sprite sprite;
    public int reward;
    public Dictionary<int, int> transitions;

    public TileData(int id, string name, string slug, int reward, Dictionary<int,int> transitions)
    {
        this.id = id;
        this.name = name;
        this.sprite = Resources.Load<Sprite>("Sprites/tiles/" + slug);
        this.reward = reward;
        this.transitions = transitions;
    }
    
    // Explicit cast from JsonData to make code nicer
    public static explicit operator TileData(JsonData json)
    {
        Dictionary<int, int> transitions = new Dictionary<int, int>();

        for (int i = 0; i < json["transitions"].Count; i++)
        {
            transitions.Add((int)json["transitions"][i]["item"], (int)json["transitions"][i]["next"]);
        }

        return new TileData((int)json["id"],
                            (string)json["name"],
                            (string)json["slug"],
                            (int)json["reward"],
                            transitions
                            );
    }
}

