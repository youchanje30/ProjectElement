using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public RectTransform rectTransform;
    private GameManager gameManager;
    private Transform Canvas;
    private Transform previousParent;
    private CanvasGroup canvasGroup;

    [Header("Inventory Setting")]
    
    public GameObject[] ItemInfo;
    public RectTransform Infopos;
    public GameObject Info;
    public RectTransform Card;
    Transform _startParent;

    Canvas canvas;

    public RectTransform targetRectTr;
    public Camera uiCamera;
    public Vector2 screenPoint;
    public float Speed;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rectTransform = GetComponent<RectTransform>();
        Canvas = FindObjectOfType<Canvas>().transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        Info.SetActive(false); 
        for (int i = 0; i < ItemInfo.Length; i++) { ItemInfo[i].SetActive(false); }
        targetRectTr = GameObject.FindGameObjectWithTag("UI").GetComponent<RectTransform>();
        uiCamera = Camera.main;           
    }

    // Update is called once per frame
    void Update()
    {
        MoveCard();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
        Infopos.SetAsLastSibling();
        Infopos.anchoredPosition = new Vector3(screenPoint.x - 136, screenPoint.y + 196);
    }  
    public void SetItem()
    {
        for(int i = 0; i<gameManager.inventory.HavingItem.Length; i++)
        {
            for (int j = 0; j < gameManager.Item.itemDatas.Count; j++)
            {
                if (gameManager.inventory.HavingItem[i] == gameManager.Item.itemDatas[j]  )
                {
                    gameManager.InvenItem[i] = gameManager.inventory.HavingItem[i].itemImg;
                }
                else
                {
                    gameManager.InvenItem[i] = null; 
                }
            }
        }
        
    }
    public void OpenInformation()
    {
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            //    Debug.Log("On Mouse");         
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {      
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {    
        Infopos.SetAsLastSibling();
        Card.anchoredPosition = screenPoint;
        rectTransform.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rectTransform.position = previousParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {   
        Info.SetActive(true);
        

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Info.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Info.SetActive(false);
        previousParent = transform.parent;

        transform.SetParent(Canvas);
        transform.SetAsLastSibling();

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }
    public void MoveInformation()
    {
        
        
    }
    public void MoveCard()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Card.anchoredPosition.x == 673 && Card.anchoredPosition.y == 278)
            {
                Card.anchoredPosition = new Vector3(286, 278, 0);
            }
            else if (Card.anchoredPosition.x == 286 && Card.anchoredPosition.y == 278)
            {
                Card.anchoredPosition = new Vector3(479, 234, 0);
                Card.SetAsLastSibling();
            }
            else if (Card.anchoredPosition.x == 479 && Card.anchoredPosition.y == 234)
            {
                Card.anchoredPosition = new Vector3(673, 278, 0);
            }
        }
    }
}
