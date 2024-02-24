using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class Spirit : MonoBehaviour
{
    [Header("필요 참조자")]
    [SerializeField] Battle                      battle;
    [SerializeField] Animator                    animator;
    [SerializeField] RuntimeAnimatorController[] spiritAnimator;
    [Space(20f)]


    [Header("추적 관련 정보")]
    [SerializeField] Transform                  target;
    [SerializeField] float                      nearDistance;
    float                                       imageScale;
    
    private Queue<Vector3>                      targetPositions = new Queue<Vector3>();
    [SerializeField] int                        lineNum;
    float                                       curSpeed;
    [SerializeField] float                      maxSpeed;

    void Awake()
    {
        animator = GetComponent<Animator>();
        
        battle = GameObject.FindGameObjectWithTag("Player").GetComponent<Battle>();
        target = GameObject.FindGameObjectWithTag("SpiritPos").transform;


        imageScale = Mathf.Abs(transform.localScale.x);
    }

    void Start()
    {
        ChangeAnim();
        AddTargetPos();
        SetSpeed();
    }

    
    void Update()
    {
        LookAtTarget();
        AddTargetPos();
    }

    void AddTargetPos()
    {
        targetPositions.Enqueue(target.position);

        if(targetPositions.Count > lineNum)
            MoveToVec(targetPositions.Dequeue());
    }

    bool IsInArea(ref float distance)
    {
        return Vector2.Distance(transform.position, target.position) <= distance;
    }

    void MoveToVec(Vector3 targetVec)
    {
        if(targetVec == transform.position) return;

        if (targetVec.y > target.position.y)
            targetVec.y = target.position.y;

        if (IsInArea(ref nearDistance))
        {
            if(Vector2.Distance(transform.position, target.position) > 0.5f)
                transform.position = Vector3.Lerp(transform.position, targetVec, Time.deltaTime * curSpeed);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, targetVec, Time.deltaTime * Vector2.Distance(transform.position, target.position) * 2f);
    }

    void SetSpeed()
    {
        DOTween.To(() => curSpeed, x => curSpeed = x, (Vector2.Distance(transform.position, target.position) / nearDistance) * maxSpeed, 0.1f).OnComplete(() => SetSpeed());
    }

    void LookAtTarget()
    {
        transform.localScale = new Vector3(imageScale * transform.position.x < target.position.x ? -1 : 1, imageScale, 1);
    }

    public void ChangeAnim()
    {
        animator.runtimeAnimatorController = spiritAnimator[(int)battle.WeaponType];
    }
}
