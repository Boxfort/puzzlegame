using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: Consider creating get methods for individual points/items?
public class Card : MonoBehaviour
{
	const string CARDTILEPREFAB_PATH = "Prefabs/CardTile";

	List<Point> cardShape;
	List<ItemData> cardItems;
	GameObject cardTile;
	Point cardShift;
    
	public Point Shift { get { return cardShift; } }
	public List<Point> Shape { get { return cardShape; } }
	public List<ItemData> Items { get { return cardItems; } }

	public void ConstructCard(List<Point> shape, List<ItemData> items)
	{    
		cardShape = shape;
		cardItems = items;
		cardTile = Resources.Load<GameObject>(CARDTILEPREFAB_PATH);

		cardShift = new Point(0, 0);

		// Build the card GameObject
		for(int i = 0; i < cardShape.Count; i++)
		{
			GameObject instance = Instantiate(cardTile, transform, false);
			float size = instance.GetComponent<RectTransform>().rect.width;
			instance.transform.localPosition = new Vector2(size * cardShape[i].x, size * cardShape[i].y);
			instance.transform.GetChild(0).GetComponent<Image>().sprite = cardItems[i].sprite;

			cardShift.x = cardShape[i].x != 0 ? cardShape[i].x * -1 : cardShift.x;
			cardShift.y = cardShape[i].y != 0 ? cardShape[i].y * -1 : cardShift.y;         
		}
	}   
}