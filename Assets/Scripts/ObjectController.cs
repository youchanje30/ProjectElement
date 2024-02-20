using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class ObjectController : MonoBehaviour
{
    
    public enum InteractObjects
    {
        NPC,
        Portal,
        Shop,
        Weapon,
        SpiritAwake // 정령 각성 오브젝트
    }

    public WeaponTypes WeaponType;
    [SerializeField] private InteractObjects ObjType;
    [SerializeField] private GameManager manager;
    public int objectID;
    public string objectTag;
    public GameObject interactView;

    [SerializeField] private GameObject ShopItem;
    [SerializeField] private GameObject HpHealItem;
    [SerializeField] private GameObject SpiritSoulItem;
    [SerializeField] private ScrollRect buyScrollRect;
    [SerializeField] private ScrollRect sellScrollRect;
    [SerializeField] private float Space;
    public List<RectTransform> buyShopObjects = new List<RectTransform>();
    public List<RectTransform> sellShopObjects = new List<RectTransform>();

    private void Update()
    {
        ActiveSpirit();
    }

    void Awake()
    {
        if(!manager)
            manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        

        objectTag = gameObject.tag;


    }

    void Start()
    {
       switch(ObjType)
        {
            case InteractObjects.NPC:
                // GameManager.instance.Action(gameObject);
                break;

            case InteractObjects.Portal:
                Snap();
                // Portal(objectID);
                break;

            case InteractObjects.Shop:
                if(!buyScrollRect)
                    buyScrollRect = UIController.instance.buyScrollRect;
                if(!sellScrollRect)
                    sellScrollRect = UIController.instance.sellScrollRect;
                
                for (int i = 0; i < 2; i++)
                {
                    SpawnConsumableItem(HpHealItem);
                }
                for (int i = 0; i < 5; i++)
                {
                    SpawnShopItem();
                }
                Invoke("SpawnSellItem", 0.1f);
                break;
            
            case InteractObjects.SpiritAwake:

                break;
        }
    }


    public void SpawnShopItem()
    {
        GameObject NewShopItem = Instantiate(ShopItem, buyScrollRect.content);
        NewShopItem.GetComponent<ShopItem>().objectController = this ;
        NewShopItem.GetComponent<ShopItem>().buyBool = true ;
        NewShopItem.GetComponent<ShopItem>().Setting(ItemManager.instance.GetShopItem());
        var newUi = NewShopItem.GetComponent<RectTransform>();
        buyShopObjects.Add(newUi);
        SetPosShop();
    }


    public void SpawnConsumableItem(GameObject obj)
    {
        GameObject NewShopItem = Instantiate(obj, buyScrollRect.content);
        NewShopItem.GetComponent<ShopItem>().objectController = this;
        NewShopItem.GetComponent<ShopItem>().buyBool = true;
        var newUi = NewShopItem.GetComponent<RectTransform>();
        buyShopObjects.Add(newUi);
        SetPosShop();
    }

    public void SetPosShop()
    {
        // float y = 0f;

        
        // float y = 0f;
        float x = 0f;
        for (int i = 0; i < buyShopObjects.Count; i++)
        {
            buyShopObjects[i].anchoredPosition = new Vector2(x, buyShopObjects[i].anchoredPosition.y);
            x += buyShopObjects[i].sizeDelta.x + Space;
        }

        // buyScrollRect.content.sizeDelta = new Vector2(x, buyScrollRect.content.sizeDelta.y);

        // x = 0f;
        // for (int i = 0; i < buyShopObjects.Count; i++)
        // {
        //     buyShopObjects[i].anchoredPosition = new Vector2(x, buyShopObjects[i].anchoredPosition.y);
        //     x += buyShopObjects[i].sizeDelta.x + Space;
        // }

        buyScrollRect.content.sizeDelta = new Vector2(x, buyScrollRect.content.sizeDelta.y);

        // shopObjects.Remove()

        x = 0f;
        for (int i = 0; i < sellShopObjects.Count; i++)
        {
            sellShopObjects[i].anchoredPosition = new Vector2(x, sellShopObjects[i].anchoredPosition.y);
            x += sellShopObjects[i].sizeDelta.x + Space;
        }

        sellScrollRect.content.sizeDelta = new Vector2(x, buyScrollRect.content.sizeDelta.y);
    }

    public void SpawnSellItem()
    {
        Inventory inven = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        for (int i = 0; i < 6; i++)
        {
            if(inven.HavingItem[i] == null) continue;

            GameObject NewShopItem = Instantiate(ShopItem, sellScrollRect.content);
            NewShopItem.GetComponent<ShopItem>().objectController = this;
            NewShopItem.GetComponent<ShopItem>().buyBool = false;
            NewShopItem.GetComponent<ShopItem>().Setting(inven.HavingItem[i]);

            var newUi = NewShopItem.GetComponent<RectTransform>();
            sellShopObjects.Add(newUi);
            SetPosShop();

            Debug.Log(NewShopItem);
        }
    }

    public void AddShop(int itemID)
    {
        Inventory inven = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        GameObject NewShopItem = Instantiate(ShopItem, sellScrollRect.content);
        NewShopItem.GetComponent<ShopItem>().objectController = this;
        NewShopItem.GetComponent<ShopItem>().buyBool = false;
        NewShopItem.GetComponent<ShopItem>().Setting(ItemManager.instance.AddItem(itemID));

        var newUi = NewShopItem.GetComponent<RectTransform>();
        sellShopObjects.Add(newUi);
        SetPosShop();

        Debug.Log(NewShopItem);
    }

    public void Interaction()
    {
        /* switch(objectTag)
        {
            case "NPC":
                GameManager.instance.Action(gameObject);
                break;

            case "Portal":
                Portal(objectID);
                break;

            case "Shop":
                
                break;
        } */
 
        switch(ObjType)
        {
            case InteractObjects.NPC:
                StartCoroutine(GameManager.instance.Action(gameObject));
                break;

            case InteractObjects.Portal:
                Portal(objectID);
                break;

            case InteractObjects.Shop:
                OpenShop();
                break;
                
            case InteractObjects.SpiritAwake:
                OpenSpiritAwake();
                break;

            case InteractObjects.Weapon:
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                player.PlayerWeaponType = WeaponType;
                player.SetEquipment();
                SaveManager.instance.Save();
                // player.ChangeAnim();
                // player.SetEquipment();
                break;
        }
    
    }

    public void Snap()
    {    

        Vector3 pos = transform.position;
        RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.down, 10f, LayerMask.GetMask("Platform"));
        if(!ray) return;
        
        pos.y -= ray.distance;
        transform.position = pos;
    }

    public void OpenSpiritAwake()
    {
        GameManager.instance.SpiritAwakeUI.SetActive(true);
        GameManager.instance.isSpiritAwake = true;
    }

    
    public void OpenShop()
    {
        GameManager.instance.ShopUI.SetActive(true);
        GameManager.instance.isShop = true;

        GameManager.instance.buyPanel.SetActive(GameManager.instance.viewBuy);
        GameManager.instance.sellPanel.SetActive(!GameManager.instance.viewBuy);
        Debug.Log("OpenShop");
    }

    public void Portal(int ID)
    {
        //Chapter Stage 이동은 ID 10
        //
        switch (ID)
        {
            case 10:
                GameManager.instance.RandomStageRoad();
                break;

            case 20:
                GameManager.instance.MainstageRoad();
                break;

        }
    }

    public void InteractView(bool isOn)
    {
        interactView.SetActive(isOn);   
    }
    
    public void ActiveSpirit()
    {
        for(int i = 0; i< manager.inventory.HavingElement.Length; i++)
        {
            if (manager.inventory.HavingElement[i].ElementalID == this.objectID && manager.inventory.HavingElement[i].WeaponTypes == this.WeaponType)
            {
                manager.Elements[i] = this.gameObject;
                manager.Elements[i].SetActive(false);
            }
        }      
    }

}