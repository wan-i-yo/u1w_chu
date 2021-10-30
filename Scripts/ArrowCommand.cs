using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//画面中央の矢印
public class ArrowCommand : MonoBehaviour
{
    [SerializeField] private bool primeArrow = false;
    public int Direction { get; private set; }
    void Start()
    {
        if (primeArrow) {
            Direction = 2;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Direction * 90);
        } else {
            Direction = Random.Range(1, 5);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Direction * 90);
        }
    }

    private void OnBecameVisible() {
        this.GetComponent<BoxCollider2D>().enabled = true;
    }
}
