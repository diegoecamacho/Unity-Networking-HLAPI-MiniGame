using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TransformPacket : MessageBase {

    public static short msgID = (short)GameMessageID.TransformPacket;
    public TransformPacket()
    {
        m_position = Vector3.zero;
        m_rotation = Quaternion.identity;
    }
    public Vector3 m_position;
    public Quaternion m_rotation;
    public NetworkInstanceId m_netId;
}
