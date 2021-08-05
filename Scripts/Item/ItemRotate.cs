using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemRotate : MonoBehaviour
{
    private GameObject targetObject_;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "TutorialScene")
        {
            targetObject_ = GameObject.Find("Player");
        }
        else
        {
            targetObject_ = GameObject.Find("tPlayer");
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(targetObject_.transform);
    }
}
