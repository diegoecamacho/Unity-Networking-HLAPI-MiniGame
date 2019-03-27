
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : NetworkManager
{
    public Vector3[] SpawnLocations;
    private int currentSpawnIndex = 0;

    private void OnPlayerConnected(NetworkPlayer player)
    {
        print("Kill Me");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        
    }
}
