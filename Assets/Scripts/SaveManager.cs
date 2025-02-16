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
    public bool isFirstTime;

    [Header("Get Data Objs")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameManager manager;
    [Space(20f)]


    [Header("Save Datas")]
    public WeaponTypes playerWeapon;


    void Awake()
    {
        if(!instance)
            instance = this;
        else
            Destroy(gameObject);    
        
        if(!player)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        

        if(!manager)
            manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        
    }



    public void Save()
    {
        playerWeapon = player.PlayerWeaponType;

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

        
        // 플레이어 현재 무기
        XmlElement curPlayerWeapon = xmlDocument.CreateElement("CurPlayerWeapon");
        curPlayerWeapon.InnerText = ((int)(player.GetComponent<PlayerController>().PlayerWeaponType)).ToString();
        root.AppendChild(curPlayerWeapon);
        

        // 플레이어 스테이터스 저장
        // XmlElement playerStrength = xmlDocument.CreateElement("PlayerStrength");
        // playerStrength.InnerText = player.GetComponent<PlayerStatus>().strength.ToString();
        // root.AppendChild(playerStrength);
        
        // XmlElement playerDexterity = xmlDocument.CreateElement("PlayerDexterity");
        // playerDexterity.InnerText = player.GetComponent<PlayerStatus>().dexterity.ToString();
        // root.AppendChild(playerDexterity);
        
        // XmlElement playerLuck = xmlDocument.CreateElement("PlayerLuck");
        // playerLuck.InnerText = player.GetComponent<PlayerStatus>().luck.ToString();
        // root.AppendChild(playerLuck);


        //시간 저장
        XmlElement time = xmlDocument.CreateElement("Timer");
        time.InnerText = manager.TimerVal.ToString();
        root.AppendChild(time);

        // 골드 저장
        XmlElement playerGold = xmlDocument.CreateElement("PlayerGold");
        playerGold.InnerText = player.GetComponent<Inventory>().Gold.ToString();
        root.AppendChild(playerGold);
        
        // 현재 체력
        XmlElement curHp = xmlDocument.CreateElement("PlayerHp");
        curHp.InnerText = player.GetComponent<PlayerStatus>().curHp.ToString();
        root.AppendChild(curHp);

        // 정수 저장
        // XmlElement playerSpiritSoul = xmlDocument.CreateElement("PlayerSpiritSoul");
        // playerSpiritSoul.InnerText = player.GetComponent<Inventory>().SpiritSoul.ToString();
        // root.AppendChild(playerSpiritSoul);

        // 클리어한 스테이지 수 저장
        XmlElement clearStageNum = xmlDocument.CreateElement("ClearStageNum");
        clearStageNum.InnerText = manager.clearStage.ToString();
        root.AppendChild(clearStageNum);

        // 현재 스테이지 저장
        XmlElement curStageName = xmlDocument.CreateElement("CurStageName");
        curStageName.InnerText = manager.curStageName;
        root.AppendChild(curStageName);



        //스테이지 번호 저장
        XmlElement sceneNum = xmlDocument.CreateElement("SceneNum");
        sceneNum.InnerText = manager.SceneNum.ToString();
        root.AppendChild(sceneNum);
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
        // xmlDocument.Save(Application.dataPath + "/DataXML.xml");
            xmlDocument.LoadXml(DecryptGameData());
            
            // xmlDocument.Save(Application.dataPath + "/TESTDataXML.xml");
            // xmlDocument.Load(Application.dataPath + "/DataXML.xml");

            // 무기 타입
            XmlNodeList ElementDatas = xmlDocument.GetElementsByTagName("ElementDatas");
            if (ElementDatas.Count != 0)
            {
                // for (int i = 0; i < ElementDatas.Count; i++)
                for (int i = 0; i < 3; i++)
                {
                    XmlNodeList playerElementalData = xmlDocument.GetElementsByTagName("ElementalData");
                    int ID = int.Parse(playerElementalData[i].InnerText);
                    player.inventory.HavingElement[i] = ElementalManager.instance.AddElement(ID);
                }
            }

            // 클리어 스테이지 개수
            XmlNodeList clearStageNum = xmlDocument.GetElementsByTagName("ClearStageNum");
            manager.clearStage = int.Parse(clearStageNum[0].InnerText);

            //스테이지 번호
            XmlNodeList sceneNum = xmlDocument.GetElementsByTagName("SceneNum");
            manager.SceneNum = int.Parse(sceneNum[0].InnerText);

            // 현재 스테이지
            XmlNodeList sceneName = xmlDocument.GetElementsByTagName("CurStageName");
            manager.curStageName = sceneName[0].InnerText;

            // 플레이어 현재 무기
            XmlNodeList curPlayerWeapon = xmlDocument.GetElementsByTagName("CurPlayerWeapon");
            int playerWeapon = int.Parse(curPlayerWeapon[0].InnerText);
            player.PlayerWeaponType = (WeaponTypes)playerWeapon;


            //시간
            XmlNodeList timer = xmlDocument.GetElementsByTagName("Timer");
            manager.TimerVal = float.Parse(timer[0].InnerText);

            // 골드
            XmlNodeList gold = xmlDocument.GetElementsByTagName("PlayerGold");
            player.GetComponent<Inventory>().Gold = int.Parse(gold[0].InnerText);

            // 현재 체력
            XmlNodeList curHp = xmlDocument.GetElementsByTagName("PlayerHp");
            player.GetComponent<PlayerStatus>().curHp = float.Parse(curHp[0].InnerText);

            // 정령
            // XmlNodeList spiritSoul = xmlDocument.GetElementsByTagName("PlayerSpiritSoul");
            // player.GetComponent<Inventory>().SpiritSoul = int.Parse(spiritSoul[0].InnerText);
            

            // 플레이어 스테이터스
            // XmlNodeList playerStrength = xmlDocument.GetElementsByTagName("PlayerStrength");
            // player.GetComponent<PlayerStatus>().strength = int.Parse(playerStrength[0].InnerText);
            // XmlNodeList playerDexterity = xmlDocument.GetElementsByTagName("PlayerDexterity");
            // player.GetComponent<PlayerStatus>().dexterity = int.Parse(playerDexterity[0].InnerText);
            // XmlNodeList playerLuck = xmlDocument.GetElementsByTagName("PlayerLuck");
            // player.GetComponent<PlayerStatus>().luck = int.Parse(playerLuck[0].InnerText);

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

        }
        else
        {
            player.GetComponent<PlayerStatus>().curHp = player.GetComponent<PlayerStatus>().basicMaxHp;
            isFirstTime = true;
            
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
            ElementDatas.InnerText = "0";


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
        // XmlElement playerStrength = xmlDocument.CreateElement("PlayerStrength");
        // playerStrength.InnerText = 0.ToString();
        // root.AppendChild(playerStrength);
        // XmlElement playerDexterity = xmlDocument.CreateElement("PlayerDexterity");
        // playerDexterity.InnerText = 0.ToString();
        // root.AppendChild(playerDexterity);
        // XmlElement playerLuck = xmlDocument.CreateElement("PlayerLuck");
        // playerLuck.InnerText = 0.ToString();
        // root.AppendChild(playerLuck);


        // 현재 체력
        XmlElement curHp = xmlDocument.CreateElement("PlayerHp");
        curHp.InnerText = player.GetComponent<PlayerStatus>().basicMaxHp.ToString();
        root.AppendChild(curHp);
        // 정수 저장
        // XmlElement playerSpiritSoul = xmlDocument.CreateElement("PlayerSpiritSoul");
        // playerSpiritSoul.InnerText = 0.ToString();
        // root.AppendChild(playerSpiritSoul);

        // 클리어한 스테이지 수 저장
        XmlElement clearStageNum = xmlDocument.CreateElement("ClearStageNum");
        clearStageNum.InnerText = 0.ToString();
        root.AppendChild(clearStageNum);

        XmlElement sceneNum = xmlDocument.CreateElement("SceneNum");
        sceneNum.InnerText = 0.ToString();
        root.AppendChild(sceneNum);

        
        // 플레이어 현재 무기
        XmlElement curPlayerWeapon = xmlDocument.CreateElement("CurPlayerWeapon");
        curPlayerWeapon.InnerText = 0.ToString();
        root.AppendChild(curPlayerWeapon);

        // 현재 스테이지
        XmlElement curStageName = xmlDocument.CreateElement("CurStageName");
        curStageName.InnerText = "Main Scene";
        root.AppendChild(curStageName);


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

        SaveFile(EncryptGameData(xmlDocument));
        if(File.Exists(Application.dataPath + "/DataXML.xml"))
        {
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
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Data);
 
            fs.Write(bytes, 0, bytes.Length);
        }
    }


    static string LoadFile(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            byte[] bytes = new byte[(int)fs.Length];
 
            fs.Read(bytes, 0, (int)fs.Length);
 
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
        Debug.Log("Auto Saved");
        Save();
        Invoke("AutoSave", 5f);
    }

   
}
