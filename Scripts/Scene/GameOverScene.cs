using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text text = gameObject.transform.Find("DeadText").GetComponent<Text>();

        if (SanitMng.deadType_ == SanitMng.DeadType.SANIT)
        {
            text.text = "あなたは正気を失いました";
        }
        else if (SanitMng.deadType_ == SanitMng.DeadType.HIT)
        {
            text.text = "あなたは化け物に捕まりました";
        }
        else
        {
            text.text = "あなたは謎の死を迎えました";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BackTitleScene()
    {
        SceneManager.LoadScene("TitleSample");
    }
}
