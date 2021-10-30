using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//コンボ数表示
public class Combo : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro = default;
    [SerializeField] private GameObject gameManagerObj = default;
    private GameManager gameManager = default;

    void Start()
    {
        gameManager = gameManagerObj.GetComponent<GameManager>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMeshPro.text = gameManager.Combo.ToString("0");
    }
}
