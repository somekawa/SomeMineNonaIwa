using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlenderManCtl : MonoBehaviour
{
    // Component取得用変数
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    public GameObject[] targetObjects;        // 移動予定地のオブジェクト
    public GameObject[] warpPoints;           // ワープ先のオブジェクト
    public Vector3 soundPoint;                // 音のした場所に向かうための座標格納用
    public bool listenFlag = false;           // 音が聞こえたか否かのフラグ(デフォルト：聞こえていない＝false)

    private int targetRange;                  // 移動予定地の乱数格納用
    private int warpRange;
    private float warpCnt;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();                  // Animatorの取得
        navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();      // Nav Mesh Agentの取得
        soundPoint = new Vector3(0, 0, 0);
        SetTargetPoint();       // 移動先の決定
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent.velocity.magnitude > 0)     // 移動しているかどうかの判定
        {
            anim.SetBool("moveFlag", true);          // 歩くモーションの開始
            if (listenFlag == true)                  // 音が聞こえた時に呼び出す
            {
                SetTargetPoint();
            }
        }
        else
        {
            anim.SetBool("moveFlag", false);        // 歩くモーションの停止
            SetTargetPoint();                       // 次の移動先の決定
            listenFlag = false;                     // 音のした場所に着いたらfalseにする
        }
        warpCnt += Time.deltaTime;
        if(warpCnt>30)
        {
            SetWarpPoint();
            warpCnt = 0;
        }
        
    }
    
    private void SetTargetPoint()
    {
        targetRange = Random.Range(0, targetObjects.Length);                               // 決まった移動先の乱数を格納
        navMeshAgent.SetDestination(targetObjects[targetRange].transform.position);        // 移動先の設定
        if (listenFlag == true)
        {
            navMeshAgent.SetDestination(soundPoint);                                       // 音がした場所に設定
        }
    }

    private void SetWarpPoint()
    {
        navMeshAgent.ResetPath();
        warpRange = Random.Range(0, warpPoints.Length);                     // 決まった移動先の乱数を格納
        navMeshAgent.Warp(warpPoints[warpRange].transform.position);        // 移動先の設定
        targetRange = Random.Range(0, targetObjects.Length);
        navMeshAgent.SetDestination(targetObjects[targetRange].transform.position);
    }
}
