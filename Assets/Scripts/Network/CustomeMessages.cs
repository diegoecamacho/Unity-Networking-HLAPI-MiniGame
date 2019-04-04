
using UnityEngine.Networking;

    enum GameMessageID : short
{
    PlayersReady = MsgType.Highest + 1,
    GameReady,
    LoginPacket,
}