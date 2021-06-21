using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recognition : MonoBehaviour
{
    private GameObject playerObj_;          // プレイヤー情報

    public GameObject mainCamera_;          // メインカメラ
    private CameraAction cameraAction_;     // 正気度低下時のカメラアクション

    public GameObject lightRange_;          // 懐中電灯内のあたり判定

    private float time_;                    // アクション用タイム

    private bool resetFlag_ = false;        // リセット中か
    private bool haniFlag_ = false;         // 範囲内か
    private float targetAngle_;             // プレイヤーとターゲットとの角度
    private Vector3 defaultAngle_;          // 懐中電灯(コリジョン)の元のアングル
    private Vector3 defaultPos_;            // 懐中電灯(コリジョン)の元の座標

    private HideControl hideControl_;

    // Start is called before the first frame update
    void Start()
    {
        playerObj_ = transform.root.gameObject;

        cameraAction_ = mainCamera_.GetComponent<CameraAction>();

        defaultAngle_ = lightRange_.transform.localEulerAngles;
        defaultPos_ = lightRange_.transform.localPosition;

        hideControl_ = playerObj_.GetComponent<HideControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!resetFlag_)
        {
            return;
        }

        time_ += Time.deltaTime;
        lightRange_.transform.localEulerAngles = defaultAngle_;
        lightRange_.transform.localPosition = defaultPos_;
        //ReseteLightRange();
        if (cameraAction_.ResetCamera(time_))
        {
            time_ = 0.0f;
            resetFlag_ = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag != "Enemy") || (resetFlag_))
        {
            return;
        }

        
    }
        private void OnTriggerStay(Collider other)
    {
        if ((!lightRange_.activeSelf) ||            // ライトは付いているか
            (hideControl_.GetHideFlg()) ||          // 箱に隠れ中か
            (other.gameObject.tag != "Enemy"))      // 当たったオブジェクトが敵か
        {
            return;
        }

        Vector3 enemyPos = other.gameObject.transform.position;
        enemyPos.y += mainCamera_.transform.position.y;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 targetDirection = enemyPos - transform.position;
        targetAngle_ = (Mathf.RoundToInt(Vector3.SignedAngle(forward, targetDirection, Vector3.up) * 0.1f) * 10.0f);
        Debug.Log("角度"+ targetAngle_);
        if (Mathf.Abs(targetAngle_) < 90.0f) 
        {

            haniFlag_ = true;
            MoveLightRange();

            if (!cameraAction_.CameraLong())
            {

                time_ += Time.deltaTime;
                cameraAction_.FacingCamera(enemyPos, time_);
            }
        }
        else if(haniFlag_)
        {
            time_ = 0.0f;
            resetFlag_ = true;
            haniFlag_ = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            return;
        }
        time_ = 0.0f;
        resetFlag_ = true;
        haniFlag_ = false;
    }

    private void MoveLightRange()
    {
        float localEulerAngle = Mathf.RoundToInt((lightRange_.transform.localEulerAngles.y - 360.0f) * 0.1f) * 10.0f;
        float angle = targetAngle_ - 90.0f;
        Debug.Log("localEulerAngle:" + localEulerAngle+" : "+ angle);
        if (Mathf.Abs(localEulerAngle - angle) <= 10.0f)  // 10度以下の誤差までは許容範囲にする
        {
            lightRange_.transform.localEulerAngles = new Vector3(defaultAngle_.x, angle, defaultAngle_.z);
            return;
        }

        Vector3 point = playerObj_.transform.position;
        float period = 180.0f;
        if (targetAngle_ < 0.0f)
        {
            period = -period;
        }
        lightRange_.transform.RotateAround(point, Vector3.up, period * Time.deltaTime);
    }
}
