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
    ActiveSkill skill;
    public float Damage;
    public float moveSpeed;
    public Transform target;

    public float duration;
    public float tick;
    public float per;
    [Tooltip("관통 후 데미지 감소율 %")]
    public float DeclineRate;

    void Update()
    {
        Move();
    }

    void Move()
    {
        switch (Projectile)
        {
            case Type.Arrow:
                transform.position += new Vector3(transform.localScale.x, 0, 0) * Time.deltaTime * moveSpeed;
                break;

            case Type.Magic:
                if (target != null)
                    transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
                else
                    transform.position += new Vector3(transform.localScale.x, 0, 0) * Time.deltaTime * moveSpeed;
                break;

            case Type.WindSkill:
                transform.position += new Vector3(transform.localScale.x, 0, 0) * Time.deltaTime * moveSpeed;
                break;
            case Type.Bomb:
                StartCoroutine(BombAtk());
                break;
        }
    }

    private void Awake()
    {
        Collider2D = GetComponent<CapsuleCollider2D>();
        skill = GameObject.FindGameObjectWithTag("Player").GetComponent<ActiveSkill>();
        battle = skill.GetComponent<Battle>();

        if (Projectile != Type.FireSkill && Projectile != Type.WaterSkill)
        {
            Invoke("Remove", 5f);
        }
        if(Projectile == Type.FireSkill)
        {
            Invoke("Remove",skill.ActiveTime );
        }
        if(Projectile == Type.WaterSkill )
        {
            Invoke("Remove", skill.activeTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.tag);
        if (other.tag == "Floor")
        {
            if (Projectile != Type.WindSkill && Projectile != Type.WaterSkill && Projectile != Type.FireSkill)
            {
                Destroy(gameObject);
            }
        }
        else if (other.tag == "Monster" && Projectile != Type.Bomb)
        {
            // other.GetComponent<Monster>().GetDamaged(Damage);
                other.GetComponentInParent<MonsterBase>().GetDamaged(Damage);
            if (Projectile == Type.Magic)
            {
                other.GetComponentInParent<MonsterDebuffBase>().ContinueBuff(0f, duration, tick, BuffTypes.Slow, per);
                battle.PlayerSynergy(other.gameObject);
            }
            if (Projectile == Type.Arrow)
            {
                battle.PlayerSynergy(other.gameObject);
            }
            if (Projectile != Type.WindSkill && Projectile != Type.WaterSkill && Projectile != Type.FireSkill)
            {
                Destroy(gameObject);
            }
            if (Projectile == Type.WaterSkill)
            {
                skill.passive.ActivePassive(WeaponTypes.Wand, other.GetComponentInParent<MonsterDebuffBase>());
                //Damage *= 1 - (DeclineRate / 100);
            }    
        }
        else if (other.tag == "Destruct"  )
        {
            other.GetComponent<DestructObject>().DestroyObj();
            if (Projectile != Type.WindSkill && Projectile != Type.WaterSkill && Projectile != Type.Bomb)
            {
                Destroy(gameObject);
            }
        }
        if (Projectile == Type.WaterSkill)
        {
            if (other.tag == "TileMap" || other.tag == "OneWayPlatForm")
            {
                WaterY(skill.WaterY);
                //if (other.tag == "Monster")
                //{
                //   // other.GetComponentInParent<MonsterBase>().GetDamaged(Damage);
                //}
                //Remove();


            }

        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Monster") 
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
       
        yield return new WaitForSeconds(skill.BombChargeTime);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, skill.BombRange, 0);
        {
            Debug.Log("1");
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Monster")
                {
                    skill.passive.ActivePassive(WeaponTypes.Wand, collider.GetComponentInParent<MonsterDebuffBase>());
                    skill.SkillAtk(collider.gameObject, skill.DefaultDamage *= 1 + (skill.BombDamageIncreaseRate / 100));              
                }
                if (collider.tag == "Destruct")
                {
                    skill.SkillAtk(collider.gameObject, skill.DefaultDamage *= 1 + (skill.BombDamageIncreaseRate / 100));
                }
            }
        }
        Remove();
    }
    public void WaterY(float y)
    {
        if(transform.position.y <= y)
        {
            transform.GetComponent<ProjectileType>().Projectile = Type.Bomb;
            transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            transform.GetComponent<Rigidbody2D>().position += new Vector2(0,0.5f);
            transform.GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    if(Projectile == Type.FireSkill)
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(new Vector3(transform.position.x + 0.6f, transform.position.y),new Vector3(transform.position.x + 1.1f, transform.position.y));
    //        Gizmos.DrawLine(new Vector3(transform.position.x - 0.6f, transform.position.y), new Vector3(transform.position.x - 1.1f, transform.position.y));
    //    }
    //}
}
