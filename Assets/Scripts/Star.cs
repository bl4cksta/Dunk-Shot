using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public Vector3 endPoint;
    [SerializeField] private float duration = 0.7f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Pick();
        }
    }

    void Pick()
    {
        GlobalEventManager.PickStar();

        // крутим звезду и отправляем вверх
        var s = DOTween.Sequence();
        s.Append(transform.DOBlendableRotateBy(new Vector3(0, 0, 720), duration, RotateMode.LocalAxisAdd));
        s.Insert(0, transform.DOScale(0.5f, duration));
        s.Insert(0, transform.DOMove(endPoint, duration));

        Destroy(gameObject, duration + 0.1f);
    }
}
