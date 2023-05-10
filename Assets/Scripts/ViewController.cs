using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ViewController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Header("Prefab")]
    [SerializeField] private GameObject _playerPrefab;
    [Header("UI")]
    [SerializeField] private FixedJoystick _fixedJoystickPosition;
    [SerializeField] private FixedJoystick _fixedJoystickRotation;
    [SerializeField] private Slider _health;
    [SerializeField] private float _hpChange;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private GameObject _popup;
    [SerializeField] private TextMeshProUGUI _popupText;

    private int _coins;
    
    private void Start()
    {
        _roomName.text = string.Format(_roomName.text, PhotonNetwork.CurrentRoom.Name);
        
        var position = new Vector3(Random.Range(-5.5f, 5.5f), Random.Range(-2.5f, 3.5f));
        var player = PhotonNetwork.Instantiate(_playerPrefab.name, position, Quaternion.identity);
        var playerController = player.GetComponent<PlayerController>();
        
        _fixedJoystickPosition.OnDragging += playerController.Move;
        _fixedJoystickRotation.OnDragging += playerController.Rotate;
        playerController.OnCoinCollected += CoinCollected;
        playerController.OnBulletHit += BulletHit;
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
    
    private void CoinCollected()
    {
        _coins++;
        _coinsText.text = _coins.ToString();
    }

    private void BulletHit()
    {
        _health.value -= _hpChange;
        if (_health.value == 0)
            Death();
    }
    
    private void Death()
    {
        PhotonNetwork.RaiseEvent(0, true, RaiseEventOptions.Default, SendOptions.SendReliable);
        PhotonNetwork.LeaveRoom();
    }
    
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 0:
            {
                if (_health.value > 0 && PhotonNetwork.CurrentRoom.PlayerCount == 2)
                    PopupVictory();
                break;
            }
        }
    }

    private void PopupVictory()
    {
        _popupText.text = string.Format(_popupText.text, PhotonNetwork.NickName, _coins);
        _popup.SetActive(true);
    }
}
