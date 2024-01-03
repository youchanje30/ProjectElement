using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    Transform CanvasPos;
    Transform ParentPos;
    public InventoryUI inventoryUI;
    void Start()
    {
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
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
        //inventoryUI.Info.SetActive(false);
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
        if(ParentPos.GetComponent<RectTransform>().anchoredPosition.x == 399 && ParentPos.GetComponent<RectTransform>().anchoredPosition.y == -260)
        {
            
        }
    }
   
}
