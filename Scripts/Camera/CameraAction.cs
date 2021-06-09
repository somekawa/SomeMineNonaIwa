using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    private Vector2 pos_;
    private Vector3 localPos_;
    private Quaternion rot_;
    private bool facingFlag_ = false;
    private bool cameraResetFlag_ = false;
    private float time_ = 0.0f;
    private float speed_ = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        localPos_ = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraResetFlag_) 
        {
            return;
        }

        // カメラを元の方向に戻す
        time_ += (speed_ * 5.0f) * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, rot_, time_);
        if (transform.rotation == rot_) 
        {
            cameraResetFlag_ = false;
        }
    }

    public void SanitCameraAction(GameObject gameobj)
    {
        // カメラを敵のほうに向ける
        FacingCamera(gameobj);

        // 画面揺れ
        Shake();
    }

    private void FacingCamera(GameObject gameobj)
    {
        if(!facingFlag_)
        {
            // カメラの方向を保存
            time_ = 0.0f;
            rot_ = transform.rotation;
            facingFlag_ = true;
        }

        time_ += speed_ * Time.deltaTime;
        // 敵の座標
        Vector3 enmyPos = gameobj.transform.position;
        enmyPos.y += 2.5f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((enmyPos - transform.position).normalized), time_);
    }

    private void Shake()
    {
        Vector3 pos = transform.localPosition;
        float x = (pos.x - pos_.x) + Random.Range(-1.0f, 1.0f) * 0.01f;
        float y = (pos.y - pos_.y) + Random.Range(-1.0f, 1.0f) * 0.01f;

        transform.localPosition = new Vector3(x, y, pos.z);

        pos_.x = x - pos.x;
        pos_.y = y - pos.y;
    }

    public void OffShake()
    {
        transform.localPosition = localPos_;
        pos_.x = 0.0f;
        pos_.y = 0.0f;

        facingFlag_ = false;
        cameraResetFlag_ = true;
        time_ = 0.0f;
    }
}
