using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // ドアと接触したらクリアシーンに移る
        if (other.gameObject.tag == "Door")
        {
            Debug.Log("タイトルに戻るドアに接触");
            //SceneManager.LoadScene("ClearScene");
        }
        else if (other.gameObject.tag == "target")
        {
            Debug.Log("ゲームに戻るドアに接触");

        }
    }

}
