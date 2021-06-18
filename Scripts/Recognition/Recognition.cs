using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recognition : MonoBehaviour
{
    private GameObject playerObj_;
    private CameraAction cameraAction_;
    public GameObject mainCamera_;
    public GameObject lightRange_;

    private float time_;

    private bool resetFlag_ = false;
    private bool haniFlag_ = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObj_ = transform.root.gameObject;
        cameraAction_ = mainCamera_.GetComponent<CameraAction>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!resetFlag_)
        {
            return;
        }

        time_ += Time.deltaTime;
        if(cameraAction_.ResetCamera(time_))
        {
            time_ = 0.0f;
            resetFlag_ = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }
        //time_ = 0.0f;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag != "Enemy")||(resetFlag_))
        {
            return;
        }

        Vector3 enemyPos = other.gameObject.transform.position;
        enemyPos.y += mainCamera_.transform.position.y;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 targetDirection = enemyPos - transform.position;
        //float objAngles = mainCamera_.transform.localEulerAngles.y;
        float angle = Vector3.Angle(forward, targetDirection);

        Debug.Log("角度"+angle);

        if ((!cameraAction_.CameraLong()) && (angle < 90.0f))  
        {
            haniFlag_ = true;
            time_ += Time.deltaTime;

            cameraAction_.FacingCamera(enemyPos, time_);
        }
        else if(haniFlag_)
        {
            time_ = 0.0f;
            resetFlag_ = true;
            haniFlag_ = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }
        time_ = 0.0f;
        resetFlag_ = true;
        haniFlag_ = false;
    }
}
