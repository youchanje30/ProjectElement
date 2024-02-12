using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] LayerMask layer;
    public Vector2 size;

    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * 5f, new Color(0,1,0));    
    }

    void OnEnable()
    {
        
        float err = size.x / GetComponent<SpriteRenderer>().bounds.size.x;
        transform.localScale *= err;

        Vector3 pos = transform.position;
        RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.down, 5f, layer);
        if(!ray) return;
        
        pos.y -= ray.distance;

        transform.position = pos;
    }

    public void EffectEnd()
    {
        gameObject.SetActive(false);
    }
}
