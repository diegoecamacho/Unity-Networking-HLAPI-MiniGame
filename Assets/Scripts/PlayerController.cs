using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    [Header("Player Stats")]
    [SyncVar]
    public float Health = 100;

    public float MovementSpeed;
    public float RotationSpeed;

    private int MaxHealth = 100;

    [Header("Weapon Variables")]
    public GameObject bulletPrefab;

    public Transform bulletSpawn;

    public float m_currentPositionUpdateTime = 0.0f;
    public float m_positionUpdateTime = 0.5f;

    private NetworkClient m_client;

    //Components

    private Rigidbody m_rigidbody;

    [SerializeField]
    private Slider playerHealthSlider;

    private bool allowFire = false;
    // Use this for initialization
    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        GenerateColor();
    }


    // Update is called once per frame
    private void Update()
    {
        playerHealthSlider.value = Health / MaxHealth;

        if (!hasAuthority) { return; }

        if (isLocalPlayer)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Vector3 forward = transform.forward * Input.GetAxisRaw("Vertical") * MovementSpeed * Time.deltaTime;

        transform.Rotate(0, Input.GetAxisRaw("Horizontal") * RotationSpeed * Time.deltaTime, 0);

        m_rigidbody.velocity = forward; // right;

        if (Input.GetKeyDown(KeyCode.Space) && allowFire)
        {
            CmdFire();
        }
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        var bulletRB = bullet.GetComponent<Rigidbody>();
        var bulletScript = bullet.GetComponent<BulletBase>();

        bulletRB.AddForce(transform.forward * bulletScript.BulletSpeed, ForceMode.Impulse);

        Destroy(bullet, bulletScript.BulletLifetime);

        NetworkServer.Spawn(bullet);
    }

    public override void OnStartClient()
    {
        m_client = NetworkManager.singleton.client;
        m_client.RegisterHandlerSafe((short)GameMessageID.PlayersReady, OnPacketReceive);
    }

    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler((short)GameMessageID.PlayersReady, OnPacketReceive);
    }

    public void GenerateColor()
    {
        var meshComponent = GetComponent<MeshRenderer>();
        var randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        meshComponent.material.color = randomColor;
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

    public void OnPacketReceive(NetworkMessage netMsg)
    {
        allowFire = true;
        Debug.Log("Packet");
    }
}