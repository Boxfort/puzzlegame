using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    #region singleton
    static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance == null)
            throw new System.Exception("No GameManager exists in scene.");

        return instance;
    }
    #endregion

    public delegate void OnTileSelectedCallback(TileData tile, Point pos);

    public static event OnTileSelectedCallback OnTileSelected;

    public int money;
    Text moneyText;
    GameState gameState;

    public int Money { get { return money; } }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Use this for initialization
    void Start()
    {
        moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyText.text = "$" + money.ToString();
        gameState = GameState.Playing;

        // Callbacks
        BoardTile.OnTileChanged += OnTileChanged;
        CardDrag.OnCardPlaced += OnTurnEnd;
        SpellManager.OnSpellCast += OnSpellCast;
        SpellManager.OnSpellBeginTarget += BeginTargeting;
        SpellManager.OnSpellEndTarget += EndTargeting;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.CastingSpell)
        {
            // FIXME: Preprocessor statements for Android platform
            //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            //{
            //    // Get the tile clicked
            //    Vector3 position = Input.touches[0].position;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Clicky!");
                Vector3 position = Input.mousePosition;

                PointerEventData eventData = new PointerEventData(EventSystem.current)
                {
                    position = position
                };

                List<RaycastResult> results = new List<RaycastResult>();
                
                EventSystem.current.RaycastAll(eventData, results);

                foreach (RaycastResult r in results)
                {
                    Debug.Log(r.gameObject.name);
                    if(r.gameObject.tag == "BoardTile")
                    {
                        BoardTile tile = r.gameObject.GetComponent<BoardTile>();
                        OnTileSelected(tile.Tile, tile.Pos);
                        return;
                    }
                }
            }
        }
    }
    
    void BeginTargeting()
    {
        Debug.Log("Begin targeting");
        gameState = GameState.CastingSpell;
        // Change buttons, disable card dragging
    }
    
    void EndTargeting()
    {
        gameState = GameState.Playing;
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
