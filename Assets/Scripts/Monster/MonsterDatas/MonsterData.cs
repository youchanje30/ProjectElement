using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    [Header("몬스터 초기 정보")]
    public DetectTypes detectType;
    public AtkTypes atkType;
    public float maxMoveSpeed;
    public float maxHp;
    public float maxDamage;
    public float maxAtkCoolTime;
    public float maxKnockbackTime;
    [Space(20f)]

    [Header("몬스터 상태 관련 정보")]
    public float imageScale;
    public float floorRayX;
    public float floorRayY;
    // public Vector2 detectSize;
    // [Space(20f)]

}