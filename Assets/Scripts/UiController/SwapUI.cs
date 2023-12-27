using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwapUI : MonoBehaviour
{
    public RectTransform rectTransform;
    private GameManager gameManager;

    
    void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();     
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public void Start()
    {
    }
    public void Update()
    {
        sellectSlot();
    }
 
    public void sellectSlot()
    {     
        if (Input.GetKeyDown(KeyCode.RightArrow) && rectTransform.anchoredPosition.x < 562)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + 562, -277);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && rectTransform.anchoredPosition.x > -562)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - 562, -277);
        }

        if (rectTransform.anchoredPosition.x == -562)
        {
            gameManager.slot = 0;
        }
        else if (rectTransform.anchoredPosition.x == 0)
        {
            gameManager.slot = 1;
        }
        else if (rectTransform.anchoredPosition.x == 562)
        {
            gameManager.slot = 2;
        }
    }
   
}
