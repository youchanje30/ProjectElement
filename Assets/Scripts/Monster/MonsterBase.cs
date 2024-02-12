using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum DetectTypes { none, box, capsule }
public enum AtkTypes { multi, box }

public class MonsterBase : MonoBehaviour
{
    #region 필요 컴포넌트
    [Header("필요 컴포넌트")]
    public MonsterData monsterData;
    [SerializeField] protected Rigidbody2D rigid;
    public Animator animator;
    [SerializeField] protected DetectPlayer trackDetect;
    [SerializeField] protected DetectPlayer tryAtkDetect;
    [SerializeField] protected Transform target;
    [SerializeField] private MonsterSynergy monsterSynergy;
    #endregion

    #region 몬스터 정보 선언
    // 몬스터 최대 정보는 MonsterData에 존재함

    [Header("몬스터 현재 정보")]
    public float moveSpeed;
    public float curHp;
    public float damage;
    protected float atkCoolTime;
    protected float curAtkCoolTime;
    protected float knockbackTime;

    [Header("몬스터 상태 정보")]
    protected bool isDead;      // 죽는 상태
    protected bool isKnockback; // 넉백 상태
    protected bool isAtking;    // 공격 상태
    protected bool isTracking;  // 추적 상태
    protected bool isMove;      // 이동 상태
    public bool isStun;
    public bool isHit;
    #endregion


    #region 필요 정보들
    protected int nextDir;
    protected bool canMove;
    protected bool canTrack; // 추적 이동 canMove와 구분해서 사용예정
    protected bool canAtk;


    [System.Serializable]
    public struct AtkInfo
    {
        [LabelText("공격 종류")] public string atkName;
        [LabelText("공격 위치")] public Vector3 atkPos;
        [LabelText("공격 범위")] public Vector2 atkSize;
    }
    
    [TitleGroup("공격 기본 설정")]
    [ListDrawerSettings(ShowIndexLabels = true)]
    [SerializeField] protected AtkInfo[] atkInfo;
    #endregion

    /*

    행동 불가능한 상태 : isDead , isAtking , isKnockback
    위 상태는 행동을 끝내기 전까지 아무것도 할 수 없음

    1. 이동 - 지속적으로 이동, 정지를 하며 플레이어를 찾아다님
    2. 추적 - 추적 범위 내에 플레이어가 들어오면 추적을 시작. 해당 플레이어 위치로 이동 시도
    3. 공격 - 공격 범위 내에 플레이어가 들어오면 공격을 시작. 공격이 끝난 후 행동 가능
    
    공격 시도 범위 / 피격 범위
   
    플레이어가 땅에 있는가..
    텔레포트 위치가 땅을 벗어나지 않는가..

    */


    #region 초기화
    protected virtual void Init()
    {
        if(!rigid)
            rigid = GetComponent<Rigidbody2D>();

        if(!animator)
            animator = GetComponent<Animator>();

        if(!trackDetect)
            trackDetect = GetComponentsInChildren<DetectPlayer>()[0];

        if(!tryAtkDetect)
            tryAtkDetect = GetComponentsInChildren<DetectPlayer>()[1];

        if(!target)
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if(!monsterSynergy)
            monsterSynergy = GetComponentInChildren<MonsterSynergy>();

        // 몬스터 정보 초기화
        moveSpeed = monsterData.maxMoveSpeed;
        curHp = monsterData.maxHp;
        damage = monsterData.maxDamage;
        atkCoolTime = monsterData.maxAtkCoolTime;
        curAtkCoolTime = atkCoolTime;
        knockbackTime = monsterData.maxKnockbackTime;
    }

    // 위치 초기화도 필요함 몬스터는 Trigger가 켜져있기 때문에..
    #endregion
    
    protected virtual void Awake()
    {
        Init();
        RandomDir();
        isMove = true;
    }

    protected virtual void Start()
    {
        StageManager.instance.AddMonster(gameObject);
    }

    protected virtual void Update()
    {
        CheckState();
        SetState();

        if(isMove)
            Move();
            
        if(isTracking)
            Tracking();

        TimeProcess();
    }


    #region 몬스터 상태 관리
    protected virtual void CheckState()
    {
        if(isDead || isAtking || isKnockback || isStun)
        {
            canMove = false;
            canAtk = false;
            return;
        }

        canAtk = false;
        canTrack = false;
        canMove = false;

        if(tryAtkDetect.isEnter)
        {
            if(curAtkCoolTime <= 0)
                canAtk = true;
            canMove = false;
        }
        else if(trackDetect.isEnter)
        {
            canTrack = true;
        }
        else
            canMove = true;
    }

    protected virtual void ChangeLocalScale(int value)
    {
        if(value == 0) return;

        transform.localScale = new Vector3(-value * monsterData.imageScale, monsterData.imageScale , 1);
    }


    // 몬스터 상태 설정
    protected virtual void SetState()
    {
        // isAtking = false;
        isTracking = false;
        isMove = false;

        if(isDead || isAtking || isKnockback || isStun)
        {
            canMove = false;
            canAtk = false;
            return;
        }

        if(monsterSynergy.isBind)
        {
            canMove = false;
            return;
        }

        if(canAtk)
        {
            rigid.velocity = Vector2.zero;
            isAtking = true;
            Atk();
            return;
        } 

        if(canTrack)
        {
            isTracking = true;
            return;
        }

        if(canMove)
        {
            isMove = true;
            return;
        }
    }

