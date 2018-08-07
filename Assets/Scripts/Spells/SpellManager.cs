using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

/*
 * Need to manage states : Paused, casting spell, etc
 * when casting spell, cards cannot be dragged.
 *                     spell icon should be changed.
 */
public class SpellManager : MonoBehaviour 
{
	const string SPELLS_PATH = "Data/spells";
    
    public delegate void OnSpellCastCallback(SpellData spell);
    public delegate void OnSpellBeginTargetCallback();
    public delegate void OnSpellEndTargetCallback();

    public static event OnSpellCastCallback OnSpellCast;
    public static event OnSpellBeginTargetCallback OnSpellBeginTarget;
    public static event OnSpellEndTargetCallback OnSpellEndTarget;

	GameObject spellBook;
	GameObject buyButton;
	SpellScroll spellScroller;

	List<SpellData> spellsData;
	Dictionary<string, ISpellBehaviour> spellBehaviours;

	int selectedSpell = 0;

	// Use this for initialization
	void Start () 
	{
		spellsData = new List<SpellData>();
		
		// Get GameObjects
		spellBook = GameObject.Find("SpellBook");
		spellScroller = GameObject.Find("SpellScroller").GetComponent<SpellScroll>();
		buyButton = GameObject.Find("BuySpellButton");
		// Get Spells Behaviours 
		// TODO: maybe put spells into thier own dll files
		spellBehaviours = new Dictionary<string, ISpellBehaviour>
		{
            {"newcard", new NewCardSpell()},
            {"fix", new FixSpell()},
            {"fixall", new FixAllSpell()},
		};

		// Populate Spellbook
		JsonData spellJson = JsonHelper.LoadJsonResource(SPELLS_PATH);
        
		for (int i = 0; i < spellJson.Count; i++)
		{
			spellsData.Add((SpellData)spellJson[i]);
			spellScroller.AddSpell(spellsData[i]);
		}

		UpdateButton();

		// Callbacks
		SpellScroll.OnSpellChanged += SpellChanged;
		GameManager.OnTileSelected += CastTargetedSpell;
        // Disable Spellbook
		spellBook.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
       
	void CastSpell()
    {
        CloseSpellBook();
    
		Debug.Log("Casting spell - " + selectedSpell.ToString());
		ISpellBehaviour behaviour = spellBehaviours[spellsData[selectedSpell].behaviour];

        // If the spell needs a target then get one.
		if (behaviour is ITargetableSpell) 
        {
			// Start selection - howw
			// Get targetable tiletype
			// Highlight all valid tiles
            // who handles the state????
			OnSpellBeginTarget();
        }
		else
        {
            behaviour.Execute();
            OnSpellCast(spellsData[selectedSpell]);
        }
    }
    
    void CastTargetedSpell(TileData tile, Point pos)
    {
        ISpellBehaviour behaviour = spellBehaviours[spellsData[selectedSpell].behaviour];
        
        if(((ITargetableSpell)behaviour).TargetableTiles.Contains(tile.id))
        {
            // FIXME: Target not being set.
			Debug.Log("Fixing tile at " + pos.x + "," + pos.y);
			((ITargetableSpell)behaviour).Target = pos;
			Debug.Log("Fuckin cast that shit at " + ((ITargetableSpell)behaviour).Target.x +","+((ITargetableSpell)behaviour).Target.y);
			behaviour.Execute();
            OnSpellCast(spellsData[selectedSpell]);
			OnSpellEndTarget();
        }
    }
    
	void SpellChanged(int spell)
	{
		Debug.Log("Selecting spell - " + spell.ToString());
		selectedSpell = spell;
		UpdateButton();
	}

    void UpdateButton()
	{
		buyButton.transform.GetChild(0).GetComponent<Text>().text = "Buy Spell - $" + spellsData[selectedSpell].price;
		if (spellsData[selectedSpell].price > GameManager.GetInstance().Money)
        {
			buyButton.GetComponent<Button>().interactable = false;
        }
		else
        {
            buyButton.GetComponent<Button>().interactable = true;
        }
	}

    public void OpenSpellBook()
	{
		spellBook.SetActive(true);
		UpdateButton();
	}

	public void CloseSpellBook()
	{
		spellScroller.Reset();
		spellBook.SetActive(false);
	}
}
