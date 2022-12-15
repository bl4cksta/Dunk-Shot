using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFollower : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveDuration = 0.6f;

    private void Awake()
    {
        GlobalEventManager.OnMoved.AddListener(MoveUp);
    }
    void MoveUp()
    {
        transform.DOMove(transform.position + offset, moveDuration);
    }
}
