using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHitDestroy : MonoBehaviour
{
    public string targeTag;

    /*チュートリアル用*/
    private GameObject tutorial;
    private TutorialCollision tCollision;

    private GameObject tutorialSlender;
    private tTutoriaSlender tSlenderManCtl_;

    void Start()
    {
        tutorial = GameObject.Find("Player/TutorialColl");
        if (tutorial != null)
        {
            tCollision = tutorial.gameObject.GetComponent<TutorialCollision>();
            //  testFlag = false;
        }
        else
        {
            // tutorialが存在しない＝pracitcに移っている
            tutorialSlender = GameObject.Find("TutorialSlender");
            tSlenderManCtl_ = tutorialSlender.gameObject.GetComponent<tTutoriaSlender>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // チュートリアルシーン用スレンダーマンの動き
        if (tutorialSlender != null)
        {
            if (collision.gameObject.tag == targeTag)
            {
                tSlenderManCtl_.soundPoint.x = this.gameObject.transform.position.x;
                tSlenderManCtl_.soundPoint.z = this.gameObject.transform.position.z;
                tSlenderManCtl_.listenFlag = true;

                GameObject obj = (GameObject)Resources.Load("GlassSE");
                if (obj == null)
                {
                    Debug.Log("objがnullです");
                }

                Instantiate(obj);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (collision.gameObject.tag == targeTag)
            {
                GameObject obj = (GameObject)Resources.Load("GlassSE");
                if (obj == null)
                {
                    Debug.Log("objがnullです");
                }

                Instantiate(obj);
                SoundScript.GetInstance().PlaySound(4);
                Destroy(this.gameObject);
            }

        }
    }
}
