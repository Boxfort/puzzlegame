using System;
using UnityEngine;
using LitJson;

public struct ItemData
{
	public int id;
	public string name;
	public Sprite sprite;

	public ItemData(int id, string name, string slug)
    {
        this.id = id;
        this.name = name;
        this.sprite = Resources.Load<Sprite>("Sprites/items/" + slug);
    }

    // Explicit cast from JsonData to make code nicer
    public static explicit operator ItemData(JsonData json)
    {      
		return new ItemData((int)json["id"],
                            (string)json["name"],
                            (string)json["slug"]
                            );
    }
}