    // 시간 감소하는 코드.. 패시브 같은 것도 여기서 구현을 할까
    protected virtual void TimeProcess()
    {
        curAtkCoolTime -= Time.deltaTime;

    }

    // 땅 위에 있는지 체크
    protected virtual bool IsOnGround() // 위치값을 받아서 확인할 수 있으면 좋을 것 같음
    {
        Vector2 frontVec = new Vector2(transform.position.x + nextDir * monsterData.floorRayX, transform.position.y - monsterData.floorRayY);

        Debug.DrawRay(frontVec, Vector2.down, new Color(0,1,0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector2.down, 1 , LayerMask.GetMask("Platform"));
        
        return raycast.collider != null;
    }
    #endregion


    #region 몬스터 이동
    // 추적 이동
    protected virtual void Tracking()
    {
        animator.SetBool("isRange", true);
        animator.SetBool("isMove", false);
        // float moveDir = target.position.x - transform.position.x;

        int targetDir = target.position.x > transform.position.x ? 1 : -1;
        rigid.velocity = new Vector2(targetDir * moveSpeed, rigid.velocity.y);
        // transform.position += new Vector3(targetDir, 0, 0) * moveSpeed * Time.deltaTime;
        ChangeLocalScale(targetDir);
        // transform.localScale = new Vector3(-targetDir * monsterData.imageScale, monsterData.imageScale , 1);
        // animator.SetBool("isMove", nextDir != 0);
    }

    // 일반 이동
    protected virtual void Move()
    {
        if(!IsOnGround())
            nextDir *= -1;

        animator.SetBool("isMove", nextDir != 0);
        animator.SetBool("isRange", false);

        // transform.position += new Vector3(nextDir, 0, 0) * moveSpeed * Time.deltaTime;
        rigid.velocity = new Vector2(nextDir * moveSpeed, rigid.velocity.y);
        ChangeLocalScale(nextDir);
        // if(nextDir != 0)
        //     transform.localScale = new Vector3(-nextDir * monsterData.imageScale, monsterData.imageScale , 1);
    }

    protected virtual void RandomDir()
    {
        nextDir = Random.Range(-1, 1 + 1);
        Invoke("RandomDir", Random.Range(2f, 5f));
    }
    #endregion 


    #region 몬스터 공격
    protected virtual void Atk()
    {
        isAtking = true;
        canAtk = false;
        animator.SetTrigger("Atk");
    }

    protected virtual void AtkDetect(int index = 0)
    {
        Vector3 detectPos = transform.position + atkInfo[index].atkPos * (Mathf.Abs(transform.localScale.x) / transform.localScale.x);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(detectPos, atkInfo[index].atkSize, 0, LayerMask.GetMask("Player"));
        foreach(Collider2D collider in collider2Ds)
        {
            Debug.Log(collider.tag);
            if(!collider.CompareTag("Player")) continue;
                
            collider.GetComponent<Battle>().GetDamaged(damage);
            // Debug.Log(atkInfo[index].atkPos);
            // Debug.Log(atkInfo[index].atkSize);

        }
    }

    protected virtual void AtkEnd()
    {   
        curAtkCoolTime = atkCoolTime;
        isAtking = false;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        

        for (int i = 0; i < atkInfo.Length; i++)
        {
            Vector3 detectPos = transform.position + atkInfo[i].atkPos * (Mathf.Abs(transform.localScale.x) / transform.localScale.x);
            Gizmos.DrawWireCube(detectPos, atkInfo[i].atkSize);
        }
        
    }
    #endregion


    #region 몬스터 데미지 받음
    public virtual void GetDamaged(float getDamage, bool canKncokBack = true)
    {
        if(isKnockback || isDead) return;

        curHp -= getDamage;
        if(isAtking) AtkEnd();

        // 체력바 추가 해야 함

        if(curHp <= 0)
        {
            isDead = true;
            animator.SetTrigger("Dead");
            Invoke("Dead", 1f); // 안죽는 경우 대비
            SetDead();
        }

        // animator.ResetTrigger("Hurt");
        animator.SetTrigger("Hurt");

        if(!canKncokBack) return;

        
        float subX = transform.position.x - target.position.x;
        int knockX = 1;

        if(subX < 0)
            knockX = 1;
        else
            knockX = -1;

        isKnockback = true;
        StartCoroutine(Knockback(knockX));
    }

    protected void SetDead()
    {
        gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
        rigid.bodyType = RigidbodyType2D.Static;
    }

    protected IEnumerator Knockback(int dir)
    {
        //sprite.color = new Color(0, 0, 0); 깜빡이는 효과 Dotween 으로 할 예정

        float ctime = 0;
        // transform.localScale = new Vector3(-dir * monsterData.imageScale, monsterData.imageScale , 1);
        ChangeLocalScale(dir);
        
        rigid.velocity = Vector2.left * dir * 10;
        while (ctime < knockbackTime)
        {
            ctime += Time.deltaTime;
            yield return null;
        }
        isKnockback = false;
        rigid.velocity = Vector2.zero;

        //sprite.color = new Color(1, 1, 1);
    }
    #endregion


    #region 몬스터 사망
    protected virtual void Dead()
    {
        CancelInvoke("Dead");
        StageManager.instance.DeadMonster(gameObject);
        Reward();
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    protected virtual void Reward()
    {
        GameObject Coin = ItemDropManager.instance.Coin;
        for (int i = 0; i < ItemDropManager.instance.CoinDrop(); i++)
        {
            GameObject spawnCoin = Instantiate(Coin);
            spawnCoin.transform.position = transform.position;
        }
    }
    #endregion
}