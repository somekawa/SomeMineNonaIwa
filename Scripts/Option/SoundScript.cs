using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public enum Difficulty
    {
        EASY,
        HARD
    };

    public Difficulty difficulty;

    public AudioClip[] bgmList;//BGMを読み込む
    public AudioSource audioSourceBGM;//BGMの音の大きさを調節するために読み込む

    public AudioClip[] seList;//SEを読み込む
    public AudioSource audioSourceSE;//SEの音の大きさを調節するために読み込む

    //BGMのボリューム調節する関数を作成
    public float BGMVolume
    {
        //audioSourceBGMのvolumeをいじることでBGMを調整
        get { return audioSourceBGM.volume; }
        set { audioSourceBGM.volume = value; }
    }

    //SEのボリュームを調節する関数を作成
    public float SEVolume
    {
        //audioSourceSEのvolumeをいじることでSEを調整
        get { return audioSourceSE.volume; }
        set { audioSourceSE.volume = value; }
    }

    //SecneをまたいでもObjectが破壊されないようにする
    static SoundScript Instance = null;

    public static SoundScript GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<SoundScript>();
        }
        return Instance;
    }

    private void Awake()
    {
        if (this != GetInstance())
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    //BGMを再生する関数を作成
    public void PlayBGM(int index)
    {
        audioSourceBGM.clip = bgmList[index];
        audioSourceBGM.Play();
    }

    //SEを再生する関数を作成
    public void PlaySound(int index)
    {
        // 物が落下した際に、他のSEが鳴っていたらそのSEを中断させる
        if (index == 6 && audioSourceSE.isPlaying)
        {
            audioSourceSE.Stop();
        }
        audioSourceSE.clip = seList[index];
        audioSourceSE.Play();
    }
}
