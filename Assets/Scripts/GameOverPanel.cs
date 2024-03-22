using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] 
    private GameObject _wheelObject;
    
    [SerializeField] 
    private GameObject _gameOverPanel;
    
    [SerializeField] 
    private UnityEngine.UI.Button _restartGameButton;
    
    [SerializeField] 
    private UnityEngine.UI.Button _exitApplicationButton;

    [SerializeField] 
    private Car _car;
    
    public void Awake()
    {
        _car.OnGameOver += OnGameOver;
        _restartGameButton.onClick.AddListener(OnRestartGameButtonClicked);
        _exitApplicationButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnDestroy()
    {
        _restartGameButton.onClick.RemoveListener(OnRestartGameButtonClicked);
        _exitApplicationButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    private void OnGameOver()
    {
        _gameOverPanel.SetActive(true);
        _wheelObject.gameObject.SetActive(false);
    }

    private void OnRestartGameButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
