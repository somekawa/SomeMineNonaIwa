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

    // Start is called before the first frame update

    void Start()
    {
        //BGMを再生
        OptionScript.GetInstance().PlayBGM(0);
    }

    //BGMMenuViewのSliderを動かしたときに呼び出す関数を作成
    public void OnChangedBGMSlider()
    {
        //Sliderの値に応じてBGMを変更
        OptionScript.GetInstance().BGMVolume = bgmSlider.value;
        //volumeTextの値をSliderのvalueに変更
        bgmVolumeText.text = string.Format("{0:0}", bgmSlider.value * 100);
    }

    //SEMenuViewのSliderを動かしたときに呼び出す関数を作成
    public void OnChangedSESlider()
    {
        //Sliderの値に応じてSEを変更
        OptionScript.GetInstance().SEVolume = seSlider.value/5;
        //volumeTextの値をSliderのvalueに変更
        seVolumeText.text = string.Format("{0:0}", seSlider.value * 100);
    }

    //Buttonを押したときに呼ばれる関数
    public void OnPushButton()
    {
        //SEを再生
        OptionScript.GetInstance().PlaySound(0);
    }
}
