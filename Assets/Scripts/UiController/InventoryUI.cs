using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public RectTransform rectTransform;
    private GameManager gameManager;
    [Header("Inventory Setting")]
    public GameObject InvenUI;
    public bool isInven = false;
    public float Speed;
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rectTransform = this.GetComponent<RectTransform>();
    }
    void Start()
    {
        InvenUI.SetActive(isInven);
      
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
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


}
