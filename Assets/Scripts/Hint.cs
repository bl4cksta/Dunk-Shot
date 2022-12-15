using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float delay0, delay1;
    private Sequence loop;
    private Vector3 startPos;
    private void Awake()
    {
        GlobalEventManager.OnFirstMoved.AddListener(SelfDestroy);
    }
    void Start()
    {
        // сохраняем начальную позицию
        startPos = transform.position;
        // двигаем на точку и обратно
        loop = DOTween.Sequence();
        loop.Insert(delay1, transform.DOMove(targetPos, delay0));
        loop.Insert(delay0 + delay1 / 2, transform.DOMove(startPos, delay1));
        loop.OnComplete(Restart);
    }
    void SelfDestroy()
    {
        loop.Kill();
        transform.DOScale(0, 0.2f);
        Destroy(gameObject, 0.3f);
    }
    void Restart()
    {
        loop.Restart();
    }
}
