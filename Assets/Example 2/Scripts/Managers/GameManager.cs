using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObservables _gameObservables;
    [SerializeField] private Button _restartButton;

    public enum GameState
    {
        Playing,
        Completed
    }

    private void Start()
    {
        _gameObservables.GameState
            .Where(state => state == GameState.Completed)
            .Subscribe(SetActiveRestartButton)
            .AddTo(gameObject);

        _restartButton.onClick.AddListener(RestartScene);
    }

    private void SetActiveRestartButton(GameState _)
    {
        _restartButton.gameObject.SetActive(true);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}