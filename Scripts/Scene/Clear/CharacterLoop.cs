using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoop : MonoBehaviour
{
    public GameObject Player;
    private float speed_;
    private float moveZ_;

    void Start()
    {
        moveZ_ = 4.5f;
        speed_ = 10.0f;//4.5f;

    }

    void Update()
    {
        moveZ_+=speed_ * Time.deltaTime;
        Player.transform.position = new Vector3(15.0f, 1.0f, moveZ_);

    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが接触してきたらプレイヤーを初期位置に戻してまた移動させる
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("プレイヤーと接触");
            // speed_ = 4.5f;
            moveZ_ = 5.5f;
            Player.transform.position = new Vector3(15.0f, 1.0f, moveZ_);
        }
    }


}
