using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAndEnemy : MonoBehaviour
{
    public GameObject alarmText;

    void Start()
    {
        Debug.Log(this+"が呼ばれています");
        alarmText.SetActive(false);

    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnCollisionEnter");
        if (other.gameObject.tag == "Enemy")
        {
            alarmText.SetActive(true);
            Debug.Log("敵と接触しました");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnCollisionExitが呼ばれています");
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("敵と距離を取りました");
            alarmText.SetActive(false);
        }
    }
}
