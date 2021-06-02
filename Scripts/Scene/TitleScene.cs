using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour
{
    public Canvas titleCanvas;
    public Canvas optionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        titleCanvas.gameObject.SetActive(true);
        optionCanvas.gameObject.SetActive(false);
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
        SceneManager.LoadScene("MainScene");
    }

    public void TransitionTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void ReturnOption()
    {
        titleCanvas.gameObject.SetActive(true);
        optionCanvas.gameObject.SetActive(false);
    }
    public void HeadOption()
    {
        titleCanvas.gameObject.SetActive(false);
        optionCanvas.gameObject.SetActive(true);
    }
}
