using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBox : MonoBehaviour
{
    private Outline outline_;
    private HideControl hideControl_;

    // Start is called before the first frame update
    void Start()
    {
        outline_ = gameObject.GetComponent<Outline>();
        outline_.enabled = false;

        GameObject obj = GameObject.Find("Player").gameObject;
        hideControl_ = obj.GetComponent<HideControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hideControl_.GetHideFlg())
        {
            outline_.enabled = false;
        }
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
}
