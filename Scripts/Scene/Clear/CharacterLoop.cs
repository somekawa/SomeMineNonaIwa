using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoop : MonoBehaviour
{
    //public ResultScript result;
    public GameObject Player;     // プレイヤーを取得
    private float speed_ = 4.5f;  // プレイヤーの動くスピード
    private float moveZ_ = 10.0f;  // プレイヤーの初期のZ座標

    void Start()
    {

    }

    void Update()
    {
        if (Player.GetComponent<CharacterCtl>().enabled == true)
        {
            Player.transform.position = new Vector3(15.0f, 1.0f, 4.5f);
            Destroy(this);
        }
        else
        {
            // 移動させる
            moveZ_ += speed_ * Time.deltaTime;
            Player.transform.position = new Vector3(15.0f, 1.0f, moveZ_);

        }
        Debug.Log(Player.GetComponent<CharacterCtl>().enabled +"ループポイントからみたプレイヤーZ座標" + Player.transform.position.z);
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
