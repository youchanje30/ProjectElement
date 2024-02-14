using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Elemental Data", menuName = "Scriptable Object/Elemental Data", order = int.MaxValue)]
public class ElementalData : ScriptableObject
{
    [Header("Element Info")]
    [SerializeField] private string elementalName;
    public string ElementalName { get { return elementalName; } }
    [SerializeField] private WeaponTypes weaponType;
    public WeaponTypes WeaponTypes { get { return weaponType; } }
    [SerializeField] private int elementalID;
    public int ElementalID { get { return elementalID; } }
    [SerializeField] private string elementalInfo;
    public string ElementalInfo { get { return elementalInfo; } }
    public Sprite elementalImg;
    public Sprite elementalIcon;
    public Sprite UnSelcIcon;
    public Sprite elementalCard;
}