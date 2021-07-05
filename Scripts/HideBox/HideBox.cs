using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBox : MonoBehaviour
{
    private Outline outline_;
    private HideControl hideControl_;

    private GameObject mannequin_;
    public bool mannequinFlag_;

    // Start is called before the first frame update
    void Start()
    {
        outline_ = gameObject.GetComponent<Outline>();
        outline_.enabled = false;

        GameObject obj = GameObject.Find("Player").gameObject;
        hideControl_ = obj.GetComponent<HideControl>();

        mannequin_= transform.Find("Mannequin").gameObject;
        mannequin_.SetActive(mannequinFlag_);
    }

    // Update is called once per frame
    void Update()
    {
        if(hideControl_.GetHideFlg())
        {
            outline_.enabled = false;
        }

        mannequin_.SetActive(mannequinFlag_);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") 
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
       
    }

    public bool SetMannequin()
    {
        return mannequinFlag_;
    }
}
