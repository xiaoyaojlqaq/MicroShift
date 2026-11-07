using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    [HideInInspector]public CinemachineImpulseSource source;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<CinemachineImpulseSource>(); 
    }

    public void Shake(Vector3 force)
    {
        source.GenerateImpulse(force);
    }
}
