using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public GameObject door;             // 回転するオブジェクト格納用
    private float minAngle_;            // 回転する前の角度
    private float maxAngle_;            // 回転した後の角度
    public bool openFlag;               // 開くためのフラグ

    // Start is called before the first frame update
    void Start()
    {
        minAngle_ = door.transform.rotation.y;
        maxAngle_ = 90;
        openFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.LerpAngle(minAngle_, maxAngle_, Time.time);
        door.transform.eulerAngles = new Vector3(0, angle, 0);
    }
}
