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
            // ��������� ���� ������ � ��� � ��������� ������
            var ball = collision.GetComponent<BallController>();
            ball.AllowShoot();
            ball.ControlCircle(transform);
            isBallInside = true;
            ball.transform.DOMove(transform.position, 0.3f);

            // ����� ����� �����
            var s = DOTween.Sequence();
            s.Append(transform.GetChild(0).DOBlendableScaleBy(new Vector3(0, 0.2f, 0), 0.1f));
            s.Append(transform.GetChild(0).DOBlendableScaleBy(new Vector3(0, -0.2f, 0), 0.1f));

            // ������ ���� ������
            foreach (var i in sprites) i.DOColor(takenColor, 0.3f);
            
            // ���������� ������ ��������� � ������
            fullCircle.gameObject.SetActive(true);
            fullCircle.DOScale(1.7f, 0.35f);
            fullCircle.GetComponent<SpriteRenderer>().DOFade(0, 0.35f);

            // ��������� ���������� ���� �� ������ ������ ��������� ���
            var colliders = GetComponentsInChildren<Collider2D>();
            foreach (var i in colliders) i.enabled = false;

            // ���������� ����� ��������� � ������
            GlobalEventManager.Goal(transform.position);
        }
    }

    void DestroyCircle() // ����������� ������ ����� �������, ���� ������ ���� ��� ���
    {
        if (!isBallInside) return;

        transform.DOScale(0, 0.5f);
        Destroy(gameObject, 0.6f);
    }
}
