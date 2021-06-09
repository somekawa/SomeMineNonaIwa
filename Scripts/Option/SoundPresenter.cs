using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundPresenter : MonoBehaviour
{
    //BGM
    public TextMeshProUGUI bgmVolumeText;       //BGMMenuViewのvolumeTextを取得
    public Slider bgmSlider;                    //BGMMenuViewのsliderを取得

    //SE
    public TextMeshProUGUI seVolumeText;        //SEMenuViewのvolumeTextを取得
    public Slider seSlider;                     //SEMenuViewのsliderを取得

    public GameObject player_;

    // Start is called before the first frame update

    void Start()
    {
        //BGMを再生
        SoundScript.GetInstance().PlayBGM(0);
    }

    void Update()
    {
        if (player_ == null)
        {
            player_ = GameObject.Find("Player");
        }
        else 
        {
            if (SoundScript.GetInstance().audioSourceBGM.clip != SoundScript.GetInstance().bgmList[1])
            {
                SoundScript.GetInstance().PlayBGM(1);
            }
        }
    }

    //BGMMenuViewのSliderを動かしたときに呼び出す関数を作成
    public void OnChangedBGMSlider()
    {
        //Sliderの値に応じてBGMを変更
        SoundScript.GetInstance().BGMVolume = bgmSlider.value;
        //volumeTextの値をSliderのvalueに変更
        bgmVolumeText.text = string.Format("{0:0}", bgmSlider.value * 100);
    }

    //SEMenuViewのSliderを動かしたときに呼び出す関数を作成
    public void OnChangedSESlider()
    {
        //Sliderの値に応じてSEを変更
        SoundScript.GetInstance().SEVolume = seSlider.value/5;
        //volumeTextの値をSliderのvalueに変更
        seVolumeText.text = string.Format("{0:0}", seSlider.value * 100);
    }

    //Buttonを押したときに呼ばれる関数
    public void OnPushButton()
    {
        //SEを再生
        SoundScript.GetInstance().PlaySound(0);
    }
}
