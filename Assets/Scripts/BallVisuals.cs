using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVisuals : MonoBehaviour
{
    [SerializeField] private Transform ballPhysics;

    bool isRotating;
    private void Awake()
    {
        GlobalEventManager.OnGoal.AddListener(StopRotation);
    }
    private void Start()
    {
        isRotating = false;
    }
    private void Update()
    {
        transform.position = ballPhysics.position;
        if (isRotating) return;
        transform.rotation = ballPhysics.rotation;
    }

    public void ImpactRotation(float power)
    {
        var direction = new Vector3(0, 0, 180);

        if (transform.position.x < 0) direction = -direction;

        transform.DOBlendableLocalRotateBy(direction * power, 2f, RotateMode.LocalAxisAdd);
        isRotating = true;
    }
    void StopRotation(Vector3 pos)
    {
        isRotating = false;
        transform.DOKill();
    }
}
