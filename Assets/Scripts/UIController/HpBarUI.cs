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
        for (int i = 0; i < ElementalImage.Length; i++)
        {
                ElementalImage[i].GetComponent<Image>().sprite = InventoryUI.inventory.HavingElement[i].elementalImg;          
        }
    }
}
