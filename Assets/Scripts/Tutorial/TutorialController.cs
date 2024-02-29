using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] Animator buttonAnim;
    [SerializeField] Animator tutoAnim;

    [SerializeField] float tutorialWaitTime;

    [SerializeField] bool isAddtionButton;
    [SerializeField] Animator additionButtonAnim;
    [SerializeField] float additionButtonWaitTime;
    


    void Start()
    {
        Invoke("Tutorial", tutorialWaitTime);
    }
    
    void Tutorial()
    {
        buttonAnim.SetTrigger("Pressed");
        tutoAnim.SetTrigger("Pressed");
        if(isAddtionButton)
            Invoke("AdditionButton", additionButtonWaitTime);    
        Invoke("Tutorial", tutorialWaitTime);
    }

    void AdditionButton()
    {
        additionButtonAnim.SetTrigger("Pressed");
    }

    
}
