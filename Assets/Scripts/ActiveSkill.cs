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
    [Tooltip("��¡ �ð�")]
    public float ChargeTime;
    [Tooltip("�߻� �� ���� �ð�")]
    public float InvicibleTime;
    [Tooltip("�߻� �� �ݵ�")]
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
