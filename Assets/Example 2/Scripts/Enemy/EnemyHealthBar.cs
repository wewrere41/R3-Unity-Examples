using System;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyObservables _enemyObservables;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _healthDeltaBar;

    private void Start()
    {
        _enemyObservables.HealthObservable.Subscribe(SetHealthBar).AddTo(gameObject);
        _enemyObservables.HealthObservable.Debounce(TimeSpan.FromSeconds(1)).Subscribe(AnimateHpDelta)
            .AddTo(gameObject);
        _enemyObservables.OnDeathObservable.Subscribe(OnDeath).AddTo(gameObject);
    }

    private void SetHealthBar(int health)
    {
        _healthBar.fillAmount = (float) health / 100;
    }

    private void AnimateHpDelta(int health)
    {
        DOTween.To(() => _healthDeltaBar.fillAmount, x => _healthDeltaBar.fillAmount = x, (float) health / 100, 0.2f)
            .SetEase(Ease.InQuad);
    }

    private void OnDeath(Unit _)
    {
        _healthBar.transform.parent.gameObject.SetActive(false);
    }
}