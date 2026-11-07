using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestoryOB : MonoBehaviour
{
    public static DontDestoryOB instance;
    public GameObject[] dontDestoryOBs;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (GameObject obj in dontDestoryOBs)
            {
                DontDestroyOnLoad(obj);
            }
        }
        else
        {
            foreach (GameObject obj in dontDestoryOBs)
            {
                Destroy(obj);
            }
            Destroy(gameObject);
        }
    }

    public void DestoryAll()
    {
        foreach (GameObject obj in dontDestoryOBs)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }
}
