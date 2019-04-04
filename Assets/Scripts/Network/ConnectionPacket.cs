using System.Collections.Generic;
using UnityEngine.Networking;

public class ConnectionPacket : MessageBase
{
    public ConnectionPacket()
    {
        networkConnections = 0;
    }
    public ConnectionPacket(int connections)
    {
        networkConnections = connections;
    }

    public int networkConnections;
}
