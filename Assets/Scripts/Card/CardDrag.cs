using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

// TODO: Fix Card dragging, if click really quickly after dropping you can change the startPosition and the card is frozen in place.
public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public delegate bool OnPlaceCardCallback (Card card, Point p);
	public delegate void OnCardEnterCallback (Card card, Point p);
	public delegate void OnCardExitCallback(Card card, Point p);
	public delegate void OnCardPlacedCallback();

	public static event OnPlaceCardCallback OnPlaceCard;
	public static event OnCardEnterCallback OnCardEnter;
	public static event OnCardExitCallback OnCardExit;
	public static event OnCardPlacedCallback OnCardPlaced;

	Vector3 startPosition;
	Vector2 offset;
	GameObject previous;
	GameObject current;
    
	Card card;
   
	float lastPos;
	float rotForce = 1.0f;
	bool isDragable = true;
	bool found;

	public bool IsDraggable { get { return isDragable; } set { isDragable = value; }}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (!isDragable)
			return;
		
		startPosition = transform.position;
		offset = (Vector2)transform.position - eventData.position;
	}

	public void OnDrag (PointerEventData eventData)
	{
		if (!isDragable)
            return;

		transform.position = eventData.position + offset;
		List<RaycastResult> results = new List<RaycastResult>();
        
		eventData.position = card.transform.GetChild(0).position;

		EventSystem.current.RaycastAll(eventData, results);

		found = false;
        
		foreach (RaycastResult r in results)
		{
			current = r.gameObject;

			if (current.tag == "BoardTile")
			{
				found = true;
				if (previous != current)
                {
					if (previous)
					    OnCardExit(card, previous.GetComponent<BoardTile>().Pos);
					
					OnCardEnter(card, current.GetComponent<BoardTile>().Pos);
                    previous = current;
                }

				return;
			}
		}

		if(!found)
		{
			if(previous)
			    OnCardExit(card, previous.GetComponent<BoardTile>().Pos);
			
            previous = null;
		}

	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if (!isDragable)
            return;
        
		transform.position = startPosition;

		if(found && OnPlaceCard != null)
		{
			if (OnPlaceCard(card, current.GetComponent<BoardTile>().Pos))
			{
				OnCardPlaced();
				Destroy(gameObject);
			}
		}
	}

    void Start()
	{
		card = GetComponent<Card>();
	}

	void Update()
	{

	}
}
