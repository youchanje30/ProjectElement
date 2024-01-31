using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera cinemachineCam;
    public static CameraController instance;
    [SerializeField] float shakeTime;
    
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

        if(!cinemachineCam)
            GetComponent<CinemachineVirtualCamera>();
        
        if(!cinemachineCam.Follow)
            cinemachineCam.Follow = GameObject.FindWithTag("Player").GetComponent<Transform>();
        // if(!basicMultiChannel)
        //     cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    
    void Update()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            if(shakeTime <= 0f)
            {
                CinemachineBasicMultiChannelPerlin basicMultiChannel = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                basicMultiChannel.m_AmplitudeGain = 0f;
            }
        }


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
        // cameraHandler.position = Vector3.Lerp(cameraHandler.position, Target.position + Offset , Time.fixedDeltaTime * smoothness);
    }

    public IEnumerator Shake(float duration = 0.1f , float magnitude = 0.2f)
    {   
        float elasped = 0f;

        while(elasped < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            
            transform.localPosition = new Vector3(x, y, 0);

            elasped += Time.deltaTime;

            yield return null;

        }
        transform.localPosition = new Vector3(0,0,0);
 
    }

    public void ShakeCamera(float time, float intensity)
    {
        CinemachineBasicMultiChannelPerlin basicMultiChannel = cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        basicMultiChannel.m_AmplitudeGain = intensity;
        shakeTime = time;
    }

    //public IEnumerator ShakeR(float duration = 0.1f , float magnitude = 0.2f)
    //{
    //    float curTime = 0f;
    //    while(curTime < duration)
    //    {
    //        float z = Random.Range(-1f, 1f) * magnitude;
            
    //        // transform.rotation = Quaternion.Euler()
    //        transform.rotation = Quaternion.Euler(new Vector3(0, 0, z));

    //        curTime += Time.deltaTime;

    //        yield return null;

    //    }
    //    transform.rotation = Quaternion.Euler(Vector3.zero);
    //}

}
