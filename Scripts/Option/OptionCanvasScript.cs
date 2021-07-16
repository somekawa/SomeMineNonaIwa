using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionCanvasScript : MonoBehaviour
{
    public Button optionButton;                      // オプション画面に行くためのボタン
    public Button leaveButton;                       // オプション画面に行くためのボタン
    public GameObject optionMenu;                    // オプションで表示するオブジェクト
    public TextMeshProUGUI optionText;               // オプション画面と分かるテキスト
    public Image backImage;                          // オプション画面の背景画像

    private GameObject titleCanvas_;
    private GameScene gameScene_;
    private bool optionFlag_;
    private Vector3 basePos_;

    //SecneをまたいでもObjectが破壊されないようにする
    static OptionCanvasScript Instance = null;

    public static OptionCanvasScript GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<OptionCanvasScript>();
        }
        return Instance;
    }

    private void Awake()
    {
        if (this != GetInstance())
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Active(false);
        titleCanvas_ = GameObject.Find("TitleCanvas");
        basePos_ = optionButton.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "TitleSample")
        {
            Active(false);
            optionButton.gameObject.transform.position = basePos_;
            if (optionFlag_ == false)                                    // オプションを開いていないときの処理
            {
                Active(false);
                titleCanvas_.gameObject.SetActive(true);
            }
            else
            {
                Active(true);
                titleCanvas_.gameObject.SetActive(false);
            }

            // テストコード
            //optionButton.gameObject.transform.position = basePos_;
            //Active(optionFlag_);
            //titleCanvas_.gameObject.SetActive(!optionFlag_);  // Activeとフラグを反転させる

        }
        else if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if(gameScene_ == null)
            {
                gameScene_ = GameScene.FindObjectOfType<GameScene>();
            }

            if (gameScene_.GetPauseFlag() == false)
            {
                Active(false);
                optionButton.gameObject.SetActive(false);               // ポーズ中でなければオプションを開けないようにする
                optionButton.gameObject.transform.position = new Vector3(basePos_.x, basePos_.y+ 75f, basePos_.z);
            }
            else
            {
                if (optionFlag_ == false)
                {
                    Active(false);
                }
                else
                {
                    Active(true);
                }

                // テストコード
                //Active(optionFlag_);
            }
        }
        else
        {
            return;     // 何も処理を行わない
        }
    }

    public void HeadOption()
    {
        optionFlag_ = true;
    }

    public void LeaveOption()
    {
        optionFlag_ = false;
    }

    void Active(bool activeFlg)     // 表示非表示の切り替え
    {
        optionButton.gameObject.SetActive(!activeFlg);
        leaveButton.gameObject.SetActive(activeFlg);
        optionMenu.gameObject.SetActive(activeFlg);
        optionText.gameObject.SetActive(activeFlg);
        backImage.gameObject.SetActive(activeFlg);
    }
}
