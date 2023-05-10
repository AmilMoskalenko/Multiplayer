using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private List<Color> _playerColors;

    public event Action OnCoinCollected = delegate { };
    public event Action OnBulletHit = delegate { };

    private PhotonView _photonView;
    private float _inputX;
    private float _inputY;

    private const string CoinTag = "Coin";
    private const string BulletTag = "Bullet";

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        var colorNumber = _photonView.Owner.ActorNumber;
        foreach (var rend in GetComponentsInChildren<SpriteRenderer>())
        {
            rend.color = _playerColors[colorNumber];
        }
    }

    public void Move(Vector2 input, bool isDragging)
    {
        _inputX = input.x;
        _inputY = input.y;
    }

    public void Rotate(Vector2 input, bool isDragging)
    {
        if (!isDragging) return;
        var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void Update()
    {
        transform.position += new Vector3(_inputX * _speed * Time.deltaTime, _inputY * _speed * Time.deltaTime, 0);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(CoinTag))
            OnCoinCollected?.Invoke();
        if (other.CompareTag(BulletTag))
            OnBulletHit?.Invoke();
        other.gameObject.SetActive(false);
    }
}
