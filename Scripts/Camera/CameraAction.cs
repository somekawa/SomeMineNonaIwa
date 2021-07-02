using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    private float time_      = 0.0f;
    private bool actionFlag_ = false;

    private Vector2 pos_;
    private Vector3 localPos_;

    private bool cameraResetFlag_ = false;
    private float facingTime_ = 0.0f;
    private float speed_ = 0.1f;

    private Camera camera_;
    private float fieldOfView_;
    private float fieldOfViewMin_;

    private GameObject playerObj_;
    private PlayerCameraControll cameraControll_;

    // サウンド
    private AudioSource audioSource_;

    // Start is called before the first frame update
    void Start()
    {
        localPos_ = transform.localPosition;

        camera_ = GetComponent<Camera>();
        fieldOfView_ = camera_.fieldOfView;
        fieldOfViewMin_ = fieldOfView_ / 2.0f;

        playerObj_ = transform.root.gameObject;
        cameraControll_ = playerObj_.GetComponent<PlayerCameraControll>();

        audioSource_ = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraResetFlag_)
        {
            return;
        }

        time_ += Time.deltaTime;
        // カメラを元の方向に戻す
        if (ResetCamera(time_))
        {
            cameraControll_.SetOperationFlag(true);
            time_ = 0.0f;
            cameraResetFlag_ = false;
        }
    }

    public void SanitCameraAction(GameObject gameobj)
    {
        cameraControll_.SetOperationFlag(false);
        if (!actionFlag_)
        {
            // リセット中の場合は中断する
            cameraResetFlag_ = false;
            time_ = 0.0f;

            facingTime_ = 0.0f;
            actionFlag_ = true;

            audioSource_.Play();
        }

        time_ += Time.deltaTime;
        // ズームカメラ
        ZoomCamera();

        // カメラを敵のほうに向ける
        Vector3 enmyPos = gameobj.transform.position;
        enmyPos.y += 2.5f;
        FacingCamera(enmyPos, time_);

        // 画面揺れ
        Shake();
    }

    private void ZoomCamera()
    {
        float view = fieldOfView_ - (((fieldOfView_ - fieldOfViewMin_) / 1.0f) * time_);
        camera_.fieldOfView = Mathf.Clamp(view, fieldOfViewMin_, fieldOfView_);
    }

    public void FacingCamera(Vector3 enmyPos, float time)
    {
        facingTime_ = speed_ * time;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((enmyPos - transform.position).normalized), facingTime_);
    }

    public bool ResetCamera(float time)
    {
        facingTime_ = (speed_ * 5.0f) * time;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(playerObj_.transform.localEulerAngles), facingTime_);

        var angle = transform.localEulerAngles;
        angle.x = Mathf.RoundToInt(angle.x);
        angle.y = Mathf.RoundToInt(angle.y);
        angle.z = Mathf.RoundToInt(angle.z);
        if (angle == Vector3.zero)
        {
            // カメラが元に戻った
            transform.localEulerAngles = Vector3.zero;
            return true;
        }

        return false;
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
        // 音停止
        audioSource_.Stop();

        time_ = 0.0f;
        actionFlag_ = false;

        transform.localPosition = localPos_;
        pos_.x = 0.0f;
        pos_.y = 0.0f;

        cameraResetFlag_ = true;
        facingTime_ = 0.0f;

        camera_.fieldOfView = fieldOfView_;
    }

    public bool CameraLong()
    {
        return audioSource_.isPlaying;
    }
}
