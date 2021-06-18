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
    public GameObject titleCanvas;
    public Image backImage;                          // オプション画面の背景画像

    private GameScene gameScene;
    private bool optionFlag;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameScene == null) gameScene = GameScene.FindObjectOfType<GameScene>();
        if (SceneManager.GetActiveScene().name == "TitleSample")
        {
            titleCanvas = GameObject.Find("TitleCanvas");
            Active(false);
            if (optionFlag == false)
            {
                Active(false);
                titleCanvas.gameObject.SetActive(true);
            }
            else
            {
                Active(true);
                titleCanvas.gameObject.SetActive(false);
            }
        }
        else if (SceneManager.GetActiveScene().name == "MainGame")
        {
            if (gameScene.GetPauseFlag() == false)
            {
                Active(false);
                optionButton.gameObject.SetActive(false);
            }
            else
            {
                if (optionFlag == false)
                {
                    Active(false);
                }
                else
                {
                    Active(true);
                }
            }
        }
    }

    public void HeadOption()
    {
        optionFlag = true;
    }

    public void LeaveOption()
    {
        optionFlag = false;
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
