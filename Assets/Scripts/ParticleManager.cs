using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [SerializeField] ParticleSystem particleObj;
    private List<ParticleSystem> particlePool = new List<ParticleSystem>();

    [SerializeField] Vector2 spawnPosition;
    [SerializeField] int moveDirection;


    void Awake()
    {
        if(!instance)    
            instance = this;
        else
            Destroy(gameObject);
    }
    
    void Update()
    {
        
    }

    public void SpawnParticle(Vector2 spawnPos, float moveDir)
    {
        ParticleSystem particle = GetParticle();

        if(moveDir > 0)
        {
            particle.transform.rotation = Quaternion.Euler(-90, 90, -90);
        }
        else
        {
            particle.transform.rotation = Quaternion.Euler(130, 90, -90);
        }
        particle.transform.position = spawnPos;
        particle.Play();
        
    }


    public ParticleSystem GetParticle()
    {
        ParticleSystem particle = null;

        for (int i = 0; i < particlePool.Count; i++)
        {
            if(!particlePool[i].isPlaying)
            {
                particle = particlePool[i];
                break;
            }
        }

        if(!particle)
        {
            particle = Instantiate(particleObj, transform);
            particlePool.Add(particle);
        }

        return particle;

    }
}
