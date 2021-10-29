using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    private static bool created = false;
    [SerializeField] private AudioMixer _audioMixer = default;
    [SerializeField] private GameObject bgmSliderObj = default;
    [SerializeField] private GameObject seSliderObj = default;
    [SerializeField] private AudioSource ac = default;
    private Slider bgmSlider = default;
    private Slider seSlider = default;

    // Start is called before the first frame update
    void Start()
    {
        bgmSlider = bgmSliderObj.GetComponent<Slider>();
        seSlider = seSliderObj.GetComponent<Slider>();
    }
    void Awake () {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // SEのVolumeを設定したい時に呼ぶメソッド
    public void ChangeSEVolume(float vol){
        print("SE?"+vol);
        //_audioMixer.SetFloat("SEExposed", ConvertVolumeToDecibel(vol));
        _audioMixer.SetFloat("SEExposed", vol);
        ac.Play();
        
    }
    
    public void ChangeBGMVolume(float vol){
        print("BGM?"+vol);
        //_audioMixer.SetFloat("BGMExposed", ConvertVolumeToDecibel(vol));
        _audioMixer.SetFloat("BGMExposed", vol);
    }
    
    // 要注意な点はここで設定する値は 0 ~ 1 の値ではなくdB単位( -80 ~ 0 )なので直感的な0 ~ 1での制御をし
    // たい場合は別途 0 ~ 1 をデシベルに変換する関数を作成する必要がある
    // 0 ~ 1の値をdB( デシベル )に変換.
    private float ConvertVolumeToDecibel(float vol){
        var result = 0f;
        float VOL_MIN_VAL = 0f;
        float VOL_MAX_VAL = 1f;
        var aaa = Mathf.Clamp(vol, VOL_MIN_VAL, VOL_MAX_VAL);    
        // normalize = (zi - min(x)) /  (max(x) - min(x))
        var normalize = (aaa - VOL_MIN_VAL) / (VOL_MAX_VAL - VOL_MIN_VAL);
        // AudioMix min: -80db, max: 0db
        var masterVolume = normalize * 80 + (-80);
        print("vol:"+vol);
        print("masvo:"+masterVolume);
        return result;
    }
}
