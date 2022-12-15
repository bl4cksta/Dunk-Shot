using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour
{
    [SerializeField] private BallVisuals ballVisuals;
    [SerializeField] private PathRenderer pathRenderer;
    [SerializeField] private float force = 2.5f;
    [SerializeField] private float maxForce = 10, minForce = 3.5f;
    [SerializeField] private Transform currentCircle;

    private Rigidbody2D rb;
    private bool allowMove; // ������ �� ������ ���� ���� �� �����
    private bool isPointerOverUI; // ������ ���� �� ������ �� UI
    private bool isPullingCorrect; // �������� �� ���������� ������ ������� �� ����� �� �� ����� �����
    private bool isFirstMoved; // ���������� ����� �� ������ ���
    private Vector3 mouseStartPos, direction, ballPos;
    private Vector3 circleScale;
    private void Start()
    {
        isPointerOverUI = false;
        isPullingCorrect = false;
        isFirstMoved = false;
        rb = GetComponent<Rigidbody2D>();
        circleScale = currentCircle.GetChild(0).localScale;
        AllowShoot();
    }
    void Update()
    {
        if (!allowMove) return;

        if (Input.GetMouseButtonDown(0))
        {
            // ��������� �� �������������� � UI
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
#else
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
            {
                isPointerOverUI = true;
                return;
            }
            // ���������� ������ ���
            if (!isFirstMoved)
            {
                GlobalEventManager.FirstMove();
                isFirstMoved = true;
            }
            // ��������, ��������� ��������� �����
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            new Plane(-Vector3.forward, transform.position).Raycast(ray, out float enter);
            mouseStartPos = ray.GetPoint(enter);
            ballPos = transform.position;
            isPullingCorrect = true;
        }
        else if (Input.GetMouseButton(0))
        {
            // ���� �� ������ �� UI
            if (isPointerOverUI) return;
            // ���� ����� �����������
            if (!isPullingCorrect) return;

            // ������, ���������� ��������� � ������� �����
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            new Plane(-Vector3.forward, transform.position).Raycast(ray, out float enter);
            var mousePos = ray.GetPoint(enter);
            direction = (mousePos - mouseStartPos) * -force;

            // ������� ��� ������ ������
            var offset = direction;
            offset.y = Mathf.Clamp(offset.y, 0, 10f);
            offset.x = Mathf.Clamp(offset.x, -2.5f, 2.5f);
            transform.SetPositionAndRotation(ballPos + (offset * -0.025f), Quaternion.LookRotation(Vector3.forward, direction));

            // ���������� � ����� ����������
            direction *= 1.5f;
            direction.y = Mathf.Clamp(direction.y, minForce, maxForce);
            direction.x = Mathf.Clamp(direction.x, -maxForce, maxForce); 
            pathRenderer.ShowPath(transform.position, direction, minForce);

            // ������ ������� ������, ���� ��� ����
            if (currentCircle == null) return;
            currentCircle.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            var y = (direction.y - 3.5f) / 15f + 0.71f;
            currentCircle.GetChild(0).localScale = new Vector3(circleScale.x, y, circleScale.z);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!isPointerOverUI && isPullingCorrect)
            {
                // ���������, ��������� ��� ���������� �� ��������
                if (direction.y > minForce)
                    Shot(direction);
                else
                    transform.position = ballPos;

                // ������ ����������
                pathRenderer.HidePath();
                isPullingCorrect = false;
            }

            isPointerOverUI = false;

        }
    }
    void Shot(Vector3 direction) // ������ ���� � �������� �����������
    {
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        rb.simulated = true;
        rb.AddForce(direction, ForceMode2D.Impulse);
        allowMove = false;
        pathRenderer.enabled = false;

        ballVisuals.ImpactRotation(direction.y);
        GlobalEventManager.Move();
    }
    public void AllowShoot() // ��������� ��������� ���
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        allowMove = true;
        pathRenderer.enabled = true;
    }
    public void ControlCircle(Transform circle) // ��������� �������� ������
    {
        currentCircle = circle;
    }
}
