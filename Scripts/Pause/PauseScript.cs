using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;   //　ポーズした時に表示するUI

    public GameScene gameManager;
    private bool pauseFlag_ = false;      // true：pause中　false：ゲーム中

    void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        //　ポーズUIのアクティブ状態切り替え
        if (gameManager.GetPauseFlag() == true)
        {
            ChangePauseActive(gameManager.GetPauseFlag(), 0.0f);
            Debug.Log("pauseUIアクティブ" + pauseUI.activeSelf + "(：ゲーム中true 停止中false)");
        }
        else
        {
            ChangePauseActive(gameManager.GetPauseFlag(), 1.0f);
            Debug.Log("pauseUIアクティブ" + pauseUI.activeSelf + "(：ゲーム中true 停止中false)");
        }
    }

    public bool GetPauseFlag()
    {
        // マウス処理はpause中でも入ってしまうためフラグで管理
        return pauseFlag_;
    }

    public void TransitionTitle()
    {
        SceneManager.LoadScene("TitleSample");
        Time.timeScale = 1.0f;
    }

    // テストコード
    void ChangePauseActive(bool flag,float time)
    {
        pauseUI.SetActive(flag);
        Time.timeScale = time;
        pauseFlag_ = flag;
    }
}
