using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Canvas titleCanvas;

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
    }

    public void TransitionGame()
    {
        Common("MainScene");
    }

    public void TransitionTutorial()
    {
        Common("TutorialScene");
    }

    public void ReturnOption()
    {
        titleCanvas.gameObject.SetActive(true);
    }

    public void HeadOption()
    {
        titleCanvas.gameObject.SetActive(false);
    }

    // シーン遷移の共通処理
    private void Common(string sceneName)
    {
        SoundScript.GetInstance().audioSourceBGM.clip = null;
        SoundScript.GetInstance().audioSourceSE.clip = null;
        SoundScript.GetInstance().PlaySound(2);
        SceneManager.LoadScene(sceneName);
    }
}
