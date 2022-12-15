using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isAudioActive;
    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] private AudioClip soundGoal, soundMove, soundPickStar;
    [SerializeField] private AudioClip[] bounceSounds, perfectSounds;

    private void Awake()
    {
        GlobalEventManager.OnGoal.AddListener(PlayGoal);
        GlobalEventManager.OnBounce.AddListener(PlayBounce);
        GlobalEventManager.OnStarPicked.AddListener(PlayPickStar);
        GlobalEventManager.OnMoved.AddListener(PlayMove);
        GlobalEventManager.OnPerfectCombo.AddListener(PlayPerfect);
    }
    void PlayGoal(Vector3 pos)
    {
        if (!isAudioActive) return;
        m_AudioSource.PlayOneShot(soundGoal);
    }
    void PlayPickStar()
    {
        if (!isAudioActive) return;
        m_AudioSource.PlayOneShot(soundPickStar);
    }
    void PlayMove()
    {
        if (!isAudioActive) return;
        m_AudioSource.PlayOneShot(soundMove);
    }
    void PlayBounce()
    {
        if (!isAudioActive) return;
        m_AudioSource.PlayOneShot(bounceSounds[Random.Range(0, bounceSounds.Length)]);
    }
    void PlayPerfect(int combo)
    {
        if (!isAudioActive) return;
        combo = Mathf.Clamp(combo, 0, perfectSounds.Length - 1);
        m_AudioSource.PlayOneShot(perfectSounds[combo]);
    }
}
