using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementalManager : MonoBehaviour
{
    public static ElementalManager instance;

    [SerializeField] private List<ElementalData> ElementalDatas;

    public List<ElementalData> elementalDatas = new List<ElementalData>();

    void Awake()
    {
        instance = this;
        SortingElement();
    }


    public void SortingElement()
    {
        foreach (ElementalData Element in ElementalDatas)
        {          
            elementalDatas.Add(Element);
        }

    }
    public ElementalData AddElement(int ID)
    {
        //if (ID == 0) return null;

        ElementalData AddingEle = null;

        foreach (ElementalData elementalData in ElementalDatas)
        {
            if (elementalData.ElementalID == ID)
            {
                AddingEle = elementalData;
                break;
            }
        }

        return AddingEle;
    }
}
