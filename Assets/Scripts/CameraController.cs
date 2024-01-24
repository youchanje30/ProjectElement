using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;
    public Transform Target;
    public Vector3 Offset;
    public Transform cameraHandler;

    public delegate void cameraDelegate(float x);
    public cameraDelegate cameraTranslate;
    
    public float smoothness = 3f;

    private float oldPosition;    

    void Awake()
    {
        instance = this;

        if(!Target)
            Target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    
    void Update()
    {
        // cameraHandler.position = Vector3.Lerp(cameraHandler.position, Target.position + Offset , Time.fixedDeltaTime * smoothness);

    }

    void FixedUpdate()
    {
        
        if (cameraHandler.position.x != oldPosition)
        {
            if (cameraTranslate != null)
            {
                float x = oldPosition - cameraHandler.position.x;
                cameraTranslate(x);
            }
 
            oldPosition = cameraHandler.position.x;
        }
        cameraHandler.position = Vector3.Lerp(cameraHandler.position, Target.position + Offset , Time.fixedDeltaTime * smoothness);
    }

    


    public IEnumerator Shake(float duration = 0.1f , float magnitude = 0.2f)
    {
        // if(GameManager.instance.CanCameraShake)
        // {
            Vector3 originalPos = transform.position;
        
            float elasped = 0f;

            while(elasped < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;
                
                transform.localPosition = new Vector3(x, y, originalPos.z);

                elasped += Time.deltaTime;

                yield return null;

            }
            // transform.position = originalPos;
            transform.localPosition = new Vector3(0,0,0);
        // }
    }
}
