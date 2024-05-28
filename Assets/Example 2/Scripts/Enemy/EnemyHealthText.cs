using DG.Tweening;
using R3;
using TMPro;
using UnityEngine;

public class EnemyHealthText : MonoBehaviour
{
    [SerializeField] EnemyObservables _enemyObservables;
    [SerializeField] private TextMeshProUGUI _healthText;

    private void Start()
    {
        _enemyObservables.HealthObservable.Subscribe(UpdateHealthText).AddTo(gameObject);
        _enemyObservables.OnDeathObservable.Subscribe(OnDeath).AddTo(gameObject);
    }

    private void UpdateHealthText(int health)
    {
        _healthText.text = health.ToString();
        _healthText.transform.DOScale(_healthText.transform.localScale, 0.2f)
            .From(_healthText.transform.localScale * 0.25f).SetEase(Ease.OutBack);
    }

    private void OnDeath(Unit _)
    {
        _healthText.color = Color.red;
        _healthText.text = "DEAD";
    }
}