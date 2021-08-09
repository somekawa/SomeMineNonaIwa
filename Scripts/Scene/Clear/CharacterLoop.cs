using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoop : MonoBehaviour
{
    public GameObject Player;     // プレイヤーを取得
    private float speed_ = 4.5f;  // プレイヤーの動くスピード
    private float moveZ_ = 10.0f;  // プレイヤーの初期のZ座標

    void Start()
    {
    }

    void Update()
    {
        // 移動させる
        moveZ_ += speed_ * Time.deltaTime;
        Player.transform.position = new Vector3(15.0f, 1.0f, moveZ_);

    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが接触してきたらプレイヤーを初期位置に戻してまた移動させる
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("プレイヤーと接触");
            moveZ_ = 1.35f;             // ループポイントに接触した際に飛ぶZ座標
            Player.transform.position = new Vector3(15.0f, 1.0f, moveZ_);
        }
    }
}
