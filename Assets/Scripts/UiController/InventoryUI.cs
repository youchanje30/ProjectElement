using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour//, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public RectTransform rectTransform;
    private GameManager gameManager;
    private Inventory inventory;

    private Transform Canvas;
    private Transform previousParent;
    private CanvasGroup canvasGroup;


    [Header("Inventory Setting")]
    
    public GameObject[] ItemInfo;
    public RectTransform[] Infopos;
    public GameObject[] Info;
    public RectTransform[] Card;
    Transform _startParent;
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
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {

        for (int i = 0; i < ItemInfo.Length; i++) { ItemInfo[i].SetActive(false); }
        for (int i = 0; i < ItemInfo.Length; i++) { Info[i].SetActive(false); }
        targetRectTr = Canvas.GetComponent<RectTransform>();
        uiCamera = Camera.main;           
    }

    // Update is called once per frame
    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
        //Infopos.SetAsLastSibling();
        //Infopos.anchoredPosition = new Vector3(screenPoint.x - 136, screenPoint.y + 196);
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
        for(int i = 0; i<inventory.HavingItem.Length; i++)
        {
            for (int j = 0; j < gameManager.Item.itemDatas.Count; j++)
            {
                if (inventory.HavingItem[i] == gameManager.Item.itemDatas[j]  )
                {
                    InvenItem[i] = inventory.HavingItem[i].itemImg;
                }
                else
                {
                    InvenItem[i] = null; 
                }
            }
        }

    }

    //public void OpenInformation()
    //{
    //    if (EventSystem.current.IsPointerOverGameObject() == true)
    //    {
    //        //    Debug.Log("On Mouse");         
    //    }
    //}
    //public void OnPointerDown(PointerEventData eventData)
    //{      
    //    OnDrag(eventData);
    //}
    //public void OnDrag(PointerEventData eventData)
    //{    
    //    Infopos.SetAsLastSibling();
    //    Card.anchoredPosition = screenPoint;
    //    rectTransform.position = eventData.position;
    //}
    ////public void OnEndDrag(PointerEventData eventData)
    ////{
    ////    //if(transform.parent == canvas)
    ////    //{
    ////    //    transform.SetParent(previousParent);
    ////    //    rectTransform.position = previousParent.GetComponent<RectTransform>().position;
    ////    //}

    ////    //canvasGroup.alpha = 1.0f;
    ////    //canvasGroup.blocksRaycasts = true;

    ////}
    ////public void OnPointerUp(PointerEventData eventData)
    ////{

    ////}
    ////public void OnPointerEnter(PointerEventData eventData)
    ////{   
    ////    Info.SetActive(true);
        

    ////}
    ////public void OnPointerExit(PointerEventData eventData)
    ////{
    ////    Info.SetActive(false);
    ////}
    ////public void OnBeginDrag(PointerEventData eventData)
    ////{
    ////    Info.SetActive(false);
    ////    previousParent = transform.parent;

    ////    //transform.SetParent(Canvas);
    ////    //transform.SetAsLastSibling();

    ////    //canvasGroup.alpha = 0.6f;
    ////    //canvasGroup.blocksRaycasts = false;
    ////}
    ////public void MoveInformation()
    ////{
        
        
    ////}
    ////public void MoveCard()
    ////{
    ////    //if (Input.GetKeyDown(KeyCode.E))
    ////    //{
    ////    //    for (int i = 0; i<Card.Length; i++)
    ////    //    {
    ////    //        if (Card[i].anchoredPosition.x == 673 && Card[i].anchoredPosition.y == 278)
    ////    //        {
    ////    //            Card[i].anchoredPosition = new Vector3(286, 278, 0);
    ////    //        }
    ////    //        else if (Card[i].anchoredPosition.x == 286 && Card[i].anchoredPosition.y == 278)
    ////    //        {
    ////    //            Card[i].anchoredPosition = new Vector3(479, 234, 0);
    ////    //            Card[i].SetAsLastSibling();
    ////    //        }
    ////    //        else if (Card[i].anchoredPosition.x == 479 && Card[i].anchoredPosition.y == 234)
    ////    //        {
    ////    //            Card[i].anchoredPosition = new Vector3(673, 278, 0);
    ////    //        }
    ////    //    }
           
    ////    //}
    ////}
    
    public void OpenInventory()
    {
        SetCard();
        gameManager.isInven = !gameManager.isInven;
        InvenUI.SetActive(gameManager.isInven);
    }

}
