using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    // 共有
    private PlayerCameraControll cameraControll_;               // カメラ操作
    private bool actionFlag_                        = false;    // アクション中
    private bool cameraResetFlag_                   = false;    // リセット中

    // 画面揺れ関連
    private Vector2 variation_;                                 // カメラをずらした量
    private Vector3 localPos_;                                  // 元のカメラの座標

    // カメラを敵のほうに向ける関連
    private GameObject playerObj_;                              // プレイヤー
    private float speed_                            = 0.1f;     // カメラを動かす際の速度
    private float time_                             = 0.0f;     // カメラを動かした時間

    // ズームカメラ関連
    private Camera camera_;                                     // カメラ
    private float fieldOfView_;                                 // カメラの視野
    private float fieldOfViewMin_;                              // カメラの視野の最小値

    // Start is called before the first frame update
    void Start()
    {
        localPos_ = transform.localPosition;

        // カメラの取得
        camera_ = GetComponent<Camera>();
        fieldOfView_ = camera_.fieldOfView;
        fieldOfViewMin_ = fieldOfView_ / 2.0f;

        // プレイヤーオブジェクトの取得
        playerObj_ = transform.root.gameObject;
        cameraControll_ = playerObj_.GetComponent<PlayerCameraControll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraResetFlag_)
        {
            // リセットが完了した
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
            // リセット中だった場合は中断する
            cameraResetFlag_ = false;
            time_ = 0.0f;

            actionFlag_ = true;

            SoundScript.GetInstance().PlaySound(3);
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

    // ズームカメラ
    private void ZoomCamera()
    {
        float view = fieldOfView_ - (((fieldOfView_ - fieldOfViewMin_) / 1.0f) * time_);
        camera_.fieldOfView = Mathf.Clamp(view, fieldOfViewMin_, fieldOfView_);
    }

    // カメラを敵のほうに向ける
    public void FacingCamera(Vector3 enmyPos, float time)
    {
        float facingTime = speed_ * time;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((enmyPos - transform.position).normalized), facingTime);
    }

    // カメラを元の位置に戻す
    public bool ResetCamera(float time)
    {
        float facingTime = (speed_ * 5.0f) * time;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(playerObj_.transform.localEulerAngles), facingTime);
       
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

    // 画面揺れ
    private void Shake()
    {
        Vector3 pos = transform.localPosition;
        float x = (pos.x - variation_.x) + Random.Range(-1.0f, 1.0f) * 0.01f;
        float y = (pos.y - variation_.y) + Random.Range(-1.0f, 1.0f) * 0.01f;

        transform.localPosition = new Vector3(x, y, pos.z);

        variation_.x = x - pos.x;
        variation_.y = y - pos.y;
    }

    // カメラアクションの終了
    public void OffCameraAction()
    {
        time_ = 0.0f;
        actionFlag_ = false;

        transform.localPosition = localPos_;
        variation_.x = 0.0f;
        variation_.y = 0.0f;

        cameraResetFlag_ = true;

        camera_.fieldOfView = fieldOfView_;

        // 音停止
        SoundScript.GetInstance().audioSourceSE.Stop();
    }

    public bool CameraLong()
    {
        return SoundScript.GetInstance().audioSourceSE.isPlaying;
    }
}
