using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class BulletsManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private int _delayShooting;
    [SerializeField] private int _speedBullet;
    
    private PhotonView _photonView;
    private double _lastTickTime;

    private const double ServerDelay = 0.3;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.Time > _lastTickTime + _delayShooting)
        {
            var bullet = BulletPool.SharedInstance.GetPooledObject();
            bullet.transform.position = _bulletSpawn.position;
            bullet.transform.rotation = Quaternion.identity;
            bullet.SetActive(true);
            var angle = _photonView.transform.eulerAngles.z * Mathf.Deg2Rad;
            var rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _speedBullet, ForceMode2D.Impulse);
            _lastTickTime = PhotonNetwork.Time;
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _lastTickTime = PhotonNetwork.Time - _delayShooting + ServerDelay;
    }
}
