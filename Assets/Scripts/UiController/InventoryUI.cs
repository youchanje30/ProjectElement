using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IPointerEnterHandler
{
    public RectTransform rectTransform;
    public GameManager gameManager;

    [Header("Inventory Setting")]
    public GameObject InvenUI;
    public GameObject[] ItemInfo;
    public RectTransform[] ElementInfopos;
    public GameObject[] ElementInfo;
    public RectTransform Card;


    Canvas canvas;

    public RectTransform targetRectTr;
    public Camera uiCamera;
    public Vector2 screenPoint;

    public bool isInven = false;
    public float Speed;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rectTransform = this.GetComponent<RectTransform>();
    }
    void Start()
    {
        for (int i = 0; i < ElementInfo.Length; i++) { ElementInfo[i].SetActive(false); }
        for (int i = 0; i < ItemInfo.Length; i++) { ItemInfo[i].SetActive(false); }
        targetRectTr = GameObject.FindGameObjectWithTag("UI").GetComponent<RectTransform>();
        uiCamera = Camera.main;
        InvenUI.SetActive(isInven);
      
    }

    // Update is called once per frame
    void Update()
    {
        OpenInformation();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ElementImg();
            SetItem();
            isInven = !isInven;
            InvenUI.SetActive(isInven);
        }
        MoveElementsCard();
    }
    public void MoveElementsCard()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (rectTransform.anchoredPosition.x == 768 && rectTransform.anchoredPosition.y == 13)
            {
                rectTransform.anchoredPosition = new Vector3(381, 13, 0);
            }
            else if (rectTransform.anchoredPosition.x == 381 && rectTransform.anchoredPosition.y == 13)
            {
                rectTransform.anchoredPosition = new Vector3(574, -31, 2);
                rectTransform.SetAsLastSibling();
            }
            else if (rectTransform.anchoredPosition.x == 574 && rectTransform.anchoredPosition.y == -31)
            {
                rectTransform.anchoredPosition = new Vector3(768, 13, 0);
            }
        }
    }
    public void ElementImg()
    {
        for (int i = 0; i < gameManager.inventory.HasWeapon.Length; i++)
        {
            if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Sword)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[1];
            }
            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Wand)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[2];
            }
            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Shield)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[3];
            }
            else if (gameManager.inventory.HavingWeapon[i] == (int)WeaponTypes.Bow)
            {
                gameManager.EleCards[i].sprite = gameManager.Ele[4];
            }
        }

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
        Debug.Log("point down");
        OnDrag(eventData);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

       Card.anchoredPosition = screenPoint;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ElementInfo[0].SetActive(true);
        ElementInfopos[0].anchoredPosition = screenPoint;
    }


}
