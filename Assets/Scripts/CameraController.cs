using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform Target;
    public Vector3 Offset;
    public Transform cameraHandler;


    public float smoothness = 3f;

    void Awake()
    {
        instance = this;
    }
    
    void Update()
    {
        //Vector3 velocity = Vector3.zero;

        //cameraHandler.position = Vector3.SmoothDamp(cameraHandler.position, Target.position + Offset, ref velocity, Time.deltaTime * smoothness);
        //cameraHandler.position = Vector3.Lerp(cameraHandler.position, Target.position + Offset , Time.deltaTime * smoothness);
         
    }

    void FixedUpdate()
    {
        cameraHandler.position = Vector3.Lerp(cameraHandler.position, Target.position + Offset , Time.fixedDeltaTime * smoothness);
    }



    public IEnumerator Shake(float duration = 1f , float magnitude = 1f)
    {
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
        transform.position = originalPos;

    }
}
