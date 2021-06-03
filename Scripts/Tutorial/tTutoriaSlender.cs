using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class tTutoriaSlender : MonoBehaviour
{  
    // Component取得用変数
    private NavMeshAgent navMeshAgent_;         // Nav Mesh Agentの格納用
    private Animator anim_;                     // Animatorの格納用

    public GameObject targetObject;             // 移動予定地の親オブジェクト格納用
   public GameObject warpPoint;                // ワープ予定地の親オブジェクト格納用
    public Vector3 soundPoint;                  // 音のした場所に向かうための座標格納用
    public bool listenFlag = false;             // 音が聞こえたか否かのフラグ(デフォルト：聞こえていない＝false)
    public bool inSightFlag = false;            // 視界内に入ったか否かのフラグ(デフォルト：入っていない＝false)

    private GameObject[] targetObjects_;        // 移動予定地のオブジェクト群
    private GameObject[] warpPoints_;           // ワープ先のオブジェクト群
    private int targetRange_;                   // 移動予定地の乱数格納用
    private int warpRange_;                     // ワープ予定地の乱数格納用
    private float warpCnt_;                     // ワープが発生するまでの時間格納用
    private Vector3 soundWarpPoint;             // 音のした場所近くの座標格納用

    private Vector3[] targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = new Vector3[2];
        targetPos[0] = new Vector3(-8, 0, -7);
        targetPos[0] = new Vector3(-8, 0, 7);

        anim_ = this.gameObject.GetComponent<Animator>();                  // Animatorの取得
        navMeshAgent_ = this.gameObject.GetComponent<NavMeshAgent>();      // Nav Mesh Agentの取得
        soundPoint = new Vector3(0, 0, 0);
        soundWarpPoint = new Vector3(0, 0, 0);

        

        targetObjects_ = targetObject.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
      warpPoints_ = warpPoint.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();

        SetTargetPoint();                                                  // 移動先の決定
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent_.velocity.magnitude > 0)     // 移動しているかどうかの判定
        {
            anim_.SetBool("moveFlag", true);          // 歩くモーションの開始
            if (listenFlag == true)                   // 音が聞こえた時に呼び出す
            {
                SetWarpPoint();
            }
        }
        else
        {
            anim_.SetBool("moveFlag", false);        // 歩くモーションの停止
            SetTargetPoint();                        // 次の移動先の決定
            listenFlag = false;                      // 音のした場所に着いたらfalseにする
        }

        if (listenFlag == false)
        {
            warpCnt_ += Time.deltaTime;
        }



        if (warpCnt_ > 30)
        {
            SetWarpPoint();
            warpCnt_ = 0;
        }

        if (inSightFlag == true)
        {
            SetWarpPoint();
            listenFlag = false;
            inSightFlag = false;
        }
        //Debug.Log("navMeshAgent_X" + navMeshAgent_.nextPosition.x +
        //  "       navMeshAgent_" + navMeshAgent_.nextPosition.z );
    }

    private void SetTargetPoint()
    {
        targetRange_ = Random.Range(1, targetObjects_.Length);    //targetPos[2];                              // 決まった移動先の乱数を格納
        navMeshAgent_.SetDestination(targetObjects_[targetRange_].transform.position);          // 移動先の設定
        if (listenFlag == true)
        {
            navMeshAgent_.SetDestination(soundPoint);
        }
    }

    private void SetWarpPoint()
    {
        navMeshAgent_.ResetPath();
        warpRange_ = Random.Range(1, warpPoints_.Length);                      // 決まった移動先の乱数を格納
        navMeshAgent_.Warp(warpPoints_[warpRange_].transform.position);        // 移動先の設定
        if (listenFlag == true)
        {
            DistanceCalculation();
            navMeshAgent_.Warp(soundWarpPoint);
        }
        SetTargetPoint();
    }

    private void DistanceCalculation()
    {
        float maxDistance = 1000;
        float distance = 0;
        for (int i = 0; i < warpPoints_.Length; i++)                                         // ワープポイントの数だけまわす
        {
            distance = Vector3.Distance(soundPoint, warpPoints_[i].transform.position);      // 音のした場所とすべてのワープポイントの距離を測る
            if (maxDistance > distance)                                                      // maxDistanceより値が小さくなった場合
            {
                maxDistance = distance;                                                      // maxDistanceの再設定
                                                                                             // soundWarpPoint = warpPoints_[i].transform.position;                          // その時のワープポイントの座標を格納する
            }
        }
    }
}
