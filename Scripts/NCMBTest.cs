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
    }

    public void OnClick()
    {
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking (gameManager.Score);
    }
}
