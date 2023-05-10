using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _roomNameCreate;
    [SerializeField] private TextMeshProUGUI _roomNameJoin;
    [SerializeField] private TextMeshProUGUI _nameText;
    
    private void Start()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(10, 99);
        _nameText.text = PhotonNetwork.NickName;
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_roomNameCreate.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_roomNameJoin.text);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Game");
    }
}
