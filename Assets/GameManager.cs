using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    void Start()
    {
        Debug.Log("Start");

        if (isServer)
        {
            NetworkServer.RegisterHandler((short)GameMessageID.GameReady, OnGameReady);

        }
        else
        {
            NetworkClient m_client = NetworkManager.singleton.client;
            m_client.RegisterHandlerSafe((short)GameMessageID.GameReady, OnGameReady);

        }
    }
    public override void OnStartClient()
    {

        Debug.Log("Started Client",gameObject);
    }

    public override void OnStartServer()
    {
        Debug.Log("Started Server");

    }

    void OnGameReady(NetworkMessage message)
    {
        Debug.Log("Game Is Ready");
    }
}
