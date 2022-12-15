using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Stars")]
    [SerializeField] private Star starPrefab;
    [SerializeField] private int randomFactor = 75; // % на отсутствие звезды в сгенерированном кольце
    [Header("Circles generation")]
    [SerializeField] private Circle circlePrefab;
    [SerializeField] private Vector3[] offsetPositionsLeft;
    [SerializeField] private Vector3[] offsetPositionsRight;
    [SerializeField] private Vector3[] circleRotationsLeft;
    [SerializeField] private Vector3[] circleRotationsRight;

    private bool isRight; // сторона спавна кольца
    private Vector3 starEndPoint = new Vector3(2f, 8.2f);

    private void Awake()
    {
        GlobalEventManager.OnGoal.AddListener(SpawnCircle);
    }
    private void Start()
    {
        isRight = false;
    }
    void SpawnCircle(Vector3 pos)
    {
        var rPos = Random.Range(0, offsetPositionsLeft.Length);
        var rRot = Random.Range(0, circleRotationsLeft.Length);
        Vector3 position, rotation;

        if(isRight)
        {
            position = transform.position + offsetPositionsRight[rPos];
            rotation = circleRotationsRight[rRot];
        }
        else
        {
            position = transform.position + offsetPositionsLeft[rPos];
            rotation = circleRotationsLeft[rRot];
        }

        var circle = Instantiate(circlePrefab, position, Quaternion.Euler(rotation));

        isRight = !isRight;

        // спавн звезды
        if(Random.Range(0, 100) > randomFactor)
        {
            var star = Instantiate(starPrefab, position + circle.transform.up * 0.55f, Quaternion.identity);
            star.endPoint = transform.position + starEndPoint;
        }
    }
}
