using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameManager gameManager;
    public Inventory inventory;
    private Transform Canvas;

    [Header("Inventory Setting")]
    public GameObject ItemInfo;
    public GameObject Info;
    public RectTransform[] Card;
    public GameObject[] EleCards;
    public GameObject[] CardImg;
    public GameObject[] InvenItem;
    public GameObject InvenUI;
    public TextMeshProUGUI Stat;
    public Animator animator;
    public GameObject inven;
    public GameObject[] Slot;

    [Header("Inventory Swap")]
    public Transform[] SpinSlot;
    public float speed = 6;
    public Sprite[] frame;
    public Transform[] trans;
    public ElementalData[] WeaponTypes;

    [Space(20f)]
    public float Speed;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        //elementalData = gameManager.GetComponent<ElementalData>();
        rectTransform = GetComponent<RectTransform>();
        Canvas = GameObject.FindGameObjectWithTag("UI").GetComponent<Transform>();
    }
    void Start()
    {
        ItemInfo.SetActive(false);
        Info.SetActive(false);
    }
    void Update()
    {
        if (Info.activeSelf == true || ItemInfo.activeSelf == true)
        {
            Info.GetComponent<RectTransform>().SetAsLastSibling();
            Info.transform.position = new Vector3(Input.mousePosition.x - 140, Input.mousePosition.y - 160);
            ItemInfo.GetComponent<RectTransform>().SetAsLastSibling();
            ItemInfo.transform.position = new Vector3(Input.mousePosition.x - 190, Input.mousePosition.y + 140);
        }
        if (Input.GetKeyDown(KeyCode.E) && gameManager.isInven)
        {
            //for (int i = 0; i < WeaponTypes.Length; i++)
            //    Slot[i].GetComponent<CardController>().MoveElementsCard();
            MoveElementsCard();
        }


    }
    public void SetCard()
    {

        for (int i = 0; i < EleCards.Length; i++)
        {
            EleCards[i].GetComponent<Image>().sprite = inventory.HavingElement[i].elementalImg;
            CardImg[i].GetComponent<Image>().sprite = inventory.HavingElement[i].elementalCard;
            Slot[i].GetComponent<Text>().text = inventory.HavingElement[i].ElementalID.ToString();
            Slot[i].GetComponent<CardController>().Setslot = i;
            if (EleCards[i].GetComponent<Image>().sprite != null)
            {
                EleCards[i].SetActive(true);
                CardImg[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                EleCards[i].SetActive(false);
                CardImg[i].GetComponent<Image>().color = new Color(0.9921569f, 0.9529412f, 0.9176471f);
            }
        }
        for (int i = 0; i < InvenItem.Length; i++)
        {

            if (inventory.HavingItem[i] != null)
            {
                InvenItem[i].SetActive(true);
                InvenItem[i].GetComponent<Image>().sprite = inventory.HavingItem[i].itemImg;
            }
            else
            {
                InvenItem[i].SetActive(false);
            }
        }
        Info.SetActive(false);
        ItemInfo.SetActive(false);
    }
    public void InventoryAnim()
    {
        //ItemInfo.SetActive(false);
        //Info.SetActive(false);
        gameManager.isInven = !gameManager.isInven;
        InvenUI.SetActive(gameManager.isInven);
        animator.SetTrigger("OpenInven");
        Invoke("InventorySetting", 0.3f);
       
    }
    public void InventorySetting()
    {
        inven.SetActive(gameManager.isInven);
        SetCard();
        Time.timeScale = 0f;
    }
    public void MoveElementsCard()
    {
        for (int i = 0; i < 3; i++)
        {
            WeaponTypes[i] = inventory.HavingElement[i];
        }
        if (inventory.HavingElement[2].WeaponTypes != global::WeaponTypes.None)
        {
            //ElementalData elementalData = inventoryUI.WeaponTypes[2];
            inventory.HavingElement[0] = WeaponTypes[2];
            inventory.HavingElement[1] = WeaponTypes[0];
            inventory.HavingElement[2] = WeaponTypes[1];
            Debug.Log(1);
        }
        else if (inventory.HavingElement[1].WeaponTypes != global::WeaponTypes.None)
        {
            inventory.HavingElement[0] = WeaponTypes[1];
            inventory.HavingElement[1] = WeaponTypes[0];

        }
        SetCard();
    }
}
