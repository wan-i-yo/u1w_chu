using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class NCMBTest : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerObj = default;
    private GameManager gameManager = default;
    void Start()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        // tmr = timerObj.GetComponent<Timer>();
        // tmrText = tmr.minute.ToString("00") + ":" + ((int) tmr.seconds).ToString ("00");
        // tmrInt = tmr.minute * 60 + (int) tmr.seconds;
    }

    public void OnClick()
    {
        // var timeScore = new System.TimeSpan (0, 0, tmrInt);
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (gameManager.Score);
    }
}
