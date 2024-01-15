using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : MonoBehaviour
{
    [SerializeField] private Battle battle;
    public float[] SkillCoolTime;
    public bool[] SkillReady;
    [Header("��")]
    public GameObject FireFloor;

    [Header("�ٶ�")]
    public GameObject Arrow;
    public Transform Pos;
    public bool isCharging = false;
    public float ChargeTime;
    public float InvicibleTime;
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
        //�ٴڿ��� �� ������ ������� 
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
        isCharging = false;
        yield return new WaitForSeconds(InvicibleTime);
        battle.isGuard = false;
        StartCoroutine(ReturnAttack());
        
        //��¡�ð�, ȭ�쵥���� 
    }
    public IEnumerator ReturnAttack()
    {
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
