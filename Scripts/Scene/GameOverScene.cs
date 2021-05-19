using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    private NoiseControl noise_;

    // Start is called before the first frame update
    void Start()
    {
        noise_ = gameObject.GetComponent<NoiseControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value * 100.0f < 0.1f) 
        {
            noise_.DiscoveryNoise();
        }
    }
}
