using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearCollision : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // ドアと接触したらクリアシーンに移る
        if (other.gameObject.tag == "Door")
        {
            Debug.Log("タイトルに戻るドアに接触");
            SceneManager.LoadScene("TitleSample");
        }
        else if (other.gameObject.tag == "target")
        {
            Debug.Log("ゲームに戻るドアに接触");
            SceneManager.LoadScene("MainScene");
        }
    }
}
