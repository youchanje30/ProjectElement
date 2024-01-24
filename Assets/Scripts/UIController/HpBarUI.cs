using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarUI : MonoBehaviour
{
    InventoryUI InventoryUI;
    [SerializeField]private GameObject[] ElementalImage;
    public Image HpFIll;
    [SerializeField] private PlayerStatus status;

    void Start()
    {
        InventoryUI = GameObject.FindGameObjectWithTag("UI").GetComponent<InventoryUI>();
        status = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
         HpFIll.fillAmount = Mathf.Lerp(HpFIll.fillAmount, status.curHp / status.maxHp, Time.deltaTime * 5f);
        //SetImage();
    } 

    //public void SetImage()
    //{
    //    for (int i = 0; i < ElementalImage.Length; i++)
    //    {
    //        if (InventoryUI.inventory.HavingElement[i].ElementalID != 0)
    //        {
    //            ElementalImage[i].GetComponent<Image>().sprite = InventoryUI.inventory.HavingElement[i].elementalImg;
    //            Color color = ElementalImage[i].GetComponent<Image>().color;
    //            color.a = 1f;
    //            ElementalImage[i].GetComponent<Image>().color = color;
    //        }
    //        else
    //        {
    //            Color color = ElementalImage[i].GetComponent<Image>().color;
    //            color.a = 0f;
    //            ElementalImage[i].GetComponent<Image>().color = color;
    //        }
           
    //    }
    //}
}