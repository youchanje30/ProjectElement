using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private DontDestroy instance = null;
    private int check = 0;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance == null)
        {
            Destroy(gameObject);
        }

        check++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
