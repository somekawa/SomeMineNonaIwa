using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SlenderSpawner : MonoBehaviour
{
    public GameObject slender;                  // 生成するオブジェクト
    public GameObject[] spawnSlender;           // 生成しているSlender格納用配列
    public SlenderManCtl[] slenderManCtl;
    public GameObject warpPoints;               // ワープ予定地の親オブジェクト格納用
    public bool instantiateFlag;                // 生成するか否かのフラグ(デフォルト：生成しない＝false)

    private GameObject[] warpPoint_;            // ワープ先のオブジェクト群
    private int minCnt_;

    // Start is called before the first frame update
    void Start()
    {
        warpPoint_ = warpPoints.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        instantiateFlag = false;
        spawnSlender = new GameObject[4];
        slenderManCtl = new SlenderManCtl[4];
        minCnt_ = 0;
        spawnSlender[0] = Instantiate(slender, warpPoint_[Random.Range(1, warpPoint_.Length)].transform.position, new Quaternion(0f, 180f, 0f, 0f));
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
        if (instantiateFlag == true)
        {
            for (int i = 0; i < spawnSlender.Length; i++)
            {
                if (spawnSlender[i] == null)
                {
                    spawnSlender[i] = Instantiate(slender, warpPoint_[Random.Range(1, warpPoint_.Length)].transform.position, new Quaternion(0f, 180f, 0f, 0f));
                    slenderManCtl[i] = spawnSlender[i].GetComponent<SlenderManCtl>();
                    instantiateFlag = false;
                    break;
                }
            }
        }
    }

    public void ClosestObject(GameObject destination,int distance, bool ringingFlag, bool warpFlag)
    {
        float minDistance = 0;
        float nowDistance = 0;
        int minCnt = 0;

        for (int x = 0; x < spawnSlender.Length; x++)
        {
            if (spawnSlender[x] != null)
            {
                nowDistance = Vector3.Distance(gameObject.transform.position, spawnSlender[x].transform.position);      // 全ての敵の距離を測る
                if (minDistance >= nowDistance)
                {
                    // 一番近い敵の情報を格納
                    minDistance = nowDistance;
                    minCnt = x;
                }
            }

            if (slenderManCtl[x] != null)
            {
                slenderManCtl[minCnt].soundPoint.x = destination.gameObject.transform.position.x;
                slenderManCtl[minCnt].soundPoint.z = destination.gameObject.transform.position.z;
                slenderManCtl[minCnt].navMeshAgent_.stoppingDistance = 2;
                slenderManCtl[minCnt].listenFlag = true;
                slenderManCtl[minCnt].ringingFlag = ringingFlag;
                slenderManCtl[minCnt].warpFlag = warpFlag;
                minCnt_ = minCnt;
            }
        }
    }

    public int GetMinCnt()
    {
        return minCnt_;
    }
}
