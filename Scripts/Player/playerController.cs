using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class playerController : MonoBehaviour
{
    public Image textBack;
    public GameObject LeanAnnounceText;   // リーン可能範囲内に入ったときにテキストを表示する
    public GameScene gameManager;

    private CharacterController controller_;
    private HideControl hideControl_;

    private string sceneName_;
    private float speed_   = 4.0f;        // デフォルトの移動速度
    private float gravity_ = 9.8f;        // 重力
    private bool walkFlg_  = false;       // 移動中はtrue
    private bool slowWalk_ = false;       // 移動速度が遅くなる場合はtrue
    private bool quickTurnFlg_   = false; // クイックターンを行う際にtrue
    private float quickTurnTime_ = 0.0f;  // クイックターン用のキーが時間中に2度押しされるか計測する
    private Vector3 oldRotation_;         // 1フレーム前のプレイヤー回転度

    private const float rotateSpeed_ = 0.5f;        // 回転速度
    private const float speedMax_    = 4.0f;        // 移動速度の最大値
    private const float countMax_    = 0.5f;        // エフェクト再生時間の最大値
    private const float quickTurnTimeMax_ = 0.1f;   // この時間までに2度押しされたらクイックターンを行う
    private bool turnCheckFlag_ = false;            // チュートリアルでターンができたか確認用 ターンしたらtrue
    // リーン
    private bool leanFlg_ = false;                  // リーン中かどうかを判定する
    private bool keyFlg1_ = false;                  // リーン中に長押ししても、同じ処理を1回以上行わないようにする為に使用1
    private bool keyFlg2_ = false;                  // リーン中に長押ししても、同じ処理を1回以上行わないようにする為に使用2

    private float startAnimTime_ = 0.0f;
    private GameScene gameScene_;

    // リーンの計算式に必要な値をまとめた構造体
    public struct leanSt
    {
        public float rotate;    // 回転角度
        public float moveX;     // 移動X軸
        public float moveZ;     // 移動Z軸
    }

    IDictionary<string, leanSt> leanMap_;   // ボックスとの接触時に使用される値をまとめている(string->タグ名,leanSt->構造体)

    //private bool clearFlag=false;

    void Start()
    {
       controller_ = GetComponent<CharacterController>();
       hideControl_ = GetComponent<HideControl>();
       gameScene_ = FindObjectOfType<GameScene>();

       // 初期化
       leanSt[] lean = {
            new leanSt { rotate = 1.0f , moveX = -1.0f, moveZ = 0.0f },// 黄色ボックス
            new leanSt { rotate = -1.0f, moveX = 1.0f , moveZ = 0.0f },// オレンジボックス
            new leanSt { rotate = -1.0f, moveX = 0.0f , moveZ = -1.0f},// 赤ボックス
            new leanSt { rotate = 1.0f , moveX = 0.0f , moveZ = 1.0f },// 青ボックス
            new leanSt { rotate = -1.0f, moveX = 0.0f , moveZ = 1.0f },// ピンクボックス(赤の逆)
            new leanSt { rotate = 1.0f , moveX = 0.0f , moveZ = -1.0f},// 緑ボックス(青の逆)
            new leanSt { rotate = 1.0f , moveX = 1.0f , moveZ = 0.0f },// 紫ボックス(黄の逆)
            new leanSt { rotate = -1.0f, moveX = -1.0f, moveZ = 0.0f } // 水色ボックス(オレンジの逆)
        };

        //マップの定義
        leanMap_ = new Dictionary<string, leanSt>
        {
            //マップに値の追加
            { "LeanX_M", lean[0] },
            { "LeanX_P", lean[1] },
            { "LeanZ_P", lean[2] },
            { "LeanZ_M", lean[3] },
            { "LeanZ_P_R", lean[4] },
            { "LeanZ_M_R", lean[5] },
            { "LeanX_M_R", lean[6] },
            { "LeanX_P_R", lean[7] }
        };


        textBack.enabled = false;
        LeanAnnounceText.SetActive(false);
        LeanAnnounceText.GetComponent<Text>().text = "【T】通路を覗き込む";

        sceneName_ = SceneManager.GetActiveScene().name;
   }

    void Update()
    {
        // スタート時のアニメーション中はキャラクター操作ができないようにする
        if (gameScene_ != null && sceneName_ != "TutorialScene")
        {
            if (gameScene_.GetStartAnimTime() < 7.0f)
            {
                //startAnimTime_ += Time.deltaTime;
                return;
            }
        }

        // デバッグ中
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (SceneManager.GetActiveScene().name == "TutorialScene")
            {
                SceneManager.LoadScene("MainScene");
            }
            else if (SceneManager.GetActiveScene().name == "MainScene")
            {
                SceneManager.LoadScene("ClearScene");
            }
        }

        if ((hideControl_ != null) && (hideControl_.GetHideFlg()))
        {
            // 箱に隠れている
            return;
        }

        // リーン処理(キー押下を離した瞬間に、フラグの状態を更新する)
        if (Input.GetKeyUp(KeyCode.T))
        {
            leanFlg_ = !leanFlg_;
            if (leanFlg_)
            {
                LeanAnnounceText.GetComponent<Text>().text = "【T】視点を戻す";
                keyFlg1_ = false;
            }
            else
            {
                LeanAnnounceText.GetComponent<Text>().text = "【T】通路を覗き込む";
                keyFlg2_ = false;
            }
        }

        PlRotate();

        if (!gameManager.GetPauseFlag())
        {
            QuickTurn();
        }

        slowWalk_ = Input.GetKey(KeyCode.LeftShift) ? true : false;

        if (!leanFlg_)   // 傾き中は移動を止める
        {
            CalculateMove();
        }

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

    public float GetCountMax()
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

    public bool GetNowLean()
    {
        return leanFlg_;
    }

    private void OnTriggerExit()
    {
        textBack.enabled = false;
       LeanAnnounceText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        // マップに含まれるかチェック
        if (!leanMap_.ContainsKey(other.gameObject.tag))
        {
            return;
        }

        LeanAnnounceText.SetActive(true );// falseなら表示(true)
        textBack.enabled = true;

        // キー押下中
        if (Input.GetKey(KeyCode.T))
        {

            // 長押ししても、同じ処理を1回以上行わないようにしている
            if (!leanFlg_ && !keyFlg1_)
            {
                keyFlg1_ = true;
                Camera.main.transform.Rotate(new Vector3(0, 0, transform.eulerAngles.z + (30 * leanMap_[other.gameObject.tag].rotate)));
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (1.0f * leanMap_[other.gameObject.tag].moveX), Camera.main.transform.position.y, Camera.main.transform.position.z + (1.0f * leanMap_[other.gameObject.tag].moveZ));
            }
            else if (leanFlg_ && !keyFlg2_)
            {
                keyFlg2_ = true;
                // OnTriggerEnter側で使用した値を反転させる必要があるから、-1.0fが乗算されている
                Camera.main.transform.Rotate(new Vector3(0, 0, transform.eulerAngles.z + (30 * (leanMap_[other.gameObject.tag].rotate * -1.0f))));
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (1.0f * (leanMap_[other.gameObject.tag].moveX * -1.0f)), Camera.main.transform.position.y, Camera.main.transform.position.z + (1.0f * (leanMap_[other.gameObject.tag].moveZ * -1.0f)));
            }
            else
            {
                // 何も処理を行わない
            }
        }
    }
}