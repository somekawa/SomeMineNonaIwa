using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Canvas titleCanvas;
    public SoundScript buttonSE;

    // Start is called before the first frame update
    void Start()
    {
        //titleCanvas.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))            // キーが押されたとき
        //{
        //    titleCanvas.gameObject.SetActive(false);
        //    optionCanvas.gameObject.SetActive(true);
        //}
    }

    public void TransitionGame()
    {
        buttonSE.GetComponent<SoundScript>().PlaySound(2);
        SceneManager.LoadScene("MainScene");
    }

    public void TransitionTutorial()
    {
        buttonSE.GetComponent<SoundScript>().PlaySound(2);
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
