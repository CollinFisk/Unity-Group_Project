using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
    private PlayerController player;
    private CinemachineCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin ShakeController;
    public float shakeTimer;
    public float intensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cinemachineCamera = GetComponent<CinemachineCamera>(); 
        ShakeController = cinemachineCamera.transform.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StartShakeCamera()
    {
        ShakeController.AmplitudeGain = intensity;
        Invoke(nameof(StopShakeCamera), shakeTimer);

    }
    public void StopShakeCamera()
    {
        ShakeController.AmplitudeGain = 0;
    }

    // Update is called once per frame
    void Update()
    {
     if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                shakeTimer = 0f;
            }
        }
    }

}
