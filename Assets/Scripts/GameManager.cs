using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour 
{
#region singleton
    static GameManager instance = new GameManager();
    public static GameManager GetInstance() 
    {
        if (instance == null)
            throw new System.Exception("No GameManager exists in scene.");

        return instance; 
    }
#endregion
    
    int money = 0;
    Text moneyText;

	public int Money { get { return money; } }

	void Awake ()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyText.text = "$" + money.ToString();

        BoardTile.OnTileChanged += OnTileChanged;
        CardDrag.OnCardPlaced += OnTurnEnd;
		SpellManager.OnSpellCast += OnSpellCast;
    }

    // Update is called once per frame
    void Update () 
    {
        
    }

    void OnTurnEnd()
    {
        // Re-Enable buttons

    }
    
    void OnSpellCast(SpellData spell)
    {
		AddMoney(-spell.price);
    }
    
    void OnTileChanged(TileData tile)
    {
		AddMoney(tile.reward);
    }

    void AddMoney(int x)
    {
        money += x;
        moneyText.text = "$" + money.ToString();
    }
}
