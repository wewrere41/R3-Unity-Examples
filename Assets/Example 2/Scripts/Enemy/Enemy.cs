using R3;
using R3.Triggers;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyObservables _enemyObservables;
    [SerializeField] GameObservables _gameObservables;
    [SerializeField] Transform _model;
    [SerializeField] int _currentHP = 100;

    private void Start()
    {   
        _model.OnCollisionEnterAsObservable().Subscribe(OnCollision).AddTo(gameObject);
        _enemyObservables.OnDeathObservable.Subscribe(OnDeath).AddTo(gameObject);
    }

    private void OnCollision(Collision collision)
    {
        Destroy(collision.gameObject);

        if (_currentHP <= 0)
            return;

        _enemyObservables.Health.OnNext(_currentHP -= 25);
    }

    private void OnDeath(Unit _)
    {
        _gameObservables.EnemyDeath.OnNext(this);
    }
    
}