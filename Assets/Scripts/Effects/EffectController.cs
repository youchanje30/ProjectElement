using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] LayerMask layer;

    [Header("바닥 붙는 여부")] public bool isGroundFix;
    [Header("크기 보정 여부")] public bool isSizeFix;
    [Header("위치 오차 여부")] public bool isPositionErrorFix;

    [Header("위치 오차 정도")] [SerializeField] float fixForce;

    public Vector2 size;

    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * 5f, new Color(0,1,0));    
    }

    void OnEnable()
    {
        if(isPositionErrorFix) 
        {
            Vector2 vec = (Vector2)transform.position;
            vec.x += Random.Range(-1f, 1f) * fixForce;
            vec.y += Random.Range(-1f, 1f) * fixForce;

            transform.position = vec;
        }

        if(isSizeFix)
        {
            float sizeFix = size.x / GetComponent<SpriteRenderer>().bounds.size.x;
            transform.localScale *= sizeFix;
        }

        if(isGroundFix)
        {
            Vector3 pos = transform.position;
            RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.down, 5f, layer);
            if(!ray) return;
            
            pos.y -= ray.distance;

            transform.position = pos;
        }
    }

    public void EffectEnd()
    {
        gameObject.SetActive(false);
    }
}
