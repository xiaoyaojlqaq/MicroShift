using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.UI;
public class TimeSpaceShiftShaderControl : MonoBehaviour
{
    public static TimeSpaceShiftShaderControl instance;
    public bool isPlay;
    public float maxShaderProcess;
    public float playSpeed;
    bool isIncrease;
    Material material;
    public float shaderProcess;
    public RawImage rawImage;

    [HideInInspector]public PlayableDirector playable;

    public PlayerControl playerControl;
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
        playable = GetComponent<PlayableDirector>();
        isIncrease = true;
        material = rawImage.material;
    }

    private void Update()
    {
        Show();
    }
    void Show()
    {
        if (isPlay)
        {
            rawImage.gameObject.SetActive(true);
            if (shaderProcess >= maxShaderProcess)
            {
                isIncrease = false;
            }
            else if (shaderProcess < 0)
            {
                shaderProcess = 0;
                isPlay = false;
                rawImage.gameObject.SetActive(false);
                isIncrease = true;
            }
            if (isIncrease) shaderProcess += Time.deltaTime * playSpeed;
            else shaderProcess -= Time.deltaTime * playSpeed;
            material.SetFloat("_process", shaderProcess);
        }
    }

    public void PlayAnima()
    {
        playable.Play();
        isPlay = true;
    }
}
