using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private float speed_  = 3.0f;
    private float gravity_= 9.8f;
    private bool walkFlg_ = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        CalculateMove();
    }

    void CalculateMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        if(direction.x != 0 || direction.z != 0)
        {
            walkFlg_ = true;
        }
        else
        {
            walkFlg_ = false;
        }

        Vector3 velocity = direction * speed_;
        velocity.y -= gravity_;
        velocity = transform.transform.TransformDirection(velocity);
        controller.Move(velocity * Time.deltaTime);
    }

    public bool GetWalkFlg()
    {
        return walkFlg_;
    }
}