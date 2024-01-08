using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonsterBase
{

    [Header("Rabbit Status")]
    public float[] jumpX;
    public float jumpY;
    public float jumpForce;
    public float minAtkCoolTime;
    public float maxAtkCoolTime;


    protected override void Awake()
    {
        Init();
    }

    protected override void Update()
    {
        CheckState();
        TimeProcess();
        
        if(canAtk && !isAtking)
        {
            Atk();
            curAtkCoolTime = Random.Range(minAtkCoolTime, maxAtkCoolTime);
        }
    }


    protected override void Init()
    {
        base.Init();
        curAtkCoolTime = Random.Range(minAtkCoolTime, maxAtkCoolTime);
    }

    

    protected override void CheckState()
    {
        if(!IsOnGround())
            animator.SetBool("isGround", false);
        
        if(rigid.velocity.y < 0 && IsOnGround())
        {
            animator.SetBool("isGround", true);
            canAtk = false;
            isAtking = false;
        }

        if(curAtkCoolTime <= 0 && !isAtking)
        {
            canAtk = true;
        }
    }

    protected override void TimeProcess()
    {
        if(!isAtking)
            curAtkCoolTime -= Time.deltaTime;
    }

    protected override void Atk()
    {
        isAtking = true;
        animator.SetBool("isGround", false);
        animator.SetTrigger("Atk");

        float x = jumpX[Random.Range(0, jumpX.Length)];
        Vector2 jumpVec = new Vector2(x, jumpY).normalized;

        if(x > 0)
        {
            transform.localScale = new Vector3(monsterData.imageScale, monsterData.imageScale, 1);
        }
        else
        {
            transform.localScale = new Vector3(-monsterData.imageScale, monsterData.imageScale, 1);
        }

        rigid.AddForce(jumpVec * jumpForce, ForceMode2D.Impulse);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(canAtk && other.CompareTag("Player"))
        {
            canAtk = false;
            other.GetComponent<Battle>().GetDamaged(damage);
        }
    }
}
