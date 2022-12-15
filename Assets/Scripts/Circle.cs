using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] private bool isBallInside;
    [SerializeField] private Color takenColor;
    [SerializeField] private Transform fullCircle;
    [SerializeField] private SpriteRenderer[] sprites;
    private void Awake()
    {
        GlobalEventManager.OnMoved.AddListener(DestroyCircle);
    }
    private void Start()
    {
        isBallInside = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBallInside) return;

        if(collision.CompareTag("Player"))
        {
            // назначаем наше кольцо в м€ч и разрешаем запуск
            var ball = collision.GetComponent<BallController>();
            ball.AllowShoot();
            ball.ControlCircle(transform);
            isBallInside = true;
            ball.transform.DOMove(transform.position, 0.3f);

            // лЄгкий баунс сетки
            var s = DOTween.Sequence();
            s.Append(transform.GetChild(0).DOBlendableScaleBy(new Vector3(0, 0.2f, 0), 0.1f));
            s.Append(transform.GetChild(0).DOBlendableScaleBy(new Vector3(0, -0.2f, 0), 0.1f));

            // мен€ем цвет кольца
            foreach (var i in sprites) i.DOColor(takenColor, 0.3f);
            
            // визуальный эффект попадани€ в кольцо
            fullCircle.gameObject.SetActive(true);
            fullCircle.DOScale(1.7f, 0.35f);
            fullCircle.GetComponent<SpriteRenderer>().DOFade(0, 0.35f);

            // отключаем коллайдеры чтоб не мешали делать следующий ход
            var colliders = GetComponentsInChildren<Collider2D>();
            foreach (var i in colliders) i.enabled = false;

            // отправл€ем ивент попадани€ в кольцо
            GlobalEventManager.Goal(transform.position);
        }
    }

    void DestroyCircle() // уничтожение кольца после запуска, если внутри него был м€ч
    {
        if (!isBallInside) return;

        transform.DOScale(0, 0.5f);
        Destroy(gameObject, 0.6f);
    }
}
