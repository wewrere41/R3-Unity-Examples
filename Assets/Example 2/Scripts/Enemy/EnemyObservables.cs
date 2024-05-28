using R3;
using UnityEngine;

public class EnemyObservables : MonoBehaviour
{
    public readonly Subject<int> Health = new();
    public Observable<int> HealthObservable => Health;
    public Observable<Unit> OnDeathObservable => HealthObservable.Where(x => x <= 0).Select(_ => Unit.Default);
}