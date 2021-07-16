using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBox : MonoBehaviour
{
    private Outline outline_;
    private HideControl hideControl_;

    private GameObject mannequin_;
    private GameObject huta_;
    public bool mannequinFlag_;

    private bool inFlag_ = false;               // 入ってる
    private bool lastInFlag_ = false;           // 最後に入った箱なのか
    private float inTime_ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        outline_ = gameObject.GetComponent<Outline>();
        outline_.enabled = false;

        GameObject obj = GameObject.Find("Player").gameObject;
        hideControl_ = obj.GetComponent<HideControl>();

        mannequin_= transform.Find("Mannequin").gameObject;
        mannequin_.SetActive(mannequinFlag_);

        huta_ = transform.Find("Crate03b").gameObject;
        huta_.SetActive(!mannequinFlag_);
    }

    // Update is called once per frame
    void Update()
    {
        if (mannequinFlag_)
        {
            inTime_ = 0.0f;
            outline_.enabled = false;
            return;
        }

        if (!hideControl_.GetHideFlg()) 
        {
            // 箱に入ってない
            return;
        }

        if (!lastInFlag_) 
        {
            // 他の箱に入った
            inTime_ = 0.0f;
        }

        if(inFlag_)
        {
            outline_.enabled = false;
            inTime_ += Time.deltaTime;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Player") && (!mannequinFlag_)) 
        {
            outline_.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            outline_.enabled = false;
        }
    }

    public void SetMannequin(bool flag)
    {
        mannequinFlag_ = flag;

        mannequin_.SetActive(mannequinFlag_);
        huta_.SetActive(!mannequinFlag_);
    }

    public bool GetMannequin()
    {
        return mannequinFlag_;
    }

    public void SetInFlag(bool flag)
    {
        inFlag_ = flag;
    }

    public void SetLastInFlag(bool flag)
    {
        lastInFlag_ = flag;
    }

   public float GetInTime()
    {
        return inTime_;
    }
}
