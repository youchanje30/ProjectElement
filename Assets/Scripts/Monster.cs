using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MonsterTypes
{
    melee,
    range
}

public enum MonsterGrades
{
    common,
    elite,
    boss
}

public class Monster : MonoBehaviour
{
    public float maxHp;
    public float curHp;

    public MonsterTypes monsterType;
    public MonsterGrades monsterGrade;
    private Animator animator;
    private CircleCollider2D circleCollider2D;
    // private BoxCollider2D boxCollider2D;
    
    [SerializeField] private Transform targetPos;
    public float moveSpeed;
    public bool isTargetIn = false;
    private Rigidbody2D rigid2D;
    private int nextDir;

    private void Awake()
    {
        targetPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        curHp = maxHp;
        rigid2D = GetComponent<Rigidbody2D>();
        // circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D = GetComponentInChildren<CircleCollider2D>();
        animator = GetComponent<Animator>();
        Invoke("RandomAct", 1);
    }

    
    void Update()
    {
        if(isTargetIn)
        {
            moveTarget();
        }
        else
        {
            moveRandom();
        }
    }

    public void moveRandom()
    {
        rigid2D.velocity = new Vector2(nextDir * moveSpeed, rigid2D.velocity.y);

        Vector2 frontVec = new Vector2(rigid2D.position.x + nextDir, rigid2D.position.y);
        
        Debug.DrawRay(frontVec, Vector3.down, new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1 ,LayerMask.GetMask("Platform"));
        
        if(raycast.collider == null)
        {
            nextDir = nextDir * -1;
            CancelInvoke();
            Invoke("RandomAct", 5);
        }
    }

    public void moveTarget()
    {
        if(!isTargetIn) return;


        Vector3 moveDir = Vector3.zero;
        


        if(targetPos.position.x > transform.position.x)
        {
            // moveDir = Vector3.right;
            rigid2D.velocity = new Vector2(1 * moveSpeed, rigid2D.velocity.y);
            transform.localScale = new Vector3(-1, 1 ,1);

        }
        else
        {
            // moveDir = Vector3.left;
            rigid2D.velocity = new Vector2(-1 * moveSpeed, rigid2D.velocity.y);
            transform.localScale = new Vector3(1, 1 ,1);
        }

        
        // transform.position += moveDir * Time.deltaTime * moveSpeed;

    }



    void RandomAct()
    {
        nextDir = Random.Range(-1, 1 + 1);
        Invoke("RandomAct", Random.Range(1f , 3f));
    }






    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            targetPos = other.GetComponent<Transform>();
            
        Debug.Log("In Start");
        }
    }


    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            isTargetIn = true;
            Debug.Log("In Stay");
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            isTargetIn = false;
            // targetPos = null;
            Debug.Log("In End");
        }
    }


    public void GetDamaged(float Damage)
    {
        Debug.Log("Get Damage");
        curHp -= Damage;
        if(curHp <= 0)
        {
            Destroy(gameObject);
            // Dead();
        }
    }

    public IEnumerator MonsterAtk()
    {

        yield return new WaitForSeconds(1f);

    }

}