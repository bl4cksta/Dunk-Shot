using UnityEngine;
using UnityEngine.Events;

public class GlobalEventManager
{
    public static UnityEvent OnLose = new UnityEvent(); // ���������
    public static UnityEvent OnMoved = new UnityEvent(); // ������ ����
    public static UnityEvent OnFirstMoved = new UnityEvent(); // ������ ����
    public static UnityEvent<Vector3> OnGoal = new UnityEvent<Vector3>(); // ��������� � ������
    public static UnityEvent OnBounce = new UnityEvent(); // ������ �� ������� Bouncer
    public static UnityEvent OnStarPicked = new UnityEvent(); // ������ ������
    public static UnityEvent<int> OnPerfectCombo = new UnityEvent<int>(); // Perfect! ��������� ��� ������� Bounce!

    public static void Lose()
    {
        OnLose.Invoke();
    }
    public static void Move()
    {
        OnMoved.Invoke();
    }
    public static void FirstMove()
    {
        OnFirstMoved.Invoke();
    }
    public static void Goal(Vector3 pos)
    {
        OnGoal.Invoke(pos);
    }
    public static void Bounce()
    {
        OnBounce.Invoke();
    }
    public static void PickStar()
    {
        OnStarPicked.Invoke();
    }
    public static void PerfectCombo(int combo)
    {
        OnPerfectCombo.Invoke(combo);
    }

}
