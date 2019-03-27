
using UnityEngine.Networking;

    enum GameMessageID : short
{
    TransformPacket = MsgType.Highest + 1,
    LoginPacket,
}