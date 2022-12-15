using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [SerializeField] private int pointsCount = 10;
    [SerializeField] private Rigidbody2D ballPrefab;

    private LineRenderer lineRenderer;
    private Color startColor;
    private bool isRendererEnabled;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pointsCount;
        isRendererEnabled = true;
        startColor = lineRenderer.startColor;
    }

    public void ShowPath(Vector3 startPos, Vector3 direction, float minForce)
    {
        if (!isRendererEnabled) OnEnable();

        var simRb = Instantiate(ballPrefab, startPos, Quaternion.identity);
        simRb.AddForce(direction, ForceMode2D.Impulse);

        var points = new Vector3[pointsCount];

        points[0] = startPos;
        for (int i = 1; i < pointsCount; i++)
        {
            Physics2D.Simulate(0.02f);
            points[i] = simRb.transform.position;
        }

        lineRenderer.SetPositions(points);

        if (direction.y >= minForce + 2f)
            lineRenderer.startColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        else
            lineRenderer.startColor = new Color(startColor.r, startColor.g, startColor.b, Mathf.Clamp((direction.y - minForce) / 2f, 0, 1f));

        Destroy(simRb.gameObject);
    }
    public void HidePath()
    {
        OnDisable();
    }
    private void OnDisable()
    {
        if (!isRendererEnabled || lineRenderer == null) return;

        lineRenderer.enabled = false;
        isRendererEnabled = false;
        lineRenderer.positionCount = 0;
    }
    private void OnEnable()
    {
        if (isRendererEnabled || lineRenderer == null) return;

        lineRenderer.enabled = true;
        isRendererEnabled = true;
        lineRenderer.positionCount = pointsCount;
    }
}
