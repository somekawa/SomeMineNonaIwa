using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SlenderSpawner : MonoBehaviour
{
    public GameObject slender;
    public bool instantiateFlag;
    public GameObject[] spawnSlender;
    public GameObject warpPoints;               // ワープ予定地の親オブジェクト格納用
    public GameObject[] warpPoints_;            // ワープ先のオブジェクト群

    // Start is called before the first frame update
    void Start()
    {
        warpPoints = GameObject.Find("WarpPoints");
        warpPoints_ = warpPoints.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        instantiateFlag = false;
        spawnSlender = new GameObject[4];
        spawnSlender[0] = Instantiate(slender, warpPoints_[Random.Range(1, warpPoints_.Length)].transform.position, new Quaternion(0f, 180f, 0f, 0f));
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
                    spawnSlender[i] = Instantiate(slender, warpPoints_[Random.Range(1, warpPoints_.Length)].transform.position, new Quaternion(0f, 180f, 0f, 0f));
                    instantiateFlag = false;
                    break;
                }
            }
        }
    }
}
