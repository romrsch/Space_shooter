using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
//using TMPro;
using UniRx;
using System;



public class GameUIController : MonoBehaviour
{
    //[SerializeField] TextMeshProUGUI _countHealth;
    [SerializeField] Text _countHealth;
    [SerializeField] Text _countScore;
    //[SerializeField] TextMeshProUGUI _countScoreWindowGameOver;
    [SerializeField] Text _countScoreWindowGameOver;
    [SerializeField] GameObject _windowGameOver;

    [SerializeField] Slider _barHealth;

    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
         _disposables = new CompositeDisposable();
        var controller = Controller.Instance;

        controller.OnGameOver.Subscribe((_) => ShowWindowGameOver()).AddTo(_disposables);

        controller._myShip._health.Subscribe().AddTo(_disposables);
        controller.Score.Subscribe(UpdateScore).AddTo(_disposables);
    }

    private void UpdateBar(int value)
    { 
        _barHealth.value = ((float)value)/100;
       _countHealth.text = value.ToString(); 

    }

    private void UpdateScore(int score)
    {
        if (!_windowGameOver.activeSelf)  // проверка если окно завершения игры неактивно, то зачисляем очки
        {
            _countScore.text = score.ToString();
        }
        
    }

    public void ShowWindowGameOver()
    {
        _countScoreWindowGameOver.text = Controller.Instance.Score.Value.ToString();
        _windowGameOver.SetActive(true); // включаем окно (true)

    }

    public void ClickToMainMenu()
    {
        LevelManager.PlayScene(Scenes.MainMenu);
        gameObject.SetActive(false); 
    }

    public void ClickRestart()
    {
        LevelManager.PlayScene(Scenes.Game);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_disposables != null) 
        {
            _disposables.Dispose();
        }
        
        _disposables = null;  // обнуляем переменную, чтобы в дальнейшем не утикала память
    
    }
}
