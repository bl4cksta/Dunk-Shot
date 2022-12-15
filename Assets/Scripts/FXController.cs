using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    [SerializeField] private FX fxPrefab, trailFX;

    private GameObject trail;
    public enum FXType
    {
        Perfect,
        Bounce,
        Goal
    }
    public void SpawnFX(Vector3 pos, float duration, int count, FXType type)
    {
        var fx = Instantiate(fxPrefab, pos, Quaternion.identity);
        
        // FX settings
        string text = null;
        Color color = Color.white;
        bool delayed = false;

        switch(type)
        {
            case FXType.Bounce:
                text = "BOUNCE!";
                color = Color.blue;
                break;
            case FXType.Goal:
                text = "+" + count;
                color = Color.gray;
                delayed = true;
                break;
            case FXType.Perfect:
                if(count > 1) text = "PERFECT! x" + count;
                else text = "PERFECT!";
                color = Color.green;
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
