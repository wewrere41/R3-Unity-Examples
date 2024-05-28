using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    [SerializeField] GameObservables _gameObservables;
    [SerializeField] private List<Enemy> _enemyList;
    [SerializeField] private TextMeshProUGUI _countText;

    private void Start()
    {
        SetEnemyCountText();
        _gameObservables.EnemyDeath.Subscribe(OnEnemyDeath).AddTo(gameObject);
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        _enemyList.Remove(enemy);
        SetEnemyCountText();

        if (_enemyList.Count == 0)
            _gameObservables.GameState.Value = GameManager.GameState.Completed;
    }

    private void SetEnemyCountText()
    {
        _countText.text = $"Enemies left: {_enemyList.Count}";
    }
}