using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearScene : MonoBehaviour
{

    private int remainingSanit_;   // 取得した正気度を保存


    public Text sanitText;    // 正気度表示用

    // Start is called before the first frame update
    void Start()
    {
        remainingSanit_ = (int)SanitMng.sanit_;
        sanitText.text =  "のこりSAN ：" +remainingSanit_;        // 正気度を表示
        Debug.Log("残り正気度" + remainingSanit_);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickButton()
    {
        Debug.Log("クリックしました");
        SceneManager.LoadScene("MainScene");
    }

}
