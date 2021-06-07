using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionCanvasScript : MonoBehaviour
{
    public Button optionButton;                      // オプション画面に行くためのボタン
    public Button leaveButton;                       // オプション画面に行くためのボタン
    public GameObject optionMenu;                    // オプションで表示するオブジェクト
    public TextMeshProUGUI optionText;               // オプション画面と分かるテキスト

    private CameraController cameraController_;
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
        if (cameraController_ == null) cameraController_ = CameraController.FindObjectOfType<CameraController>();
        if (cameraController_ == null)
        {
            Active(false);
            if (optionFlag == false)
            {
                Active(false);
            }
            else
            {
                Active(true);
            }

        }
        else
        {
            if (cameraController_.FullMapFlag() == false)
            {
                Active(false);
                optionButton.gameObject.SetActive(false);
            }
        }

        if (cameraController_.FullMapFlag() == true)
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
    }
}
