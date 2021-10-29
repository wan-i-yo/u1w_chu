using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FistEffect : MonoBehaviour
{
    [SerializeField] private List<Sprite> fists;
    void Start()
    {
        this.transform.DOLocalMove(
            transform.position + new Vector3(5 ,0 ,0),
            0.3f
        ).SetEase(Ease.OutQuad);
    }

    void Update()
    {
        
    }
}
