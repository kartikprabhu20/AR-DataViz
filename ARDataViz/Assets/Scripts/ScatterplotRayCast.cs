
using UnityEngine;

using Photon.Pun;
using ExitGames.Client.Photon;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class ScatterplotRayCast : MonoBehaviourPunCallbacks
{
    public Text Player1Text;
    public Text Player2Text;

    private string myIndex;
    private string oppIndex;


    void Start()
    {
        Player1Text.text = "Player 1: -1";
        Player2Text.text = "Player 2: -1";
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                myIndex = hit.transform.name;

                if (PhotonNetwork.IsConnected)
                {
                    print("Called");
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Hashtable hash = new Hashtable();
                        hash.Add("P1", myIndex);
                        Player1Text.text = "Player 1: " + myIndex;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
                        oppIndex = (string)PhotonNetwork.CurrentRoom.CustomProperties["P2"];
                        Player2Text.text = "Player 2: " + oppIndex;
                    }
                    else
                    {
                        Hashtable hash = new Hashtable();
                        hash.Add("P2", myIndex);
                        Player2Text.text = "Player 2: " + myIndex;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
                        oppIndex = (string)PhotonNetwork.CurrentRoom.CustomProperties["P1"];
                        Player1Text.text = "Player 1: " + oppIndex;
                    }
                }
            }

        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable hash)
    {
        base.OnRoomPropertiesUpdate(hash);
        if (PhotonNetwork.IsMasterClient)
        {
            oppIndex = (string)PhotonNetwork.CurrentRoom.CustomProperties["P2"];
            if (oppIndex != null)
            {
                Player2Text.text = "Player 2: " + oppIndex;
            }
        }
        else
        {
            oppIndex = (string)PhotonNetwork.CurrentRoom.CustomProperties["P1"];
            if (oppIndex != null)
            {
                Player1Text.text = "Player 1: " + oppIndex;
            }
        }
    }

}
