using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{

    [SerializeField] float scrollSpeed;
    [SerializeField] Transform scroll;
    
    [SerializeField] float endY;
    [SerializeField] bool isEnd;


    
    void Update()
    {
        if(isEnd) return;
        
        scroll.position += Vector3.up * scrollSpeed * Time.deltaTime;

        if(scroll.position.y > endY)
        {
            isEnd = true;
            Invoke("LoadIntro", 2f);
        }
    }

    void LoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }
}
