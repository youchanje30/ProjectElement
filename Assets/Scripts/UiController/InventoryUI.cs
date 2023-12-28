using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform[] rectTransform;
    public GameManager gameManager;

    [Header("Inventory Setting")]
    
    public GameObject[] ItemInfo;
    public RectTransform Infopos;
    public GameObject Info;
    public RectTransform[] Card;


    Canvas canvas;

    public RectTransform targetRectTr;
    public Camera uiCamera;
    public Vector2 screenPoint;
    public float Speed;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < rectTransform.Length; i++) { rectTransform[i] = GetComponent<RectTransform>(); }
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

        //MoveInformation();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
       
        MoveElementsCard();
    }
    public void MoveElementsCard()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < rectTransform.Length; i++) 
            {
                if (rectTransform[i].anchoredPosition.x == 673 && rectTransform[i].anchoredPosition.y == 278)
                {
                    rectTransform[i].anchoredPosition = new Vector3(286, 278, 0);
                }
                else if (rectTransform[i].anchoredPosition.x == 286 && rectTransform[i].anchoredPosition.y == 278)
                {
                    rectTransform[i].anchoredPosition = new Vector3(479, 234, 2);
                    rectTransform[i].SetAsLastSibling();
                }
                else if (rectTransform[i].anchoredPosition.x == 479 && rectTransform[i].anchoredPosition.y == 234)
                {
                    rectTransform[i].anchoredPosition = new Vector3(673, 278, 0);
                } 
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
        for(int i = 0; i< Card.Length; i++)
        {
            if (rectTransform[i] == Card[i])
            { Card[i].anchoredPosition = screenPoint; }
           
        }
        

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
        Info.SetActive(true);
            
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Info.SetActive(false);
    }

    public void MoveInformation()
    {
        Infopos.SetAsLastSibling();
        Infopos.anchoredPosition = new Vector3(screenPoint.x - 136, screenPoint.y + 196);
        
    }
}
