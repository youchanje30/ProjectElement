using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IDropHandler, IPointerDownHandler
{
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;


    GameManager gameManager;
    public int Setslot;
    Transform CanvasPos;
    Transform ParentPos;
    public InventoryUI inventoryUI;
    public RectTransform rectTransform;
    public bool ismove  = false;
   
    void Start()
    {
        inventoryUI = GameObject.Find("Canvas").GetComponent<InventoryUI>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (ismove == true) 
         MoveElementsCard();

        if (Input.GetKeyDown(KeyCode.E))
        {
            ismove = true;
        }
    }

    void Awake()
    {
        CanvasPos = canvas.transform;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {


        //inventoryUI.SetEleImage = transform.GetChild(0).GetComponent<Image>();
        //inventoryUI.SetEleID = transform.GetComponent<Text>().text;
        //inventoryUI.SetWeaponType = weaponType;

        //for (int i = 0; i < gameManager.Elements.Length; i++)
        //{
        //    if (inventoryUI.inventory.HavingElement[i].ElementalID == int.Parse(transform.GetComponent<Text>().text))
        //    {
        //        inventoryUI.SetElement = gameManager.Elements[i];
        //        inventoryUI.SetEleID = inventoryUI.inventory.HavingElement[i].ElementalID.ToString();
        //        inventoryUI.SetWeaponType = inventoryUI.inventory.HavingElement[i].WeaponTypes;
        //        break;
        //    }
        //}
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite != null)
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
        if (transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            inventoryUI.Info.SetActive(false);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == CanvasPos)
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
        if (eventData.pointerDrag != null && transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite != null)
        {
            gameManager.Elements[eventData.pointerDrag.GetComponent<DragAndDrop>().Setslot].gameObject.SetActive(true);
            gameManager.Elements[Setslot].gameObject.SetActive(true);
            inventoryUI.inventory.HavingElement[eventData.pointerDrag.GetComponent<DragAndDrop>().Setslot] = inventoryUI.gameManager.Elemental.AddElement(int.Parse(transform.GetComponent<Text>().text));
            inventoryUI.inventory.HavingElement[Setslot] = inventoryUI.gameManager.Elemental.AddElement(int.Parse(eventData.pointerDrag.GetComponent<Text>().text));

            inventoryUI.SetCard();

        }
    }
    public void MoveElementsCard()
    {

        if (transform.parent == inventoryUI.SpinSlot[0])
        {
            transform.position = Vector3.MoveTowards(transform.position, inventoryUI.SpinSlot[1].position, Time.deltaTime * inventoryUI.speed);

            if (transform.position == inventoryUI.SpinSlot[1].position)
            {
                transform.localScale = inventoryUI.SpinSlot[1].GetChild(0).localScale;
                transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = inventoryUI.SpinSlot[1].GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite;
                transform.SetParent(inventoryUI.SpinSlot[1]);
                rectTransform.anchoredPosition = Vector2.zero;
                ismove = false;
            }
        }
        else if (transform.parent == inventoryUI.SpinSlot[1])
        {
            transform.position = Vector3.MoveTowards(transform.position, inventoryUI.SpinSlot[2].position, Time.deltaTime * inventoryUI.speed);

            if (transform.position == inventoryUI.SpinSlot[2].position)
            {
                transform.localScale = inventoryUI.SpinSlot[0].GetChild(0).localScale;
                transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = inventoryUI.SpinSlot[0].GetChild(0).GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite;
                rectTransform.sizeDelta = new Vector2(126.8916f, 186.4073f);
                transform.SetParent(inventoryUI.SpinSlot[2]);
                rectTransform.anchoredPosition = Vector2.zero;
                ismove = false;
            }         
        }
        else if (transform.parent == inventoryUI.SpinSlot[2])
        {
            transform.position = Vector3.MoveTowards(transform.position, inventoryUI.SpinSlot[0].position, Time.deltaTime * inventoryUI.speed *2);

            if (transform.position == inventoryUI.SpinSlot[0].position)
            {
                
                transform.SetParent(inventoryUI.SpinSlot[0]);
                rectTransform.anchoredPosition = Vector2.zero;
                    ismove = false;
            }        
        }        
    }
}

