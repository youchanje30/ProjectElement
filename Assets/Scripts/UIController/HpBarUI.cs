using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    InventoryUI InventoryUI;
    [SerializeField]private GameObject[] ElementalImage;
   
    void Start()
    {
        InventoryUI = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        SetImage();
    }

    public void SetImage()
    {
        for (int i = 0; i < ElementalImage.Length; i++)
        {
            if (InventoryUI.inventory.HavingElement[i].ElementalID != 0)
            {
                ElementalImage[i].GetComponent<Image>().sprite = InventoryUI.inventory.HavingElement[i].elementalImg;
                Color color = ElementalImage[i].GetComponent<Image>().color;
                color.a = 1f;
                ElementalImage[i].GetComponent<Image>().color = color;
            }
            else
            {
                Color color = ElementalImage[i].GetComponent<Image>().color;
                color.a = 0f;
                ElementalImage[i].GetComponent<Image>().color = color;
            }
           
        }
    }
}