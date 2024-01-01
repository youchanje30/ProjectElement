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
    public Sprite elementalImg;
}