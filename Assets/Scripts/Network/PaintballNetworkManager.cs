using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManagerHUD))]
public class PaintballNetworkManager : NetworkManager
{

   public int PlayersReady = 0;
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
    }

    public override void OnServerReady(NetworkConnection conn)
    {

        base.OnServerReady(conn);
        Debug.Log("Client Ready");
        PlayersReady++;

        if (PlayersReady >= 2)
        {
            Debug.Log("Clients Found!");

            var connectionPacket = new ConnectionPacket(1);

            NetworkServer.SendToAll((short)GameMessageID.GameReady, connectionPacket);
        }
    }
}
