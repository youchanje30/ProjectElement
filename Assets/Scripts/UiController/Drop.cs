using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drop : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rectTransform;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
    public void OnDrop(PointerEventData eventData)
    {
         if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rectTransform.position;
        }
    }
}
