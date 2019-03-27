
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour {
	[Header("Player Stats")]

	[SyncVar]
	public float Health = 100;

	public float MovementSpeed;

	 private int MaxHealth = 100;
	[Header("Weapon Variables")]
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

    public float m_currentPositionUpdateTime = 0.0f;
    public float m_positionUpdateTime = 0.5f;



    NetworkClient m_client;


	//Components
	Rigidbody m_rigidbody;
	[SerializeField] Slider playerHealthSlider;

    public override void OnStartClient()
    {
        m_client = NetworkManager.singleton.client;
        m_client.RegisterHandler((short)GameMessageID.TransformPacket, OnTransformReceived);
    }

    public override void OnStartServer()
    {
       
        NetworkServer.RegisterHandler((short)GameMessageID.TransformPacket, OnTransformReceived);

    }

    // Use this for initialization
    void Start () {
		m_rigidbody = GetComponent<Rigidbody>();		
	}

    

	// Update is called once per frame
	void Update () {

		playerHealthSlider.value = Health / MaxHealth;

		if(!hasAuthority) { return; }

		if (isLocalPlayer)
		{
			HandleInput();
            
		}
	}

	private void HandleInput()
	{
		Vector3 forward = transform.forward * Input.GetAxisRaw("Vertical") * MovementSpeed;

		transform.Rotate(0, Input.GetAxisRaw("Horizontal") * MovementSpeed, 0);

		m_rigidbody.velocity = forward; // right;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}

        m_currentPositionUpdateTime -= Time.deltaTime;
        if (m_currentPositionUpdateTime < 0.0f)
        {
            SendPosition();
            m_currentPositionUpdateTime = m_positionUpdateTime;
        }
    }

    public void SendPosition()
    {
        TransformPacket msg = new TransformPacket
        {
            m_netId = netId,
            m_position = transform.position,
            m_rotation = transform.rotation
        };

        bool sendResult = false;
        if (isServer)
        {
            sendResult = NetworkServer.SendToAll((short)GameMessageID.TransformPacket, msg);

        }
        else
        {
            sendResult = NetworkManager.singleton.client.Send((short)GameMessageID.TransformPacket, msg);
        }

        if (sendResult)
        {
            Debug.Log("Sending msg", this);
        }
        else
        {
            Debug.Log("Failed Sending msg");
        }


    }


    public void TakeDamage(int amount)
	{
		if (!isServer)
		{
			return;
		}

		Health -= amount;

		if (Health <= 0)
		{
			Destroy(gameObject);
		}
	}


	[Command]
	void CmdFire()
	{
		GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

		var bulletRB = bullet.GetComponent<Rigidbody>();
		var bulletScript = bullet.GetComponent<BulletBase>();

		bulletRB.AddForce(transform.forward * bulletScript.BulletSpeed, ForceMode.Impulse);

		Destroy(bullet, bulletScript.BulletLifetime);

		NetworkServer.Spawn(bullet);

	}

    void OnTransformReceived(NetworkMessage netMsg)
    {
        TransformPacket msg = netMsg.ReadMessage<TransformPacket>();

        if (msg.m_netId == netId)
        {
            transform.position = msg.m_position;
            transform.rotation = msg.m_rotation;

        }
    }
}

