using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;
using DG.Tweening;
using DG.Tweening.Core.Easing;

public class CardController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;


    GameManager gameManager;
    public int Setslot;
    Transform CanvasPos;
    Transform ParentPos;
    public InventoryUI inventoryUI;
    public RectTransform rectTransform;
    
   
    void Start()
    {
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //if (ismove == true && gameManager.inventory.HavingElement[1].WeaponTypes != WeaponTypes.None)
        //  MoveElementsCard();

        //if (Input.GetKeyDown(KeyCode.E) && gameManager.isInven)
        //{
        //    ismove = true;
        //}
    }

    void Awake()
    {
        CanvasPos = canvas.transform;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite != null)
        {
            ParentPos = transform.parent;

            transform.SetParent(CanvasPos);
            transform.SetAsLastSibling();
            canvasGroup.alpha = .7f;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            gameManager.isInven = false;
            inventoryUI.Info.SetActive(false);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == CanvasPos)
        {
            transform.SetParent(ParentPos, true);
            rectTransform.position = ParentPos.GetComponent<RectTransform>().position;
            gameManager.isInven = true;
        }
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite != null)
        {
            if (gameManager.Elements != null)
            {
                //gameManager.Elements[eventData.pointerDrag.GetComponent<CardController>().Setslot].gameObject.SetActive(true);
                //gameManager.Elements[Setslot].gameObject.SetActive(true);
            }

            if (inventoryUI.inventory.HavingElement[Setslot].WeaponTypes != WeaponTypes.None && inventoryUI.inventory.HavingElement[eventData.pointerDrag.GetComponent<CardController>().Setslot].WeaponTypes != WeaponTypes.None)
            {
                inventoryUI.inventory.HavingElement[eventData.pointerDrag.GetComponent<CardController>().Setslot] = gameManager.Elemental.AddElement(int.Parse(transform.GetComponent<Text>().text));
                inventoryUI.inventory.HavingElement[Setslot] = gameManager.Elemental.AddElement(int.Parse(eventData.pointerDrag.GetComponent<Text>().text));

                inventoryUI.SetCard();
            }
        }
    }
    //public void MoveElementsCard()
    //{
        //if (transform.parent == inventoryUI.SpinSlot[0])
        //{

        //    transform.position = Vector3.MoveTowards(transform.position, inventoryUI.SpinSlot[1].position, Time.deltaTime * inventoryUI.speed);
        //    transform.DOScale(inventoryUI.trans[0].localScale, 0.15f);

        //    if (rectTransform.position.x > inventoryUI.SpinSlot[1].transform.position.x - 1 && rectTransform.position.y > inventoryUI.SpinSlot[1].transform.position.y -     1)
        //    {               
        //        transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = inventoryUI.frame[0];
        //        transform.localScale = inventoryUI.trans[0].localScale;            
        //        transform.SetParent(inventoryUI.SpinSlot[1]);                           
        //        rectTransform.anchoredPosition = Vector2.zero;

        //        ismove = false;
        //    }
        //}
        //else if (transform.parent == inventoryUI.SpinSlot[1])
        //{

        //    transform.position = Vector3.MoveTowards(transform.position, inventoryUI.SpinSlot[2].position, Time.deltaTime * inventoryUI.speed);
        //    transform.DOScale(inventoryUI.trans[1].localScale, 0.15f);

        //    if (rectTransform.position.x  > inventoryUI.SpinSlot[2].transform.position.x -1 && rectTransform.position.y < inventoryUI.SpinSlot[2].transform.position.y + 1)
        //    {
        //        transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = inventoryUI.frame[1];
        //        transform.localScale = inventoryUI.trans[1].localScale;
        //        transform.SetParent(inventoryUI.SpinSlot[2]);             
        //        rectTransform.anchoredPosition = Vector2.zero;

        //        ismove = false;
        //    }
        //}
        //else if (transform.parent == inventoryUI.SpinSlot[2])
        //{

        //    transform.position = Vector3.MoveTowards(transform.position, inventoryUI.SpinSlot[0].position, Time.deltaTime * inventoryUI.speed * 2);

        //    if (rectTransform.position.x < inventoryUI.SpinSlot[0].transform.position.x + 2 && rectTransform.position.y < inventoryUI.SpinSlot[2].transform.position.y +2)
        //    {
        //        transform.SetParent(inventoryUI.SpinSlot[0]);
        //        rectTransform.anchoredPosition = Vector2.zero;

        //        ismove = false;           
        //    }
        //}

    //}
}

