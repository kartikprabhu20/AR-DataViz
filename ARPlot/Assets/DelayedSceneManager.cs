using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace CustomSceneManagement
{
    public class DelayedSceneManager : MonoBehaviourPunCallbacks
    {

        void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        void Start()
        {
        }

        IEnumerator LoadScene(string name, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            SceneManager.LoadScene(name);
        }

        // Photon PUN Callbacks
        public override void OnConnected()
        {
            base.OnConnected();
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.JoinOrCreateRoom("ar", roomOptions, TypedLobby.Default);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            StartCoroutine(LoadScene("PlotScene", 2.5f));
        }


        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            StartCoroutine(LoadScene("PlotScene", 2.5f));
        }

    }
}