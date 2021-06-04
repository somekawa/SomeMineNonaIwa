using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector2 pos_;
    Vector3 localPos_;
    float hight_;
    // Start is called before the first frame update
    void Start()
    {
        hight_ = transform.localPosition.y;
        localPos_ = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake(GameObject gameobj)
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
        if ((pos_.x == 0.0f) && (pos_.y == 0.0f)) 
        {
            return;
        }
        transform.localPosition = localPos_;
        pos_.x = 0.0f;
        pos_.y = 0.0f;


    }
}
