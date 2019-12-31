using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Doozy.Engine.UI;

using TMPro;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

using System.Collections.Generic;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    #region Constants

    // Colors
    private Color GREEN = new Color32(104, 159, 56, 255);
    private Color RED = new Color32(211, 47, 47, 255);

    // Photon
    private const string GAME_VERSION = "1.0";
    private const int MAX_PLAYERS_IN_ROOM = 2;
    private const int MAX_PLAYERS_IN_NETWORK = 20;

    #endregion

    #region Editor Variables

    public Image NetworkIndicator;
    public TextMeshProUGUI NetworkStatus;
    public UIView RoomUIView;
    public TMP_InputField RoomNameInput;
    public Button JoinButton;
    public TextMeshProUGUI RoomStatus;
    public UIView DatasetUIView;
    public TMP_Dropdown DatasetDropdown;
    public Button LoadButton;

    #endregion

    #region Class Variables

    string playerName = "";
    string roomName = "";

    #endregion

    void Start()
    {
        PlayerPrefs.DeleteAll();
        BetterStreamingAssets.Initialize();
        // Check for internet connection before trying to reach PHOTON
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            NetworkStatus.text = "Connection Not Found";
            NetworkStatus.color = RED;
            NetworkIndicator.color = RED;
        }
        else
        {
            ConnectToPhoton();
            LoadDatasetOptions();
        }

    }

    void LoadDatasetOptions()
    {
        List<string> Datasets = new List<string>();
        
        string path = "Dataset/";
        string extension = ".csv";
        
        string[] fileInfo = BetterStreamingAssets.GetFiles(path, "*.csv");

        string filename;
        foreach (string file in fileInfo)
        {
            filename = file.Replace(path, "");
            filename = filename.Replace(extension, "");
            filename = char.ToUpper(filename[0]) + filename.Substring(1);
            Datasets.Add(filename);
        }

        DatasetDropdown.AddOptions(Datasets);
    }

    void ConnectToPhoton()
    {
        NetworkStatus.text = "Connecting . . .";
        PhotonNetwork.GameVersion = GAME_VERSION;
        PhotonNetwork.ConnectUsingSettings();
    }

    void OnConnectionSuccess()
    {
        NetworkStatus.text = "Connected";
        NetworkStatus.color = GREEN;
        NetworkIndicator.color = GREEN;
        RoomUIView.Show();
    }

    void OnConnectionFailed()
    {
        NetworkStatus.text = "Connection Unsuccessful";
        NetworkStatus.color = RED;
        NetworkIndicator.color = RED;
    }

    // Photon PUN Callbacks
    public override void OnConnected()
    {
        base.OnConnected();
        OnConnectionSuccess();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        OnConnectionFailed();
    }

    public void OnClickJoinButton()
    {
        JoinRoom();
    }

    public void JoinRoom()
    {
        roomName = RoomNameInput.text;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = MAX_PLAYERS_IN_ROOM;

        TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default);

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.IsMasterClient)
        {
            DatasetUIView.Show();
        }
        else
        {
            LoadScatterPlotScene();
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        RoomStatus.gameObject.SetActive(true);
        Invoke("DisableRoomStatus", 3f);
    }

    private void DisableRoomStatus()
    {
        RoomStatus.gameObject.SetActive(false);
    }

    public void OnClickFinishButton()
    {
        Hashtable hashTable = new Hashtable();
        string datasetname = DatasetDropdown.captionText.text.ToLower();
        hashTable.Add("dataset", datasetname);
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashTable);
        print(datasetname);

        LoadScatterPlotScene();
    }

    void LoadScatterPlotScene()
    {
        SceneManager.LoadScene("ScatterplotScene");
    }

}
