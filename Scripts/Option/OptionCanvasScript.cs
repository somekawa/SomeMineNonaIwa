using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionCanvasScript : MonoBehaviour
{
    public Button optionButton;                  // オプション画面に行くためのボタン
    public Button leaveButton;                   // オプション画面に行くためのボタン
    public GameObject optionMenu;                // オプションで表示するオブジェクト
    public TextMeshProUGUI optionText;           // オプション画面と分かるテキスト

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
        optionButton.gameObject.SetActive(true);
        leaveButton.gameObject.SetActive(false);
        optionMenu.gameObject.SetActive(false);
        optionText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void HeadOption()
    {
        optionButton.gameObject.SetActive(false);
        leaveButton.gameObject.SetActive(true);
        optionMenu.gameObject.SetActive(true);
        optionText.gameObject.SetActive(true);
    }

    public void LeaveOption()
    {
        optionButton.gameObject.SetActive(true);
        leaveButton.gameObject.SetActive(false);
        optionMenu.gameObject.SetActive(false);
        optionText.gameObject.SetActive(false);
    }
}
