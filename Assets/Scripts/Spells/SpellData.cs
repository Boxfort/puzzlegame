﻿using UnityEngine;
using LitJson;
using System.Collections.Generic;

public struct SpellData
{
	public int id;
	public string name;
	public Sprite sprite;
    public string description;
    public int price;

	public SpellData(int id, string name, string slug, string description, int price)
    {
		this.id = id;
		this.name = name;
		this.sprite = Resources.Load<Sprite>("Sprites/spells/" + slug);
		this.description = description;
		this.price = price;
    }
    
    // Explicit cast from JsonData to make code nicer
	public static explicit operator SpellData(JsonData json)
	{
		return new SpellData((int)json["id"],
							 (string)json["name"],
							 (string)json["slug"],
							 (string)json["description"],
					         (int)json["price"]);
	}
}