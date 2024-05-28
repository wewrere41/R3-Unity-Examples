using R3;
using UnityEngine;

public class GameObservables : MonoBehaviour
{
    public readonly Subject<Enemy> EnemyDeath = new();
    public readonly ReactiveProperty<GameManager.GameState> GameState = new(GameManager.GameState.Playing);
}