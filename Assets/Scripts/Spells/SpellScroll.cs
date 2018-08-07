using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class SpellScroll : MonoBehaviour
{
    public delegate void OnSpellChangedCallback(int spell);

    public static event OnSpellChangedCallback OnSpellChanged;

    List<Transform> menuItems;
    List<float> itemTarget;
    List<SpellData> spellsData;

    GameObject spellIconPrefab;
    float dragMagnitude = 700.0f;
    float itemSpacing = 500f;
    float minSize = 0.6f;
    float maxSize = 1.0f;
    float lastMousePos;
    int selectedItem;
    bool dragging;
    bool scrolling;
    bool clicking;
  
    // Use this for initialization
    void Start ()
    {
        menuItems = new List<Transform>();
        itemTarget = new List<float>();

        spellIconPrefab = Resources.Load<GameObject>("Prefabs/SpellIcon");
    }
    
    public void AddSpell(SpellData spell)
    {
        // Create GameObject
        GameObject instance = Instantiate(spellIconPrefab, transform);
        // Set GameObjects appearance.
        instance.transform.GetChild(0).GetComponent<Image>().sprite = spell.sprite;
        instance.transform.GetChild(1).GetComponent<Text>().text = spell.name;
        instance.transform.GetChild(2).GetComponent<Text>().text = spell.description;
        // Add to lists
        menuItems.Add(instance.transform);
        itemTarget.Add(0);
        // Update 
        UpdateItemPositions();
        ScaleItems();
    }

    void UpdateItemPositions()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            itemTarget[i] = (i * itemSpacing);  
            menuItems[i].localPosition = new Vector3(itemTarget[i], menuItems[i].localPosition.y, menuItems[i].localPosition.z);
        }
    }



    public void Reset()
    {
        dragging = false;
        for (int i = 0; i < menuItems.Count; i++)
        {
            menuItems[i].localPosition = new Vector3(itemTarget[i], menuItems[i].localPosition.y, menuItems[i].localPosition.z);
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        clicking = Input.GetMouseButton(0);

        if(Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(ClickTime());
            StartDrag();
        }
        if(Input.GetMouseButtonUp(0) && dragging)
        {
            EndDrag();
        }

        if(dragging)
        {
            float mousePos = (Input.mousePosition.x / Screen.width) - 0.5f;
            float deltaX = (mousePos - lastMousePos);
            lastMousePos = mousePos;

            foreach (Transform t in menuItems)
            {
                t.position = t.position + new Vector3(deltaX, 0.0f, 0.0f) * dragMagnitude;
            }

            ScaleItems();
        }
        else if (!scrolling)
        {
            for (int i = 0; i < itemTarget.Count; i++)
            {
                //float xTarget = Mathf.Clamp(Mathf.Round(t.position.x / _itemSpacing) * _itemSpacing, (_items.Count - _items.IndexOf(t) - 1) * -_itemSpacing, _items.IndexOf(t) * _itemSpacing);
                menuItems[i].localPosition = new Vector3(Mathf.Lerp(menuItems[i].localPosition.x, itemTarget[i], 5.0f * Time.deltaTime), 
                                                      menuItems[i].localPosition.y, 
                                                      menuItems[i].localPosition.z);
            }

            ScaleItems();
        }
    }

    /*
    IEnumerator ClickTime()
    {
        float elapsedTime = 0.0f;
        float waitTime = 1.0f;

        while(elapsedTime < waitTime)
        {
            Debug.Log(_clicking);

            if (!_clicking)
            {
                Debug.Log("SHORT CLICK SON");
                Scroll();
                yield break;
            }

            Debug.Log(elapsedTime);
            elapsedTime += Time.deltaTime;
        }

        Debug.Log("BIG OL DRAG");
        StartDrag();
        yield return true;
    }

    void Scroll()
    {
        scrolling = true;

        scrolling = false;
    }

    */

    void StartDrag()
    {
        dragging = true;
        lastMousePos = (Input.mousePosition.x / Screen.width) - 0.5f;
    }

    void EndDrag()
    {
        dragging = false;

        //Find position of item 0
        float firstPos = Mathf.Clamp( Mathf.Round(menuItems[0].localPosition.x / itemSpacing) * itemSpacing,  // Value
                                      (menuItems.Count - menuItems.IndexOf(menuItems[0]) - 1) * -itemSpacing, // min
                                      menuItems.IndexOf(menuItems[0]) * itemSpacing);                         // max

        // HACK: SUBTRACT FROM STARTING POSITION (this is hacky as fuck gross man)
        int prev = selectedItem;
        selectedItem = Mathf.Abs((int)(firstPos / itemSpacing));

        if (selectedItem != prev)
        {
            OnSpellChanged((selectedItem));
        }
      
        for (int i = 0; i < itemTarget.Count; i++)
        {
            //Mathf.Round(_items[i].position.x / _itemSpacing) * _itemSpacing
            //_itemTarget[i] = Mathf.Clamp(, (_items.Count - _items.IndexOf(_items[i]) - 1) * -_itemSpacing, _items.IndexOf(_items[i]) * _itemSpacing);
            itemTarget[i] = firstPos + (itemSpacing * i);
        }
    }
       
    void ScaleItems()
    {
        foreach(Transform t in menuItems)
        {
            float scale = maxSize;
                     
            scale = Mathf.Clamp((itemSpacing  - Mathf.Abs(t.localPosition.x)) / itemSpacing, minSize, maxSize);
            
            t.localScale = new Vector3(scale, scale, scale);
        }
    }
}
