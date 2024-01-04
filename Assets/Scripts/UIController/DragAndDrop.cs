using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IDropHandler, IPointerDownHandler
{
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;

    
    GameManager gameManager;
 
    Transform CanvasPos;
    Transform ParentPos;
    public InventoryUI inventoryUI;

    void Start()
    {
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public RectTransform rectTransform;

    void Update()
    {
        
    }
    void Awake()
    {
        CanvasPos = canvas.transform;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryUI.screenPoint.x < 283)
        {
            inventoryUI.SetSlot = 0;
        }
        else if (inventoryUI.screenPoint.x >= 543.5)
        {
            inventoryUI.SetSlot = 2;
        }
        else { inventoryUI.SetSlot = 1; }

        inventoryUI.SetEleImage = transform.GetChild(0).GetComponent<Image>();
        inventoryUI.SetEleID = transform.GetComponent<Text>().text;
        inventoryUI.SetElement = gameManager.Elements[inventoryUI.SetSlot];
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentPos = transform.parent;

        transform.SetParent(CanvasPos);
        transform.SetAsLastSibling();
        canvasGroup.alpha = .7f;
        canvasGroup.blocksRaycasts = false;      
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
        inventoryUI.Info.SetActive(false);
     
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == CanvasPos)
        {
            transform.SetParent(ParentPos);
            rectTransform.position = ParentPos.GetComponent<RectTransform>().position;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {       
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //eventData.pointerDrag.transform.SetParent(transform.parent);
            //eventData.pointerDrag.GetComponent<RectTransform>().position = rectTransform.position;
            ////eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            //this.transform.SetParent(rect);
            rectTransform.anchoredPosition = new Vector2(0,0);
            if (inventoryUI.screenPoint.x < 283)
            {
                gameManager.swapUI.slot = 0;
            }
            else if (inventoryUI.screenPoint.x >= 543.5)
            {
                gameManager.swapUI.slot = 2;
            }
            else { gameManager.swapUI.slot = 1; }

            gameManager.Elements[gameManager.swapUI.slot].gameObject.SetActive(true);

            inventoryUI.inventory.HavingElement[inventoryUI.SetSlot] = inventoryUI.gameManager.Elemental.AddElement(int.Parse(transform.GetComponent<Text>().text));
            inventoryUI.inventory.HavingElement[gameManager.swapUI.slot] = inventoryUI.gameManager.Elemental.AddElement(int.Parse(eventData.pointerDrag.GetComponent<Text>().text));         
                                 
            transform.GetChild(0).GetComponent<Image>().sprite = inventoryUI.SetEleImage.sprite;
            inventoryUI.EleCards[inventoryUI.SetSlot].GetComponent<Image>().sprite = inventoryUI.SetEleImage.sprite;
            transform.GetComponent<Text>().text = inventoryUI.SetEleID;

            inventoryUI.SetCard();
            gameManager.Elements[gameManager.swapUI.slot] = inventoryUI.SetElement;
        }
    }
}
