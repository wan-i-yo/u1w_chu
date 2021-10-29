using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultComment : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerObj = default;
    private GameManager gameManager = default;
    TextMeshProUGUI resultText = default;

    void Start()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        resultText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (gameManager.Score > 15000) {
            resultText.text = "とんでもねえ奴と\n同じ時代に\n生まれちまったもんだぜ";
            resultText.fontSize = 65;
        } else if (gameManager.Score > 10000) {
            resultText.text = "う～～～～ん\n胴長！！";
            resultText.fontSize = 100;
        } else if (gameManager.Score > 5000) {
            resultText.text = "あっ…\n助からなさそう";
            resultText.fontSize = 90;
        } else {
            resultText.text = "なんかキミ\nデカくない？";
            resultText.fontSize = 100;
        }
    }
}
