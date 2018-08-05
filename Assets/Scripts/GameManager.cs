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
    
    //Dictionary<TileType, int> rewards;
    int money = 0;
    Text moneyText;
    
    void Awake ()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start () 
    {
        // TODO: Fix rewards
        /*
        rewards = new Dictionary<TileType, int>
        {
            {TileType.Golem2, 10},
            {TileType.Venus3, 15},
            {TileType.Cauldron3, 20},
            {TileType.MushMonster3, 30},
            {TileType.LavaMonster3, 30}
        };

        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyText.text = "$" + money.ToString();
        */

        BoardTile.OnTileChanged += OnTileChanged;
        CardDrag.OnCardPlaced += OnTurnEnd;
    }

    // Update is called once per frame
    void Update () 
    {
        
    }

    // TODO: Fix gamemanager tile changed event
    void OnTileChanged(int id)
    {
        /*
        int reward;
        if (rewards.TryGetValue(t, out reward))
        {
            AddMoney(reward);
        }
        */
    }
    
    void OnTurnEnd()
    {
        // Re-Enable buttons

    }

    void AddMoney(int x)
    {
        money += x;
        moneyText.text = "$" + money.ToString();
    }
}
