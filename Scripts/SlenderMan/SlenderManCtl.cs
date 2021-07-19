using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class SlenderManCtl : MonoBehaviour
{
    // Component取得用変数
    public NavMeshAgent navMeshAgent_;          // Nav Mesh Agentの格納用
    private Animator anim_;                     // Animatorの格納用

    public GameObject destinationPoints;        // 移動予定地の親オブジェクト格納用
    public GameObject warpPoints;               // ワープ予定地の親オブジェクト格納用
    public Vector3 soundPoint;                  // 音のした場所に向かうための座標格納用
    public bool listenFlag = false;             // 音が聞こえたか否かのフラグ(デフォルト：聞こえていない＝false)
    public bool warpFlag = false;               // 大きなおとが聞こえてワープするか否かのフラグ(デフォルト：聞こえていない＝false)
    public bool inSightFlag = false;            // 視界内に入ったか否かのフラグ(デフォルト：入っていない＝false)

    private GameObject[] targetObjects_;        // 移動予定地のオブジェクト群
    private GameObject[] warpPoints_;           // ワープ先のオブジェクト群
    private int targetRange_;                   // 移動予定地の乱数格納用
    private int warpRange_;                     // ワープ予定地の乱数格納用
    private float warpCnt_;                     // ワープが発生するまでの時間格納用
    private Vector3 soundWarpPoint_;            // 音のした場所近くの座標格納用

    public enum Status
    {
        IDLE,
        WALK,
        NULL
    }
   
    public Status status= Status.NULL;

    Vector2 velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        anim_ = this.gameObject.GetComponent<Animator>();                  // Animatorの取得
        navMeshAgent_ = this.gameObject.GetComponent<NavMeshAgent>();      // Nav Mesh Agentの取得

        destinationPoints = GameObject.Find("DestinationPoints");
        warpPoints = GameObject.Find("WarpPoints");

        navMeshAgent_.autoRepath = false;
        navMeshAgent_.updatePosition = false;

        soundPoint = new Vector3(0, 0, 0);
        soundWarpPoint_ = new Vector3(0, 0, 0);

        targetObjects_ = destinationPoints.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        warpPoints_ = warpPoints.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();

        this.gameObject.transform.position = warpPoints_[Random.Range(1, warpPoints_.Length)].transform.position;

        SetTargetPoint();                                                  // 移動先の決定
    }

    // Update is called once per frame
    void Update()
    {
        //if (navMeshAgent_.pathStatus != NavMeshPathStatus.PathInvalid)
        //{
        //if (navMeshAgent_.velocity.magnitude > 0)     // 移動しているかどうかの判定
        if (status == Status.WALK)
        {
            anim_.SetBool("moveFlag", true);          // 歩くモーションの開始
            if (navMeshAgent_.hasPath == false)
            {
                status = Status.IDLE;
            }
        }
        else if (status == Status.IDLE)
        {
            anim_.SetBool("moveFlag", false);        // 歩くモーションの停止
            SetTargetPoint();                        // 次の移動先の決定
            if (navMeshAgent_.hasPath == true)
            {
                status = Status.WALK;
                listenFlag = false;                      // 音のした場所に着いたらfalseにする
                navMeshAgent_.stoppingDistance = 0;
            }
        }
        else
        {
            anim_.SetBool("moveFlag", false);        // 歩くモーションの停止
        }
        //}

        if (warpFlag == true && listenFlag == true && status != Status.NULL)                   // 音が聞こえた時に呼び出す
        {
            SetWarpPoint();
        }

        if (listenFlag == true && warpFlag == false && status != Status.NULL)
        {
            SetTargetPoint();
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

        Vector3 worldDeltaPosition = navMeshAgent_.nextPosition - transform.position;

        // worldDeltaPosition をローカル空間にマップします
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        velocity = deltaPosition / Time.deltaTime;

        // アニメーションのパラメーターを更新します
        anim_.SetFloat("velx", velocity.x);
        anim_.SetFloat("vely", velocity.y);
    }

    private void SetTargetPoint()
    {
        targetRange_ = Random.Range(1, targetObjects_.Length);                                  // 決まった移動先の乱数を格納
        navMeshAgent_.SetDestination(targetObjects_[targetRange_].transform.position);          // 移動先の設定
        if(listenFlag==true)
        {
            navMeshAgent_.SetDestination(soundPoint);
        }
        status = Status.WALK;
    }

    private void SetWarpPoint()
    {
        navMeshAgent_.ResetPath();
        warpRange_ = Random.Range(1, warpPoints_.Length);                      // 決まった移動先の乱数を格納
        navMeshAgent_.Warp(warpPoints_[warpRange_].transform.position);        // 移動先の設定
        if (warpFlag == true)
        {
            DistanceCalculation();
            navMeshAgent_.Warp(soundWarpPoint_);
        }
        warpFlag = false;
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
                soundWarpPoint_ = warpPoints_[i].transform.position;                          // その時のワープポイントの座標を格納する
            }
        }
    }

    void OnAnimatorMove()
    {
        // 案: アニメーションの移動を正とする
        //Vector3 position = anim_.rootPosition;
        //position.y = navMeshAgent_.nextPosition.y;
        //transform.position = position;
        // position (位置) を agent (エージェント) の位置に更新します
        transform.position = navMeshAgent_.nextPosition;
    }
}
