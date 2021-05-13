using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public GameObject followObject;     // ミニマップカメラで追従する対象

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 高さ以外の軸で対象と同じ座標に追従
        this.gameObject.transform.position = new Vector3(followObject.transform.position.x, this.gameObject.transform.position.y, followObject.transform.position.z);
    }
}
