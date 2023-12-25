using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private WeaponTypes WeaponType;
    [SerializeField] private InteractObjects ObjType;
    public int objectID;
    public string objectTag;
    public GameObject interactView;

    [SerializeField] private GameObject ShopItem;
    [SerializeField] private GameObject HpHealItem;
    [SerializeField] private GameObject SpiritSoulItem;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float Space;
    public List<RectTransform> shopObjects = new List<RectTransform>();

    

    void Awake()
    {
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
        GameObject NewShopItem = Instantiate(ShopItem, scrollRect.content);
        NewShopItem.GetComponent<ShopItem>().Setting(ItemManager.instance.GetShopItem());
        NewShopItem.GetComponent<ShopItem>().objectController = this ;
        var newUi = NewShopItem.GetComponent<RectTransform>();
        shopObjects.Add(newUi);
        SetPosShop();
    }


    public void SpawnConsumableItem(GameObject obj)
    {
        GameObject NewShopItem = Instantiate(obj, scrollRect.content);
        NewShopItem.GetComponent<ShopItem>().objectController = this ;
        var newUi = NewShopItem.GetComponent<RectTransform>();
        shopObjects.Add(newUi);
        SetPosShop();
    }

    public void SetPosShop()
    {
        float y = 0f;
        for (int i = 0; i < shopObjects.Count; i++)
        {
            shopObjects[i].anchoredPosition = new Vector2(0f, -y);
            y += shopObjects[i].sizeDelta.y + Space;
        }

        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);

        // shopObjects.Remove()
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


}