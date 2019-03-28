using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkBehaviour))]
public class TransformTracker : MonoBehaviour {

    public float positionUpdateTime = 0.0f;
    private float m_currentPositionUpdateTime = 0.0f;

    NetworkBehaviour behaviour = null;
    NetworkClient client = null;

	// Use this for initialization
	void Start () {
		behaviour = GetComponent<NetworkBehaviour>();

        if (client == null)
        {
            client = NetworkManager.singleton.client;

        }

        if (behaviour.isServer)
        {
            NetworkServer.RegisterHandler((short)GameMessageID.TransformPacket, OnTransformReceived);
        }
        else
        {
           client.RegisterHandler((short)GameMessageID.TransformPacket, OnTransformReceived);
        }
    }
	
	// Update is called once per frame
	void Update () {

        m_currentPositionUpdateTime -= Time.deltaTime;
        if (m_currentPositionUpdateTime <= 0.0f)
        {
            SendPositionUpdate();
            m_currentPositionUpdateTime = positionUpdateTime;
        }
		
	}

    private void SendPositionUpdate()
    {
        TransformPacket packet = new TransformPacket
        {
            m_netId = behaviour.netId,
            m_position = transform.position,
            m_rotation = transform.rotation
        };

        bool result = false;
        if (behaviour.isServer)
        {
            result = NetworkServer.SendToAll(TransformPacket.msgID, packet);
        }
        else
        {
           
            result = client.Send(TransformPacket.msgID, packet);
        }

        if (result)
        {
            Debug.Log("Sending msg");
        }
        else
        {
            Debug.Log("Failed Sending msg");
        }

    }

    void OnTransformReceived(NetworkMessage netMsg)
    {
        TransformPacket msg = netMsg.ReadMessage<TransformPacket>();

        if (msg.m_netId == behaviour.netId)
        {
            transform.position = msg.m_position;
            transform.rotation = msg.m_rotation;

        }

    }

}
