using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSpaceShiftShaderControl : MonoBehaviour
{
    public static TimeSpaceShiftShaderControl instance;
    public bool isPlay;
    public float maxShaderProcess;
    public float playSpeed;
    bool isIncrease;
    Material material;
    float shaderProcess;
    public RawImage rawImage;

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
                rawImage.DOColor(Color.black, 2).OnComplete(() =>
                {
                    rawImage.DOColor(Color.white, 5);
                });
            }
            else if (shaderProcess < 0)
            {
                shaderProcess = 0;
                isPlay = false;
                isIncrease = true;
                rawImage.gameObject.SetActive(false);
            }
            if (isIncrease) shaderProcess += Time.deltaTime * playSpeed;
            else shaderProcess -= Time.deltaTime * playSpeed;
            material.SetFloat("_process", shaderProcess);
        }
    }
}
