using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    private GameObject targetObject_;

    // Start is called before the first frame update
    void Start()
    {
        targetObject_ = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(targetObject_.transform);
    }
}
