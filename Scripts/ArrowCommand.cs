using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCommand : MonoBehaviour
{
    [SerializeField] private bool primeArrow = false;
    public int Direction { get; private set; }
    void Start()
    {
        // Random.InitState();
        if (primeArrow) {
            Direction = 2;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Direction * 90);
        } else {
            Direction = Random.Range(1, 5);
            //print("arrow command dir:" + Direction);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Direction * 90);
            // print(this);
            // if (Direction == 1) {
            //     print("left:"+Direction);
            // }
            // if (Direction == 2) {
            //     print("down:"+Direction);
            // }
            // if (Direction == 3) {
            //     print("right:"+Direction);
            // }
            // if (Direction == 4) {
            //     print("up:"+Direction);
            // }
        }
    }

    private void OnBecameVisible() {
        this.GetComponent<BoxCollider2D>().enabled = true;
    }
}
