using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FX : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fxText;
    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.1f);
    }
    public void SetupFX(float duration, string text, Color color, bool delayed)
    {
        fxText.text = text;
        fxText.color = color;

        if (!delayed)
        {
            var s = DOTween.Sequence();
            s.Append(transform.DOMove(transform.position + Vector3.up, duration));
            s.Insert(duration - 0.15f, transform.DOScale(0, 0.2f));

            Destroy(gameObject, duration + 0.1f);
        }
        else
        {
            fxText.fontSize *= 1.5f;
            var s = DOTween.Sequence();
            s.Insert(duration / 2, transform.DOMove(transform.position + Vector3.up, duration));
            s.Insert(duration * 1.5f - 0.15f, transform.DOScale(0, 0.2f));

            Destroy(gameObject, duration * 1.5f + 0.1f);
        }
    }
}
