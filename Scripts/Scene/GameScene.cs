using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    private bool pauseFlag;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(pauseFlag != true)
            {
                pauseFlag = true;
            }
            else
            {
                pauseFlag = false;
            }
        }
    }

    public bool GetPauseFlag()
    {
        return pauseFlag;
    }
}
