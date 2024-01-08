using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wealth : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    public GameObject WealthUI;
    public TMP_Text Gold;
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Gold.text = manager.inventory.Gold.ToString();
    }
}
