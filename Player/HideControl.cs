using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideControl : MonoBehaviour
{
    //private Vector3 defaltCameraPos_;
    //private Quaternion defaltCameraRot_;
    private bool hideFlg_ = false;

    //private Vector3 offsetPos = new Vector3(0.0f, 1.0f, 0.0f);
    //private Vector3 offsetRot = new Vector3(-10.0f, 90.0f, 0.0f);

    private GameObject mainCamera_;
    private GameObject boxCamera_;

    private float time_;
    private float timeMax_           = 1.0f;
    private float coolTime_;
    private const float coolTimeMax_ = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera_ = gameObject.transform.Find("LookY/Main Camera").gameObject;
        coolTime_ = Time.time - coolTimeMax_;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hideFlg_)
        {
            return;
        }

        if ((Input.GetKey(KeyCode.F)) && (Time.time - time_ >= timeMax_)) 
        {
            //Camera.main.transform.position = defaltCameraPos_;
            //Camera.main.transform.rotation = defaltCameraRot_;

            if (boxCamera_ != null)
            {
                boxCamera_.gameObject.SetActive(false);
                mainCamera_.gameObject.SetActive(true);
                boxCamera_ = null;
            }

            time_ = 0.0f;
            coolTime_ = Time.time;
            hideFlg_ = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hideFlg_ || (Time.time - coolTime_ <= coolTimeMax_)) 
        {
            return;
        }

        if (other.gameObject.tag == "HideObj")
        {
            Debug.Log("当たった");
            if (!Input.GetKey(KeyCode.F))
            {
                return;
            }

            // 隠れる処理
            //defaltCameraPos_ = Camera.main.transform.position;
            //defaltCameraRot_ = Camera.main.transform.rotation;

            //Vector3 pos = other.gameObject.transform.position;
            //Camera.main.transform.position = pos + offsetPos;

            //Quaternion rot = other.gameObject.transform.rotation;
            //Camera.main.transform.rotation = Quaternion.Euler(rot.x + offsetRot.x, rot.y + offsetRot.y, rot.z + offsetRot.z);

            boxCamera_= other.gameObject.transform.Find("BoxInCamera").gameObject;
            if(boxCamera_!=null)
            {
                mainCamera_.gameObject.SetActive(false);
                boxCamera_.gameObject.SetActive(true);
            }

            hideFlg_ = true;
            time_ = Time.time;
            coolTime_ = 0.0f;
        }
    }

    public bool GetHideFlg()
    {
        return hideFlg_;
    }

}
