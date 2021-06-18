using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Canvas titleCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TransitionGame()
    {
        SoundScript.GetInstance().audioSourceBGM.clip = null;
        SoundScript.GetInstance().audioSourceSE.clip = null;
        SoundScript.GetInstance().PlaySound(2);
        SceneManager.LoadScene("MainScene");
    }

    public void TransitionTutorial()
    {
        SoundScript.GetInstance().audioSourceBGM.clip = null;
        SoundScript.GetInstance().audioSourceSE.clip = null;
        SoundScript.GetInstance().PlaySound(2);
        SceneManager.LoadScene("TutorialScene");
    }

    public void ReturnOption()
    {
        titleCanvas.gameObject.SetActive(true);
    }
    public void HeadOption()
    {
        titleCanvas.gameObject.SetActive(false);
    }
}
