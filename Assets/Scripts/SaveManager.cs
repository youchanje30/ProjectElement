using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class SaveManager : MonoBehaviour
{

    public static SaveManager instance;
    

    [Header("Get Data Objs")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameManager manager;
    [Space(20f)]

    [Header("Save Datas")]
    public WeaponTypes playerWeapon;
    public Elements playerElement;

    // [Space(20f)]


    void Awake()
    {
        instance = this;
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        if(manager == null)
        {
            manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
    }



    public void Save()
    {
        playerWeapon = player.PlayerWeaponType;
        playerElement = player.PlayerElementType;


        
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        
        XmlElement root = xmlDocument.CreateElement("Save");
        root.SetAttribute("FileName", "ElementGame_Data");

        XmlElement playerWeaponData = xmlDocument.CreateElement("PlayerWeaponType");
        playerWeaponData.InnerText = playerWeapon.ToString();
        root.AppendChild(playerWeaponData);

        XmlElement playerElementData = xmlDocument.CreateElement("PlayerElementType");
        playerElementData.InnerText = playerElement.ToString();
        root.AppendChild(playerElementData);

        xmlDocument.AppendChild(root);

        xmlDocument.Save(Application.dataPath + "/DataXML.xml");
        if(File.Exists(Application.dataPath + "/DataXML.xml"))
        {
            Debug.Log("Saved");
            // 저장 성공
        }

    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/DataXML.xml"))
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Application.dataPath + "/DataXML.xml");

            XmlNodeList playerWeaponData = xmlDocument.GetElementsByTagName("PlayerWeaponType");
            WeaponTypes PlayerWeaponData = (WeaponTypes)System.Enum.Parse(typeof(WeaponTypes), playerWeaponData[0].InnerText);
            playerWeapon = PlayerWeaponData;


            XmlNodeList playerElementData = xmlDocument.GetElementsByTagName("PlayerElementType");
            Elements PlayerElementData = (Elements)System.Enum.Parse(typeof(Elements), playerElementData[0].InnerText);
            playerElement = PlayerElementData;
            

        
            player.PlayerWeaponType = playerWeapon;
            player.PlayerElementType = playerElement;
            player.ChangeEquipment();
        }
        else
        {
            Debug.Log("no file");
        }


    }


    public void AutoSave()
    {
        Save();
        Invoke("AutoSave", 30f);
    }
}
