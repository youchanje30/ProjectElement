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
    public RectTransform[] Itempos;
    public Image[] InvenItem;
    public GameObject InvenUI;
    public TextMeshProUGUI Stat;
    public Animator animator;
    public GameObject[] Slot;

    [Header("Inventory Swap")]
    public Transform[] SpinSlot;
    public float speed = 6;
    public Sprite[] frame;
    public Transform[] trans;

    [Space(20f)]

    public RectTransform targetRectTr;
    public Camera uiCamera;
    public Vector2 screenPoint;
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
        uiCamera = Camera.main;
    }
    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
        Info.GetComponent<RectTransform>().SetAsLastSibling();
        Info.GetComponent<RectTransform>().anchoredPosition = new Vector3(screenPoint.x - 130, screenPoint.y - 196);
        ItemInfo.GetComponent<RectTransform>().SetAsLastSibling();
        ItemInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(Input.mousePosition.x - 230, Input.mousePosition.y + 150) / Canvas.GetComponent<Canvas>().scaleFactor;
    
    }
    public void SetCard()
    {
        for (int i = 0; i < EleCards.Length; i++)
        {
            EleCards[i].GetComponent<Image>().sprite = inventory.HavingElement[i].elementalImg;
            Slot[i].GetComponent<Text>().text = inventory.HavingElement[i].ElementalID.ToString();
            Slot[i].GetComponent<CardController>().Setslot = i;
}
        for (int i = 0; i < InvenItem.Length; i++)
        {           
            //InvenItem[i].sprite = inventory.HavingItem[i].itemImg;
        }
    }
    public void SetItem()
    {

    }
    public void OpenInventory()
    {
        animator.SetTrigger("OpenInven");
        SetCard();
        SetItem();
        gameManager.isInven = !gameManager.isInven;
        InvenUI.SetActive(gameManager.isInven);
    }



   
}
