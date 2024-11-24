using System.Collections;
using System.Collections.Generic;
using System.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI informationText;
    [SerializeField] private Button continueButton;
    
    [Inject] private EventBus _eventBus;
    public void Initialize()
    {
        _eventBus.Subscribe<GameEvents.OnScoreCalculated>(OnScoreCalculated);
        
        continueButton.onClick.AddListener(ContinueButtonClicked);
    }
    private void OnDisable()
    {
        _eventBus.Unsubscribe<GameEvents.OnScoreCalculated>(OnScoreCalculated);
    }
    private void OnScoreCalculated(GameEvents.OnScoreCalculated evt)
    {
        panel.SetActive(true);
        
        informationText.text = "Winner: "+evt.PlayerName+"\nScore: "+evt.Score;
    }

    private void ContinueButtonClicked()
    {
        continueButton.enabled = false;
        
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
