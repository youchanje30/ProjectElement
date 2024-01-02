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
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        objectTag = gameObject.tag;

        switch(ObjType)
        {
            case InteractObjects.NPC:
                // GameManager.instance.Action(gameObject);
                break;

            case InteractObjects.Portal:
                // Portal(objectID);
                break;

            case InteractObjects.Shop:
                // scrollRect = GameObject.FindGameObjectWithTag("ShopScroll").GetComponent<ScrollRect>();
                for (int i = 0; i < 2; i++)
                {
                    // Invoke("SpawnConsumableItem", 0.1f);
                    SpawnConsumableItem(HpHealItem);
                    SpawnConsumableItem(SpiritSoulItem);
                }
                for (int i = 0; i < 5; i++)
                {
                    Invoke("SpawnShopItem", 0.1f);
                }
                break;
            
            case InteractObjects.SpiritAwake:

                break;
        }


    }


    public void SpawnShopItem()
    {
        GameObject NewShopItem = Instantiate(ShopItem, buyScrollRect.content);
        NewShopItem.GetComponent<ShopItem>().Setting(ItemManager.instance.GetShopItem());
        NewShopItem.GetComponent<ShopItem>().objectController = this ;
        NewShopItem.GetComponent<ShopItem>().buyBool = true ;
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
        float y = 0f;
        for (int i = 0; i < buyShopObjects.Count; i++)
        {
             buyShopObjects[i].anchoredPosition = new Vector2(0f, -y);
            y += buyShopObjects[i].sizeDelta.y + Space;
        }

        buyScrollRect.content.sizeDelta = new Vector2(buyScrollRect.content.sizeDelta.x, y);

        // shopObjects.Remove()

        y = 0f;
        for (int i = 0; i < sellShopObjects.Count; i++)
        {
            sellShopObjects[i].anchoredPosition = new Vector2(0f, -y);
            y += sellShopObjects[i].sizeDelta.y + Space;
        }

        sellScrollRect.content.sizeDelta = new Vector2(sellScrollRect.content.sizeDelta.x, y);
    }

    public void SpawnSellItem()
    {
        Inventory inven = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        for (int i = 0; i < 6; i++)
        {
            if(inven.HavingItem[i] == null) continue;

            GameObject NewShopItem = Instantiate(ShopItem, sellScrollRect.content);
            NewShopItem.GetComponent<ShopItem>().Setting(inven.HavingItem[i]);
            NewShopItem.GetComponent<ShopItem>().objectController = this;
            NewShopItem.GetComponent<ShopItem>().buyBool = false;

            var newUi = NewShopItem.GetComponent<RectTransform>();
            sellShopObjects.Add(newUi);
            SetPosShop();

            Debug.Log(NewShopItem);
        }
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
                GameManager.instance.Action(gameObject);
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