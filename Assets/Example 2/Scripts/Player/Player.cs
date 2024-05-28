using System;
using R3;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObservables _gameObservables;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private GameObject _bulletPrefab;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;

        Observable.EveryUpdate()
            .Where(_ => _gameObservables.GameState.CurrentValue == GameManager.GameState.Playing)
            .Where(_ => Input.GetMouseButtonDown(0))
            .ThrottleFirst(TimeSpan.FromSeconds(_fireRate))
            .Select(_ => _camera.ScreenPointToRay(Input.mousePosition).direction)
            .Subscribe(FireToDirection).AddTo(gameObject);
    }

    private void FireToDirection(Vector3 direction)
    {
        var bullet = Instantiate(_bulletPrefab, _camera.transform.position, Quaternion.identity);

        if (bullet.TryGetComponent(out Rigidbody rb))
            rb.AddForce(direction * 30, ForceMode.Impulse);
    }
}