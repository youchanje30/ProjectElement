using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    [SerializeField] private Battle battle;
    public float[] SkillCoolTime;
    public bool[] SkillReady;
    [Header("불")]
    public GameObject FireFloor;

    [Header("바람")]
    public GameObject Arrow;
    public Transform Pos;
    public bool isCharging = false;
    [Tooltip("차징 시간")]
    public float ChargeTime;
    [Tooltip("발사 시 무적 시간")]
    public float InvicibleTime;
    [Tooltip("발사 후 반동")]
    public float Delay;
    void Awake()
    {
        battle = GetComponent<Battle>();
        for (int i = 0; i < SkillReady.Length; i++)
        {
            SkillReady[i] = true;
        }
    }
    void Update()
    {
    }

    public void TriggerSkill(WeaponTypes weapontype)
    {
        switch (weapontype)
        {
            case WeaponTypes.Sword:
                FIreSkill();
                break;
            case WeaponTypes.Wand:
                WaterSkill();
                break;
            case WeaponTypes.Shield:
                SouthSkill();
                break;
            case WeaponTypes.Bow:
                StartCoroutine(WindSkill());
                break;
        }
    }
    public void FIreSkill()
    {
        //바닥에만 불 장판이 깔려야함 
        Instantiate(FireFloor);

    }
    public void WaterSkill()
    {

    }
    public void SouthSkill()
    {

    }
    public IEnumerator WindSkill()
    {
        isCharging = true;
        SkillReady[(int)battle.WeaponType] = false;    
        yield return new WaitForSeconds(ChargeTime);
        battle.isGuard = true;
        GameObject MagicArrow = Instantiate(Arrow);
        MagicArrow.transform.position = Pos.position;
        MagicArrow.transform.localScale = new Vector3(transform.localScale.x, MagicArrow.transform.localScale.y, MagicArrow.transform.localScale.z);
        StartCoroutine(ReturnAttack());
        yield return new WaitForSeconds(InvicibleTime);
        battle.isGuard = false;
    }
    public IEnumerator ReturnAttack()
    {
        yield return new WaitForSeconds(Delay);
        isCharging = false;
        for (int i = 0; i < SkillReady.Length; i++)
        {
            if (!SkillReady[i])
            {
                yield return new WaitForSeconds(SkillCoolTime[i]);
                SkillReady[i] = true;
            }
        }
    }
}
