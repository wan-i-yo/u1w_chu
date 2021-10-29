using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    private TextMeshProUGUI _tmpro = default;
    [SerializeField] private GameObject gameManagerObj = default;
    private GameManager _gameManager = default;

    void Start()
    {
        _gameManager = gameManagerObj.GetComponent<GameManager>();
        _tmpro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _tmpro.text = _gameManager.Combo.ToString("0");
    }
}
