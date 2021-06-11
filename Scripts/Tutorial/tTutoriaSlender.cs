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

    public Vector3 soundPoint;                  // 音のした場所に向かうための座標格納用
    public bool listenFlag = false;             // 音が聞こえたか否かのフラグ(デフォルト：聞こえていない＝false)
    public bool inSightFlag = false;            // 視界内に入ったか否かのフラグ(デフォルト：入っていない＝false)


    private Vector3 targetPos;
    private Vector3 test;

    void Start()
    {
        targetPos = new Vector3(-7, 0, 25); 

        anim_ = this.gameObject.GetComponent<Animator>();                  // Animatorの取得
        navMeshAgent_ = this.gameObject.GetComponent<NavMeshAgent>();      // Nav Mesh Agentの取得
        soundPoint = new Vector3(0, 0, 0);
        SetTargetPoint();                                                  // 移動先の決定
    }

    void Update()
    {
        if (navMeshAgent_.velocity.magnitude > 0)     // 移動しているかどうかの判定
        {
            anim_.SetBool("moveFlag", true);          // 歩くモーションの開始
            if (listenFlag == true)                   // 音が聞こえた時に呼び出す
            {
                SetTargetPoint();
            }
        }
        else
        {
            Debug.Log("ターゲットポイントに移動");
            anim_.SetBool("moveFlag", false);        // 歩くモーションの停止
            SetTargetPoint();                        // 次の移動先の決定
        }

    }

    private void SetTargetPoint()
    {
        if (listenFlag == true)
        {
            // ビンを投げたときの移動先
            navMeshAgent_.SetDestination(soundPoint);
        }
        else
        {
            // プレイヤーが隠れた際の移動先
            navMeshAgent_.SetDestination(targetPos);

        }
    }
}
