using UnityEngine;
using System;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    [Header("My Network Manager Custom Properties")]
    [SerializeField] TerrainGenerator terrainPrefab = null;
    
    TerrainGenerator terrain = null;

    public override void OnStartServer()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        Settings settings = GameObject.Find("GameManager").GetComponent<Settings>();
        settings.seed = (int)UnityEngine.Random.Range(int.MinValue/10000, int.MaxValue/10000);

        UnityEngine.Random.InitState(settings.seed);
        
        terrain = Instantiate(terrainPrefab, new Vector3(0,0,0), Quaternion.identity);
        terrain.seed = settings.seed;
        NetworkServer.Spawn(terrain.gameObject);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        GameObject.Find("GameManager").GetComponent<MenuToggler>().ShowHide(false);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        
        GameObject.Find("GameManager").GetComponent<MenuToggler>().ShowHide(true);
    }
}
