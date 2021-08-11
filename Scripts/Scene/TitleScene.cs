using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Canvas titleCanvas;
    public GameObject difficultButton;

    void Start()
    {
        Time.timeScale = 1.0f;
        difficultButton.SetActive(false);
    }

    void Update()
    {
    }

    public void TransitionGame()
    {
        //Common("MainScene");
        difficultButton.SetActive(true);
    }

    public void TransitionTutorial()
    {
        Common("TutorialScene");
        SoundScript.GetInstance().difficulty = SoundScript.Difficulty.EASY;
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

    public void DifficultyEasy()
    {
        SoundScript.GetInstance().difficulty = SoundScript.Difficulty.EASY;
        Common("MainScene");
    }

    public void DifficultyHard()
    {
        SoundScript.GetInstance().difficulty = SoundScript.Difficulty.HARD;
        Common("MainScene");
    }
}
