using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText, maxScoreText, starsText;
    [SerializeField] private GameObject[] menuScreen, gameplayScreen, loseScreen, settingsScreen, pauseScreen;
    [SerializeField] private FXController fxController;
    [SerializeField] private Transform starImage;

    private int score, bounceCount, perfectCombo, starsCount, maxScore;
    private bool isPhysicsFixedUpdate; // сохраняем режим в котором работает физика для паузы
    private void Awake()
    {
        GlobalEventManager.OnGoal.AddListener(Goal);
        GlobalEventManager.OnBounce.AddListener(Bounce);
        GlobalEventManager.OnLose.AddListener(Lose);
        GlobalEventManager.OnFirstMoved.AddListener(FirstMove);
        GlobalEventManager.OnStarPicked.AddListener(AddStar);
    }
    private void Start()
    {
        var mainCam = Camera.main;
        var aspect = mainCam.aspect;

        if(aspect <= 0.47f) // 9:20
        {
            mainCam.orthographicSize = 6.3f;
        }
        else if(aspect <= 0.51f) // 9:18
        {
            mainCam.orthographicSize = 5.7f;
        }
        // загружаем 
        starsCount = PlayerPrefs.GetInt("Stars", 0);
        maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        // устанавливаем
        SetScore(0);
        SetMaxScore(maxScore);
        SetStarsCount(starsCount);

        OpenMenuScreen();
    }
    private void Goal(Vector3 pos)
    {
        var scoreToAdd = 0;

        pos.x = Mathf.Clamp(pos.x, -2.2f, 2.2f);
        FXController.FXType fxType;

        if (bounceCount > 0)
        {
            fxType = FXController.FXType.Bounce;
            scoreToAdd += bounceCount;
            perfectCombo = 0;
        }
        else
        {
            GlobalEventManager.PerfectCombo(perfectCombo);
            fxType = FXController.FXType.Perfect;
            perfectCombo++;
            scoreToAdd += perfectCombo;
        }
        bounceCount = 0;
        SetScore(scoreToAdd);

        fxController.SpawnFX(pos, 0.7f, scoreToAdd, fxType);
        fxController.SpawnFX(pos, 0.7f, scoreToAdd, FXController.FXType.Goal);
    }
    private void Bounce()
    {
        bounceCount++;
    }
    private void SetScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }
    private void Lose()
    {
        if(score > maxScore)
        {
            PlayerPrefs.SetInt("MaxScore", score);
            SetMaxScore(score);
        }
        PlayerPrefs.SetInt("Stars", starsCount);
        OpenLoseScreen();        
    }
    private void FirstMove()
    {
        OpenGameScreen();
    }
    private void OpenLoseScreen()
    {
        foreach (var i in loseScreen)
            i.SetActive(true);

        foreach (var i in menuScreen)
            i.SetActive(false);

        gameplayScreen[1].SetActive(false);
    }
    private void OpenMenuScreen()
    {
        foreach (var i in loseScreen)
            i.SetActive(false);

        gameplayScreen[0].SetActive(false);
        gameplayScreen[1].SetActive(false);

        foreach (var i in menuScreen)
            i.SetActive(true);
    }
    private void OpenGameScreen()
    {
        foreach (var i in loseScreen)
            i.SetActive(false);

        foreach (var i in gameplayScreen)
            i.SetActive(true);

        foreach (var i in menuScreen)
            i.SetActive(false);
    }
    public void OpenSettings()
    {
        foreach (var i in settingsScreen)
            i.SetActive(true);
    }

    private void AddStar()
    {
        starsCount++;
        SetStarsCount(starsCount);
        // лёгкий баунс иконки
        var s = DOTween.Sequence();
        s.Insert(0.6f, starImage.DOBlendableScaleBy(Vector3.one * 0.3f, 0.15f));
        s.Insert(0.75f, starImage.DOBlendableScaleBy(Vector3.one * -0.3f, 0.15f));
    }
    private void SetStarsCount(int count)
    {
        starsText.text = count.ToString();
    }
    private void SetMaxScore(int score)
    {
        maxScoreText.text = score.ToString();
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
    public void CloseSettings()
    {
        foreach (var i in settingsScreen)
            i.SetActive(false);
    }
    public void Pause()
    {
        // останавливаем симуляцию физики
        if (Physics2D.simulationMode == SimulationMode2D.FixedUpdate)
        {
            isPhysicsFixedUpdate = true;
            Physics2D.simulationMode = SimulationMode2D.Script;
        }
        else isPhysicsFixedUpdate = false;
		
		Time.timeScale = 0;

        foreach (var i in pauseScreen)
            i.SetActive(true);
    }
    public void OffPause()
    {
        // восстанавливаем симуляцию физики
        if (isPhysicsFixedUpdate) Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
		
		Time.timeScale = 1;
        foreach (var i in pauseScreen)
		
            i.SetActive(false);
    }
}
