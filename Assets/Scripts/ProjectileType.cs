using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Arrow, Magic, WindSkill, WaterSkill, Bomb, FireSkill
}

public class ProjectileType : MonoBehaviour
{
    public Type Projectile;
    private CapsuleCollider2D Collider2D;
    [SerializeField] Battle battle;
    public WeaponTypes weaponType;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] int groundLayer = 6;
    ActiveSkill skill;
    public float Damage;
    public float moveSpeed;
    public Transform target;

    public float duration;
    public float tick;
    public float per;
    [Tooltip("관통 후 데미지 감소율 %")]
    public float DeclineRate;
    
    
    void Awake()
    {
        if(!rigid)
            rigid = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<CapsuleCollider2D>();
        skill = GameObject.FindGameObjectWithTag("Player").GetComponent<ActiveSkill>();
        battle = skill.GetComponent<Battle>();

        if (Projectile != Type.FireSkill && Projectile != Type.WaterSkill)
        {
            Invoke("Remove", 5f);
        }
        else if(Projectile == Type.FireSkill)
        {
            Invoke("Remove",skill.ActiveTime );
        }
        else if(Projectile == Type.WaterSkill )
        {
            Invoke("Remove", skill.activeTime);
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (Projectile)
        {
            case Type.Arrow:
                rigid.velocity = new Vector2(transform.localScale.x, 0).normalized * moveSpeed;
                break;

            case Type.Magic:
                if (target != null && target.parent.gameObject.activeSelf)
                    rigid.velocity = (target.position - transform.position).normalized * moveSpeed;
                else
                    rigid.velocity = new Vector2(transform.localScale.x, 0).normalized * moveSpeed;
                break;

            case Type.WindSkill:
                transform.position += new Vector3(transform.localScale.x, 0, 0) * Time.deltaTime * moveSpeed;
                break;

            case Type.Bomb:
                // StartCoroutine(BombAtk());
                break;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.tag);
        // if (other.CompareTag("Floor"))
        // {
        //     if (Projectile != Type.WindSkill && Projectile != Type.WaterSkill && Projectile != Type.FireSkill)
        //     {
        //         Destroy(gameObject);
        //     }
        // }
        // else if (other.CompareTag("Monster") && Projectile != Type.Bomb)
        if(other.gameObject.layer == groundLayer && (Projectile == Type.Arrow || Projectile == Type.Magic || Projectile == Type.WindSkill))
        {
            Remove();
        }
        if (other.CompareTag("Monster") && Projectile != Type.Bomb && Projectile != Type.FireSkill)
        {
            other.GetComponentInParent<MonsterBase>().GetDamaged(Damage);
            EffectManager.instance.SpawnEffect(other.transform.position, 1 + (int)weaponType, Vector2.one);

            if (Projectile == Type.Magic)
            {
                other.GetComponentInParent<MonsterDebuffBase>().ContinueBuff(0f, duration, tick, BuffTypes.Slow, per);
                battle.PlayerSynergy(other.gameObject);
            }
            if (Projectile == Type.Arrow)
            {
                battle.PlayerSynergy(other.gameObject);
            }
            if (Projectile != Type.WindSkill && Projectile != Type.WaterSkill )
            {
                Destroy(gameObject);
            }
            if (Projectile == Type.WaterSkill)
            {
                skill.passive.ActivePassive(WeaponTypes.Wand, other.GetComponentInParent<MonsterDebuffBase>());
                //Damage *= 1 - (DeclineRate / 100);
            }    
        }
        else if (other.CompareTag("Destruct"))
        {
            other.GetComponent<DestructObject>().DestroyObj();
            if (Projectile != Type.WindSkill && Projectile != Type.WaterSkill && Projectile != Type.Bomb)
            {
                Destroy(gameObject);
            }
        }

        if (Projectile == Type.WaterSkill)
        {
            if (other.gameObject.layer == groundLayer)
            {
                WaterY(skill.WaterY);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        { 
            if (Projectile == Type.FireSkill)
            {
                skill.passive.ActivePassive(WeaponTypes.Sword, other.GetComponentInParent<MonsterDebuffBase>());
            }
        }
    }

    void Remove()
    {
        Destroy(gameObject);
    }

    public IEnumerator BombAtk()
    {
        yield return new WaitForSeconds(skill.BombChargeTime - 0.5f);
        GetComponent<Animator>().SetTrigger("Bomb");

        yield return new WaitForSeconds(0.5f);
        CameraController.instance.ShakeCamera(skill.BombShakeTime,skill.BombShakeMagnitude);
        
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, skill.BombRange, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Monster"))
            {
                
                skill.passive.ActivePassive(WeaponTypes.Wand, collider.GetComponentInParent<MonsterDebuffBase>());
                skill.SkillAtk(collider.gameObject, skill.DefaultDamage * (1 + (skill.BombDamageIncreaseRate / 100)));              
            }
            if (collider.CompareTag("Destruct"))
            {
                skill.SkillAtk(collider.gameObject, skill.DefaultDamage * (1 + (skill.BombDamageIncreaseRate / 100)));
            }
        }
        
        Remove();
    }
    public void WaterY(float y = 0f)
    {

        //     transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        //     transform.GetComponent<Rigidbody2D>().position += new Vector2(0, 0.4f);
        //     transform.GetComponent<Rigidbody2D>().gravityScale = 0f;
        //     transform.GetComponent<ProjectileType>().Projectile = Type.Bomb;

        //     StartCoroutine(BombAtk());
        // }
        if (transform.position.y <= y)
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            // transform.GetComponent<Rigidbody2D>().position += new Vector2(0, 0.4f);
            transform.GetComponent<Rigidbody2D>().gravityScale = 0f;
            transform.GetComponent<ProjectileType>().Projectile = Type.Bomb;
        }
        
        StartCoroutine(BombAtk());
    }
    private void OnDrawGizmos()
    {
        if (Projectile == Type.WaterSkill || Projectile == Type.Bomb)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(new Vector3(transform.position.x , transform.position.y), new Vector3(skill.BombRange.x, skill.BombRange.y));
        }
    }
}
