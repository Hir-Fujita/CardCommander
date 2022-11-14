using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

// MonoBehaviourPunCallbacksを継承して、PUNのコールバックを受け取れるようにする
public class OnlineManager : MonoBehaviourPunCallbacks
{
    public static OnlineManager instance;
    [SerializeField] Text Player_Name;

    private void Awake()
{
    if(instance == null)
    {
        instance = this;
    }
}

    bool room;
    bool matching;

    public void OnlineStart() {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster() {
        // PhotonNetwork.JoinRandomRoom();
    }

    public void Join_Room()
    {
        room = false;
        matching = false;
        PhotonNetwork.JoinRandomRoom();
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom() {
        room = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers = 2}, TypedLobby.Default);
        GameManager.start_ini = true;
        GameManager.instance.Wait_Matching();
    }

    private void Update()
    {
        if (matching)
        {
            return;
        }
        if (room)
        {
            if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                matching = true;
                GameManager.ONLINE = true;
                GameManager.my_name = Player_Name.text;
                GameManager.instance.StartGame();
            }
        }
    }
}
