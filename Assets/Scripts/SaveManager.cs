using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEditor;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    

    [Header("Get Data Objs")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameManager manager;
    [Space(20f)]


    [Header("Save Datas")]
    public WeaponTypes playerWeapon;


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
        // playerElement = player.PlayerElementType;


        
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        
        XmlElement root = xmlDocument.CreateElement("Save");
        root.SetAttribute("FileName", "ElementGame_Data");

        // 무기 종류 저장
        XmlElement playerElementalData, ElementDatas;

        for (int i = 0; i < player.inventory.HavingElement.Length; i++)
        {
            playerElementalData = xmlDocument.CreateElement("ElementalData");
            ElementDatas = xmlDocument.CreateElement("ElementDatas");

            if (player.inventory.HavingElement[i] != null)
            {
                playerElementalData.InnerText = player.inventory.HavingElement[i].ElementalID.ToString();
            }
            else
            {
                playerElementalData.InnerText = "0";
            }

            ElementDatas.AppendChild(playerElementalData);

            root.AppendChild(ElementDatas);
        }

        xmlDocument.AppendChild(root);

        // 플레이어 스테이터스 저장
        XmlElement playerStrength = xmlDocument.CreateElement("PlayerStrength");
        playerStrength.InnerText = player.GetComponent<PlayerStatus>().strength.ToString();
        root.AppendChild(playerStrength);
        
        XmlElement playerDexterity = xmlDocument.CreateElement("PlayerDexterity");
        playerDexterity.InnerText = player.GetComponent<PlayerStatus>().dexterity.ToString();
        root.AppendChild(playerDexterity);
        
        XmlElement playerLuck = xmlDocument.CreateElement("PlayerLuck");
        playerLuck.InnerText = player.GetComponent<PlayerStatus>().luck.ToString();
        root.AppendChild(playerLuck);


        //시간 저장
        XmlElement time = xmlDocument.CreateElement("Timer");
        time.InnerText = manager.TimerVal.ToString();
        root.AppendChild(time);

        // 골드 저장
        XmlElement playerGold = xmlDocument.CreateElement("PlayerGold");
        playerGold.InnerText = player.GetComponent<Inventory>().Gold.ToString();
        root.AppendChild(playerGold);
        
        // 정수 저장
        XmlElement playerSpiritSoul = xmlDocument.CreateElement("PlayerSpiritSoul");
        playerSpiritSoul.InnerText = player.GetComponent<Inventory>().SpiritSoul.ToString();
        root.AppendChild(playerSpiritSoul);

        // 클리어한 스테이지 수 저장
        XmlElement clearStageNum = xmlDocument.CreateElement("ClearStageNum");
        clearStageNum.InnerText = manager.clearStage.ToString();
        root.AppendChild(clearStageNum);


        /* string a = KeyCode.A.ToString();
        KeyCode B = (KeyCode)System.Enum.Parse(typeof(KeyCode), a); */



        // 아이템 데이터 저장
        XmlElement playerItemData, Datas;


        for (int i = 0; i < player.inventory.HavingItem.Length; i++)
        {
            playerItemData = xmlDocument.CreateElement("ItemData");
            Datas = xmlDocument.CreateElement("Datas");
            
            if(player.inventory.HavingItem[i] != null)
            {
                playerItemData.InnerText = player.inventory.HavingItem[i].ItemID.ToString();
            }
            else
            {
                playerItemData.InnerText = "0";
            }

            Datas.AppendChild(playerItemData);

            root.AppendChild(Datas);
        }

        xmlDocument.AppendChild(root);


       /*  StringWriter stringWriter = new StringWriter();
        XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);

        xmlDocument.WriteTo(xmlTextWriter);
        string encryptString = Encrypt(stringWriter.ToString());
        SaveFile(encryptString); */
        

        // xmlDocument.Save(Application.dataPath + "/DataXML.xml");
        SaveFile(EncryptGameData(xmlDocument));
        if(File.Exists(Application.dataPath + "/DataXML.xml"))
        {
            // Debug.Log("Saved");
            // 저장 성공
        }

    }

    public void Load()
    {
        if(File.Exists(Application.dataPath + "/DataXML.xml"))
        {
            // string encryptData = LoadFile(GetPath());
            // string decryptData = Decrypt(encryptData);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(DecryptGameData());
            // xmlDocument.WriteTo(decryptData);



            // xmlDocument.Load(Application.dataPath + "/DataXML.xml");

            // 무기 타입
            XmlNodeList ElementDatas = xmlDocument.GetElementsByTagName("ElementDatas");
            if (ElementDatas.Count != 0)
            {
                for (int i = 0; i < ElementDatas.Count; i++)
                {
                    XmlNodeList playerElementalData = xmlDocument.GetElementsByTagName("ElementalData");
                    int ID = int.Parse(playerElementalData[i].InnerText);
                    player.inventory.HavingElement[i] = ElementalManager.instance.AddElement(ID);
                }
            }

            // 클리어 스테이지 개수
            XmlNodeList clearStageNum = xmlDocument.GetElementsByTagName("ClearStageNum");
            manager.clearStage = int.Parse(clearStageNum[0].InnerText);

            //시간
            XmlNodeList timer = xmlDocument.GetElementsByTagName("Timer");
            manager.TimerVal = float.Parse(timer[0].InnerText);

            // 골드
            XmlNodeList gold = xmlDocument.GetElementsByTagName("PlayerGold");
            player.GetComponent<Inventory>().Gold = int.Parse(gold[0].InnerText);

            // 정령
            XmlNodeList spiritSoul = xmlDocument.GetElementsByTagName("PlayerSpiritSoul");
            player.GetComponent<Inventory>().SpiritSoul = int.Parse(spiritSoul[0].InnerText);

            // 플레이어 스테이터스
            XmlNodeList playerStrength = xmlDocument.GetElementsByTagName("PlayerStrength");
            player.GetComponent<PlayerStatus>().strength = int.Parse(playerStrength[0].InnerText);
            XmlNodeList playerDexterity = xmlDocument.GetElementsByTagName("PlayerDexterity");
            player.GetComponent<PlayerStatus>().dexterity = int.Parse(playerDexterity[0].InnerText);
            XmlNodeList playerLuck = xmlDocument.GetElementsByTagName("PlayerLuck");
            player.GetComponent<PlayerStatus>().luck = int.Parse(playerLuck[0].InnerText);

            // 아이템
            XmlNodeList Datas = xmlDocument.GetElementsByTagName("Datas");
            if(Datas.Count != 0)
            {
                for (int i = 0; i < Datas.Count; i++)
                {
                    XmlNodeList playerItemData = xmlDocument.GetElementsByTagName("ItemData");
                    int ID = int.Parse(playerItemData[i].InnerText);
                    player.inventory.HavingItem[i] = ItemManager.instance.AddItem(ID);
                }
            }

 
            // player.PlayerElementType = playerElement;
            player.SetEquipment();
        }
        else
        {
            // Debug.Log("no file");
        }


    }


    string EncryptGameData(XmlDocument xmlDocument)
    {
        StringWriter stringWriter = new StringWriter();
        XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
        xmlDocument.WriteTo(xmlTextWriter);
        string encryptString = Encrypt(stringWriter.ToString());

        return encryptString;
    }

    string DecryptGameData()
    {
        
        string encryptData = LoadFile(GetPath());
        string decryptData = Decrypt(encryptData);

        return decryptData;
    }

    
    public void ResetData()
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        
        XmlElement root = xmlDocument.CreateElement("Save");
        root.SetAttribute("FileName", "ElementGame_Data");

        // 무기 종류 저장
        XmlElement playerElementalData, ElementDatas;
        for (int i = 0; i < player.inventory.HavingItem.Length; i++)
        {
            playerElementalData = xmlDocument.CreateElement("ElementalData");
            ElementDatas = xmlDocument.CreateElement("ElementDatas");

            playerElementalData.InnerText = "0";


            ElementDatas.AppendChild(playerElementalData);

            root.AppendChild(ElementDatas);
        }

        xmlDocument.AppendChild(root);

        // 시간
        XmlElement time = xmlDocument.CreateElement("Timer");
        time.InnerText = "0";
        root.AppendChild(time);

        // 골드 저장
        XmlElement playerGold = xmlDocument.CreateElement("PlayerGold");
        playerGold.InnerText = 0.ToString();
        root.AppendChild(playerGold);

        
        // 플레이어 스테이터스 저장
        XmlElement playerStrength = xmlDocument.CreateElement("PlayerStrength");
        playerStrength.InnerText = 0.ToString();
        root.AppendChild(playerStrength);
        XmlElement playerDexterity = xmlDocument.CreateElement("PlayerDexterity");
        playerDexterity.InnerText = 0.ToString();
        root.AppendChild(playerDexterity);
        XmlElement playerLuck = xmlDocument.CreateElement("PlayerLuck");
        playerLuck.InnerText = 0.ToString();
        root.AppendChild(playerLuck);
        
        
        // 정수 저장
        XmlElement playerSpiritSoul = xmlDocument.CreateElement("PlayerSpiritSoul");
        playerSpiritSoul.InnerText = 0.ToString();
        root.AppendChild(playerSpiritSoul);

        // 클리어한 스테이지 수 저장
        XmlElement clearStageNum = xmlDocument.CreateElement("ClearStageNum");
        clearStageNum.InnerText = 0.ToString();
        root.AppendChild(clearStageNum);

        // 아이템 데이터 저장
        XmlElement playerItemData, Datas;


        for (int i = 0; i < player.inventory.HavingItem.Length; i++)
        {
            playerItemData = xmlDocument.CreateElement("ItemData");
            Datas = xmlDocument.CreateElement("Datas");
            
            playerItemData.InnerText = "0";
            

            Datas.AppendChild(playerItemData);

            root.AppendChild(Datas);
        }

        xmlDocument.AppendChild(root);

        // xmlDocument.Save(Application.dataPath + "/DataXML.xml");
        SaveFile(EncryptGameData(xmlDocument));
        if(File.Exists(Application.dataPath + "/DataXML.xml"))
        {
            //Debug.Log("Saved");
            //저장 성공
        }

    }



    static string GetPath()
    {
        return Path.Combine(Application.dataPath + "/DataXML.xml");
    }




    static void SaveFile(string Data)
    {
        using (FileStream fs = new FileStream(GetPath(), FileMode.Create, FileAccess.Write))
        {
            //파일로 저장할 수 있게 바이트화
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Data);
 
            //bytes의 내용물을 0 ~ max 길이까지 fs에 복사
            fs.Write(bytes, 0, bytes.Length);
        }
    }


    static string LoadFile(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            //파일을 바이트화 했을 때 담을 변수를 제작
            byte[] bytes = new byte[(int)fs.Length];
 
            //파일스트림으로 부터 바이트 추출
            fs.Read(bytes, 0, (int)fs.Length);
 
            //추출한 바이트를 json string으로 인코딩
            string jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            return jsonString;
        }
    }

    private static readonly string privateKey = "6gh45hrgts9za2mnlpv63nd32sdzx6dryt7645dfg34wdfx4rtyughc7tu";


    private static string Encrypt(string data)
    {
 
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateEncryptor();
        byte[] results = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Convert.ToBase64String(results, 0, results.Length);
 
    }

    private static string Decrypt(string data)
    {
 
        byte[] bytes = System.Convert.FromBase64String(data);
        RijndaelManaged rm = CreateRijndaelManaged();
        ICryptoTransform ct = rm.CreateDecryptor();
        byte[] resultArray = ct.TransformFinalBlock(bytes, 0, bytes.Length);
        return System.Text.Encoding.UTF8.GetString(resultArray);
    }



    private static RijndaelManaged CreateRijndaelManaged()
    {
        byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(privateKey);
        RijndaelManaged result = new RijndaelManaged();
 
        byte[] newKeysArray = new byte[16];
        System.Array.Copy(keyArray, 0, newKeysArray, 0, 16);
 
        result.Key = newKeysArray;
        result.Mode = CipherMode.ECB;
        result.Padding = PaddingMode.PKCS7;
        return result;
    }

    public void AutoSave()
    {
        Save();
        Invoke("AutoSave", 30f);
    }

   
}
