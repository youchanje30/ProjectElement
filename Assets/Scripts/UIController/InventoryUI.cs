using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

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
    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRectTr, Input.mousePosition, uiCamera, out screenPoint);
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