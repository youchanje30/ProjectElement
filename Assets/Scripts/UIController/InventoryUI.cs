using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class  InventoryUI: MonoBehaviour
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
    public GameObject[] InvenItem;
    public GameObject InvenUI;
    public TextMeshProUGUI Stat;
    public Animator animator;
    public GameObject inven;
    public GameObject[] Slot;

    [Header("Inventory Swap")]
    public Transform[] SpinSlot;
    public float speed = 6;
    public Sprite[] frame;
    public Transform[] trans;

    [Space(20f)]
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
    }
    void Update()
    {
        if(Info.activeSelf == true || ItemInfo.activeSelf == true)
        {
            Info.GetComponent<RectTransform>().SetAsLastSibling();
            Info.transform.position = new Vector3(Input.mousePosition.x - 90, Input.mousePosition.y - 140);
            ItemInfo.GetComponent<RectTransform>().SetAsLastSibling();
            ItemInfo.transform.position = new Vector3(Input.mousePosition.x - 160, Input.mousePosition.y + 100);
        }
 
    
    }
    public void SetCard()
    {
        for (int i = 0; i < EleCards.Length; i++)
        {
            EleCards[i].GetComponent<Image>().sprite = inventory.HavingElement[i].elementalImg;
            Slot[i].GetComponent<Text>().text = inventory.HavingElement[i].ElementalID.ToString();
            Slot[i].GetComponent<CardController>().Setslot = i;
            if (EleCards[i].GetComponent<Image>().sprite != null)
            {
                EleCards[i].SetActive(true);
            }
            else
            {
                EleCards[i].SetActive(false);
            }
        }
        for (int i = 0; i < InvenItem.Length; i++)
        {           
           
            if (inventory.HavingItem[i] != null)
            {
                InvenItem[i].SetActive(true);
                InvenItem[i].GetComponent<Image>().sprite = inventory.HavingItem[i].itemImg.sprite;
            }
            else
            {
                InvenItem[i].SetActive(false);
            }
        }
    }
    public void SetItem()
    {

    }
    public IEnumerator InventoryAnim()
    {
        inven.SetActive(gameManager.isInven);
        ItemInfo.SetActive(false);
        Info.SetActive(false);
        gameManager.isInven = !gameManager.isInven;
        InvenUI.SetActive(gameManager.isInven);
        animator.SetTrigger("OpenInven");
        yield return new WaitForSeconds(0.2f);
        inven.SetActive(gameManager.isInven);
        SetCard();
        SetItem();
    }  
}
