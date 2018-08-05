using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class CardManager : MonoBehaviour 
{   
	const float SHIFT_AMOUNT = 125.0f;
	const float ACTIVE_SCALE_FACTOR = 1.0f;
	const float HOLD_SCALE_FACTOR = 0.3f;
	const string ITEMDATA_PATH = "Data/items";
	const string CARDPREFAB_PATH = "Prefabs/Card";

	List<List<Point>> cardShapes;
	List<ItemData> itemsData;
    
	GameObject cardPrefab;
    GameObject activeCard;
    GameObject heldCard;
    GameObject holdSlot;   
   
	// Use this for initialization
	void Start () 
	{
		itemsData = new List<ItemData>();
		holdSlot = GameObject.Find("HoldSlot");
		cardPrefab = Resources.Load<GameObject>(CARDPREFAB_PATH);

		// TODO: Load from file
		cardShapes = new List<List<Point>>
        {
            new List<Point> { new Point(0,0)}, // Dot
            new List<Point> { new Point(0,0), new Point(1,0)}, // Line
            new List<Point> { new Point(0,0), new Point(0, 1)}, // Pipe
            new List<Point> { new Point(0,0), new Point(1, 0),  new Point(0, 1)}, // TL
            new List<Point> { new Point(0,0), new Point(1, 0),  new Point(1, 1)}, // TR
            new List<Point> { new Point(0,0), new Point(0, 1),  new Point(1, 1)}, // BL
            new List<Point> { new Point(0,0), new Point(1, 1), new Point(1, 0)}, // BR
        };

		JsonData tilesJson = JsonHelper.LoadJsonResource(ITEMDATA_PATH);
              
        for (int i = 0; i < tilesJson.Count; i++)
        {
            itemsData.Add((ItemData)tilesJson[i]);
        }
	}

    public void NewCard()
	{
		Destroy(activeCard);

		System.Random random = new System.Random();

        GameObject instance = Instantiate(cardPrefab);
        List<Point> shape = cardShapes[random.Next(cardShapes.Count)];
        List<ItemData> items = new List<ItemData>();

        for (int i = 0; i < shape.Count; i++)
        {
            items.Add(itemsData[random.Next(itemsData.Count)]);
        }

        instance.GetComponent<Card>().ConstructCard(shape, items);

		PutCardInMainSlot(instance);
	}

    void PutCardInMainSlot(GameObject card)
	{
		card.transform.SetParent(transform);
        card.transform.position = transform.position;
		Point shift = card.GetComponent<Card>().Shift;
        card.transform.localPosition = new Vector2(shift.x * SHIFT_AMOUNT, shift.y * SHIFT_AMOUNT);
		card.transform.localScale = new Vector2(ACTIVE_SCALE_FACTOR, ACTIVE_SCALE_FACTOR);
		card.GetComponent<CardDrag>().IsDraggable = true;
		activeCard = card;
	}
       
    public void HoldCard()
	{
		if(activeCard)
		{         
            // Put active card into hold slot
			activeCard.transform.SetParent(holdSlot.transform);
			activeCard.transform.position = holdSlot.transform.position;
			activeCard.transform.localScale = new Vector2(HOLD_SCALE_FACTOR, HOLD_SCALE_FACTOR);
			Point shift = activeCard.GetComponent<Card>().Shift;
			activeCard.transform.localPosition = new Vector2((shift.x * SHIFT_AMOUNT) * HOLD_SCALE_FACTOR, (shift.y * SHIFT_AMOUNT) * HOLD_SCALE_FACTOR);
			activeCard.GetComponent<CardDrag>().IsDraggable = false;
            
            // Swap cards
			GameObject temp = heldCard;
			heldCard = activeCard;
			activeCard = temp;

            // If the hold slot had a card, put it in the main slot
			if (activeCard)
            {
				PutCardInMainSlot(activeCard);
            }
		}

	}

	// Update is called once per frame
	void Update () 
	{
		if(!activeCard)
		{
			NewCard();
		}
	}
}
