using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlenderSpawner : MonoBehaviour
{
    public GameObject slender;
    public bool instantiateFlag;
    public GameObject[] spawnSlender;

    // Start is called before the first frame update
    void Start()
    {
        instantiateFlag = false;
        spawnSlender = new GameObject[4];
        spawnSlender[0] = Instantiate(slender, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0f, 180f, 0f, 0f));
    }

    static SlenderSpawner Instance = null;

    public static SlenderSpawner GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<SlenderSpawner>();
        }
        return Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (instantiateFlag==true)
        {
            for (int i = 0; i < spawnSlender.Length; i++)
            {
                if (spawnSlender[i] == null)
                {
                    spawnSlender[i] = Instantiate(slender, new Vector3(0.0f, 0.0f, 0.0f), new Quaternion(0f, 180f, 0f, 0f));
                    instantiateFlag = false;
                    break;
                }
            }
        }
    }
}
