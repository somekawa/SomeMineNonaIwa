using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public GameObject difficultButton;
    private Canvas titleCanvas_;
    private AudioSource buttonAudio_;

    void Start()
    {
        Time.timeScale = 1.0f;
        difficultButton.SetActive(false);
        buttonAudio_ = this.GetComponent<AudioSource>();
        titleCanvas_ = GameObject.Find("TitleCanvas").GetComponent<Canvas>();
    }

    void Update()
    {
        buttonAudio_.volume = SoundScript.GetInstance().audioSourceSE.volume;
        buttonAudio_.clip = SoundScript.GetInstance().seList[2];
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
        titleCanvas_.gameObject.SetActive(true);
    }

    public void HeadOption()
    {
        titleCanvas_.gameObject.SetActive(false);
    }

    // シーン遷移の共通処理
    private void Common(string sceneName)
    {
        SoundScript.GetInstance().audioSourceBGM.clip = null;
        SoundScript.GetInstance().audioSourceSE.clip = null;
        buttonAudio_.Play();
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
