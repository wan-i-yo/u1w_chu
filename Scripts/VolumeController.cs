using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    private static bool created = false;
    [SerializeField] private AudioMixer audioMixer = default;
    [SerializeField] private GameObject bgmSliderObj = default;
    [SerializeField] private GameObject seSliderObj = default;
    [SerializeField] private AudioSource audioSource = default;
    private Slider bgmSlider = default;
    private Slider seSlider = default;

    void Start()
    {
        bgmSlider = bgmSliderObj.GetComponent<Slider>();
        seSlider = seSliderObj.GetComponent<Slider>();
    }

    void Awake () {
        if (!created) {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        } else {
            Destroy(this.gameObject);
        }
    }

    public void ChangeSEVolume(float vol){
        audioMixer.SetFloat("SEExposed", vol);
        audioSource.Play(); // 確認用
        print("SE vol:" + vol);
    }
    
    public void ChangeBGMVolume(float vol){
        audioMixer.SetFloat("BGMExposed", vol);
        print("BGM vol:" + vol);
    }
    
    private void ConvertVolumeToDecibel(float vol){
        float VOL_MIN_VAL = 0f;
        float VOL_MAX_VAL = 1f;
        var roundedVolume = Mathf.Clamp(vol, VOL_MIN_VAL, VOL_MAX_VAL);
        var normalize = (roundedVolume - VOL_MIN_VAL) / (VOL_MAX_VAL - VOL_MIN_VAL);
        var masterVolume = normalize * 80 + (-80);
    }
}
