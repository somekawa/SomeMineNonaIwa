using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HitDestroy : MonoBehaviour
{
    public string targeTag;
    //private bool testFlag;

    private GameObject tutorial;
    private TutorialCollision tCollision;

    private GameObject slenderMan_;
    private GameObject tutorialSlender;
    private SlenderManCtl slenderManCtl_;
    private tTutoriaSlender tSlenderManCtl_;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene")
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
        else
        {
            slenderMan_ = GameObject.Find("Slender");
            slenderManCtl_ = slenderMan_.gameObject.GetComponent<SlenderManCtl>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            if (tutorialSlender != null)
            {

                if (collision.gameObject.tag == targeTag)
                {
                    tSlenderManCtl_.soundPoint.x = this.gameObject.transform.position.x;
                    tSlenderManCtl_.soundPoint.z = this.gameObject.transform.position.z;
                    tSlenderManCtl_.listenFlag = true;
                    Debug.Log("slenderManCtl_.soundPoint.x:" + tSlenderManCtl_.soundPoint.x +
                    "           slenderManCtl_.soundPoint.z:" + tSlenderManCtl_.soundPoint.z);

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
                    Destroy(this.gameObject);
                }

            }
        }
        else
        {    // このScriptがアタッチされているオブジェクトが、指定したターゲットに接触した時
           // このオブジェクトが消滅する
            if (collision.gameObject.tag == targeTag)
            {
                slenderManCtl_.soundPoint.x = this.gameObject.transform.position.x;
                slenderManCtl_.soundPoint.z = this.gameObject.transform.position.z;
                slenderManCtl_.listenFlag = true;
                GameObject obj = (GameObject)Resources.Load("GlassSE");
                if (obj == null)
                {
                    Debug.Log("objがnullです");
                }

                Instantiate(obj);
                Destroy(this.gameObject);
            }
        }
    }
}
