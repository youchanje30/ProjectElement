using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public GameManager gameManager;
    public  Inventory inventory;

    private Transform Canvas;

    [Header("Inventory Setting")]

    public GameObject ItemInfo;
    public RectTransform Infopos;
    public GameObject Info;
    public RectTransform[] Card;
    public Image[] EleCards;
    public Image[] InvenItem;
    public GameObject InvenUI;

    Canvas canvas;

    public RectTransform targetRectTr;
    public Camera uiCamera;
    public Vector2 screenPoint;
    public float Speed;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        rectTransform = GetComponent<RectTransform>();
        Canvas = FindObjectOfType<Canvas>().transform;
    
    }
    void Start()
    {
        ItemInfo.SetActive(false); 
        Info.SetActive(false);
        targetRectTr = Canvas.GetComponent<RectTransform>();
        uiCamera = Camera.main;
    }
    void Update()
    { 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
        Infopos.SetAsLastSibling();
        Infopos.anchoredPosition = new Vector3(screenPoint.x - 136, screenPoint.y -196);
    }
    public void SetCard()
    {
        for (int i = 0; i < EleCards.Length; i++)
        {
            EleCards[i].sprite = inventory.HavingElement[i].elementalImg;
        }
    }
    public void SetItem()
    {

    }
   
    public void OpenInventory()
    {
        SetCard();
        gameManager.isInven = !gameManager.isInven;
        InvenUI.SetActive(gameManager.isInven);
    }
   
}