using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    private CharacterController controller_;
    private HideControl hideControl_;

    private float speed_ = 3.0f;          // デフォルトの移動速度
    private float gravity_ = 9.8f;        // 重力
    private bool walkFlg_  = false;       // 移動中はtrue
    private bool slowWalk_ = false;       // 移動速度が遅くなる場合はtrue
    private bool quickTurnFlg_ = false;   // クイックターンを行う際にtrue
    private float quickTurnTime_ = 0.0f;  // クイックターン用のキーが時間中に2度押しされるか計測する
    private Vector3 oldRotation_;         // 1フレーム前のプレイヤー回転度

    private const float rotateSpeed_ = 0.5f;        // 回転速度
    private const float speedMax_ = 3.0f;           // 移動速度の最大値
    private const int   countMax_ = 120;            // エフェクト再生時間の最大値
    private const float quickTurnTimeMax_ = 0.1f;   // この時間までに2度押しされたらクイックターンを行う
    private bool turnCheckFlag_ = false;  // チュートリアルでターンができたか確認用 ターンしたらtrue

    // リーン
    private bool eD;
    private bool eU;
    private bool eG;
    private bool qD;
    private bool qU;
    private bool qG;
    private int lean = 0;
    private int leanOld = 0;
    private int cntR = 0;
    private int cntL = 0;

    // PlayerCameraControllに覗き込んでいる際中のフラグを送って、Y座標のみ移動可能にする

    //private bool clearFlag=false;
    void Start()
    {
       controller_ = GetComponent<CharacterController>();
       hideControl_ = GetComponent<HideControl>();
    }

    void Update()
    {
        eD = Input.GetKeyDown(KeyCode.T);
        eU = Input.GetKeyUp(KeyCode.T);
        eG = Input.GetKey(KeyCode.T);
        qD = Input.GetKeyDown(KeyCode.R);
        qU = Input.GetKeyUp(KeyCode.R);
        qG = Input.GetKey(KeyCode.R);

        // 何もしていない
        if (lean == 0)
        {
            if (cntR >= 0)
            {
                cntR--;
                Camera.main.transform.Rotate(new Vector3(0, 0, transform.eulerAngles.z + 1));
            }
            else if (cntL >= 0)
            {
                cntL--;
                Camera.main.transform.Rotate(new Vector3(0, 0, transform.eulerAngles.z - 1));
            }
            else
            {
                Debug.Log("lean平行に戻る");
            }

            if (leanOld != lean)
            {
                if(leanOld == 3)
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - 1.0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                }
                else
                {
                    Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + 1.0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                }
            }

            // 右へ
            if (!eU && eD && !(!qU && qD))
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + 1.0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                lean = 3;
            }

            // 左へ
            if (qD && !qU)
            {
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - 1.0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
                lean = 1;
            }

            leanOld = lean;
        }
        else if (lean == 3)
        {
            cntR++;

            if (cntR <= 30)
            {
                // ローカルが毎回newで新しくされてるから-1
                Camera.main.transform.Rotate(new Vector3(0, 0, transform.eulerAngles.z - 1));
            }
            else
            {
                cntR = 30;
                Debug.Log("lean傾け終わり");
            }

            // 押してないときにlean==0に戻さないといけない
            if (eU)
            {
                leanOld = lean;
                lean = 0;
            }
        }
        else if(lean == 1)
        {
            cntL++;

            if (cntL <= 30)
            {
                // ローカルが毎回newで新しくされてるから-1
                Camera.main.transform.Rotate(new Vector3(0, 0, transform.eulerAngles.z + 1));
            }
            else
            {
                cntL = 30;
                Debug.Log("lean傾け終わり");
            }

            // 押してないときにlean==0に戻さないといけない
            if (qU)
            {
                leanOld = lean;
                lean = 0;
            }
        }


        if ((hideControl_ != null) && (hideControl_.GetHideFlg()))
        {
            // 箱に隠れている
            return;
        }

        PlRotate();
        QuickTurn();
        if (Input.GetKey(KeyCode.Return))
        {
            //エンターキー入力
            Debug.Log("エンターキー入力");
            slowWalk_ = true;
        }
        else
        {
            slowWalk_ = false;
        }

        CalculateMove();


        //// クリアしたときの処理
        //// クリア条件：脱出アイテム8つ所持状態で出口に向かう
        //if (Input.GetKey(KeyCode.C))
        //{
        //    clearFlag = true;
        //}
        //if (clearFlag == true)
        //{
        //    SceneManager.LoadScene("ClearScene");
        //}

    }

    void PlRotate()
    {
        // 回転処理
        if (oldRotation_.y < transform.localEulerAngles.y)
        {
            // 前回座標 < 現在座標　→加算
            transform.Rotate(new Vector3(0.0f, rotateSpeed_, 0.0f));
        }
        else if (oldRotation_.y > transform.localEulerAngles.y)
        {
            // 前回座標 > 現在座標　→減算
            transform.Rotate(new Vector3(0.0f, -rotateSpeed_, 0.0f));
        }
        else
        {
            // 処理なし
        }
        oldRotation_ = transform.localEulerAngles;
    }

    void QuickTurn()
    {
        // Sキー連続押しでクイックターン
        // 連続押下時間を過ぎたらフラグと時間を初期値に戻す
        if (quickTurnTime_ > quickTurnTimeMax_)
        {
            quickTurnFlg_  = false;
            quickTurnTime_ = 0.0f;
        }

        // キーの2回目押下までの時間を計測して、時間内に押下されていたら回転処理を行う
        if (quickTurnFlg_ && (quickTurnTime_ <= quickTurnTimeMax_))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                // 180度回転
                transform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));
                quickTurnFlg_ = false;
                quickTurnTime_ = 0.0f;
          // チュートリアル用
                turnCheckFlag_ = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            quickTurnFlg_ = true;
        }

        if (quickTurnFlg_)
        {
            quickTurnTime_ += Time.deltaTime;
        }
    }

    void CalculateMove()
    {
        // 基本移動処理
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput   = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        // 移動中かを調べてフラグを切り替える
        if ((direction.x != 0) || (direction.z != 0))
        {
            walkFlg_ = true;    // 移動中
        }
        else
        {
            walkFlg_ = false;   // 立ち止まっている
        }

        if (slowWalk_)
        {
            if (speed_ >= speedMax_)
            {
                speed_ /= 2.0f; // slowがtrueなら速度/2にする
            }
        }
        else
        {
            speed_ = speedMax_;
        }

        Vector3 velocity = direction * speed_;
        velocity.y -= gravity_;
        velocity = transform.transform.TransformDirection(velocity);
        controller_.Move(velocity * Time.deltaTime);

    }

    public bool GetWalkFlg()
    {
        return walkFlg_;
    }

    public bool GetSlowWalkFlg()
    {
        return slowWalk_;
    }

    public bool GetTurnCheckFlag()
    {
        return turnCheckFlag_;
    }

    public int GetCountMax()
    {
        // 移動速度変更フラグを見て、エフェクトの再生時間を調整する
        if (!slowWalk_)
        {
            return countMax_;
        }
        else
        {
            return countMax_ * 2;
        }

        Debug.Log("GetCountMaxでエラー");
        return 0;
    }

    public int GetNowLean()
    {
        return lean;
    }
}