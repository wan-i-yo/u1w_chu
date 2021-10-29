using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField] GameObject timeObj;
    TextMeshProUGUI timeText;
    [SerializeField] GameObject gmObj;
    GameManager gm;

    [field: SerializeField] public float GameTime { get; set; }
    
    void Start()
    {
        gm = gmObj.GetComponent<GameManager>();
        timeText = GetComponent<TextMeshProUGUI>();
        timeText.text = string.Format("{0:00}", GameTime);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.GameState == ConstData.GameState.title)
        {
            timeText.text = string.Format("{0:00}", GameTime);
            Time.timeScale = 0;
        }
        else if(gm.GameState == ConstData.GameState.stage1)
        {
            Time.timeScale = 1.0f;
            timeText.text = string.Format("{0:00}", GameTime);
            GameTime -= Time.deltaTime;
        }
        else if(gm.GameState == ConstData.GameState.result1)
        {
            Time.timeScale = 1.0f;
            //音鳴らす
            DOVirtual.DelayedCall(
                2.0f,   // 遅延させる（待機する）時間
                () => {
                    this.gameObject.SetActive(false);
                }
            );
        }
    }
}
