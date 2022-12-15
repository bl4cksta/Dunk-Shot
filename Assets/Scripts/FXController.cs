using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    [SerializeField] private FX fxPrefab, trailFX;
    [SerializeField] private Color goalColor = Color.gray, perfectColor = Color.green, bounceColor = Color.blue;

    private GameObject trail;
    public enum FXType
    {
        Perfect,
        Bounce,
        Goal
    }
    public void SpawnFX(Vector3 pos, float duration, int count, FXType type)
    {
		// корректировка позиции по границам
		pos.x = Mathf.Clamp(pos.x, -2.1f, 2.1f);
		pos.y += 0.3f;
        var fx = Instantiate(fxPrefab, pos, Quaternion.identity);
        
        // FX settings
        string text = null;
        Color color = Color.white;
        bool delayed = false;

        switch(type)
        {
            case FXType.Bounce:
                text = "BOUNCE!";
                color = bounceColor;
                break;
            case FXType.Goal:
                text = "+" + count;
                color = goalColor;
                delayed = true;
                break;
            case FXType.Perfect:
                if(count > 1) text = "PERFECT! x" + count;
                else text = "PERFECT!";
                color = perfectColor;
                break;
        }

        fx.SetupFX(duration, text, color, delayed);
    }
    
    public void SpawnTrailFX(Transform ball)
    {
        if (trail != null) return;
        trail = Instantiate(trailFX, ball.transform.position, Quaternion.identity, ball).gameObject;
    }
    public void DestroyTrailFX()
    {
        Destroy(trail);
    }
}
