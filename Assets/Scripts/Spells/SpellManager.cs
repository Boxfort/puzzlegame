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

    public static event OnSpellCastCallback OnSpellCast;

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
        // Disable Spellbook
		spellBook.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
       
	public void CastSpell()
    {
		Debug.Log("Casting spell - " + selectedSpell.ToString());
		spellBehaviours[spellsData[selectedSpell].behaviour].Execute();
		OnSpellCast(spellsData[selectedSpell]);
		CloseSpellBook();
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
