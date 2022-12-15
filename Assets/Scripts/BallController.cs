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
    private bool allowMove; // запрет на запуск м€ча пока он летит
    private bool isPointerOverUI; // запрет если мы нажали на UI
    private bool isPullingCorrect; // проверка на корректное начало нажати€ на экран не во врем€ полЄта
    private bool isFirstMoved; // отправл€ем ивент на первый ход
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
            // провер€ем на взаимодействие с UI
#if UNITY_EDITOR
            if (EventSystem.current.IsPointerOverGameObject())
#else
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
            {
                isPointerOverUI = true;
                return;
            }
            // отправл€ем первый ход
            if (!isFirstMoved)
            {
                GlobalEventManager.FirstMove();
                isFirstMoved = true;
            }
            // нажимаем, фиксируем начальную точку
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            new Plane(-Vector3.forward, transform.position).Raycast(ray, out float enter);
            mouseStartPos = ray.GetPoint(enter);
            ballPos = transform.position;
            isPullingCorrect = true;
        }
        else if (Input.GetMouseButton(0))
        {
            // если мы нажали на UI
            if (isPointerOverUI) return;
            // если т€нем некорректно
            if (!isPullingCorrect) return;

            // держим, сравниваем начальную и текущую точки
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            new Plane(-Vector3.forward, transform.position).Raycast(ray, out float enter);
            var mousePos = ray.GetPoint(enter);
            direction = (mousePos - mouseStartPos) * -force;

            // двигаем м€ч внутри кольца
            var offset = direction;
            offset.y = Mathf.Clamp(offset.y, 0, 10f);
            offset.x = Mathf.Clamp(offset.x, -2.5f, 2.5f);
            transform.SetPositionAndRotation(ballPos + (offset * -0.025f), Quaternion.LookRotation(Vector3.forward, direction));

            // подготовка и показ траектории
            direction *= 1.5f;
            direction.y = Mathf.Clamp(direction.y, minForce, maxForce);
            direction.x = Mathf.Clamp(direction.x, -maxForce, maxForce); 
            pathRenderer.ShowPath(transform.position, direction, minForce);

            // крутим текущее кольцо, если оно есть
            if (currentCircle == null) return;
            currentCircle.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            var y = (direction.y - 3.5f) / 15f + 0.71f;
            currentCircle.GetChild(0).localScale = new Vector3(circleScale.x, y, circleScale.z);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!isPointerOverUI && isPullingCorrect)
            {
                // отпускаем, запускаем или возвращаем на исходную
                if (direction.y > minForce)
                    Shot(direction);
                else
                    transform.position = ballPos;

                // пр€чем траекторию
                pathRenderer.HidePath();
                isPullingCorrect = false;
            }

            isPointerOverUI = false;

        }
    }
    void Shot(Vector3 direction) // запуск м€ча в заданном направлении
    {
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
        rb.simulated = true;
        rb.AddForce(direction, ForceMode2D.Impulse);
        allowMove = false;
        pathRenderer.enabled = false;

        ballVisuals.ImpactRotation(direction.y);
        GlobalEventManager.Move();
    }
    public void AllowShoot() // разрешаем запускать м€ч
    {
        Physics2D.simulationMode = SimulationMode2D.Script;
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        allowMove = true;
        pathRenderer.enabled = true;
    }
    public void ControlCircle(Transform circle) // установка текущего кольца
    {
        currentCircle = circle;
    }
}
