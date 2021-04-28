using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class HandleServerMsg : MonoBehaviour
{
    public static void Welcome(Packet _packet)//ID:1
    {
        string _token = _packet.ReadString();
        int _myRid = _packet.ReadInt();
        Debug.Log($"Temp Token: {_token}");
        Debug.Log($"Room ID: {_myRid}");
        SendToServer.WelcomeReceived($"Server [{_myRid}]: Temp Token: {_token} Room ID: {_myRid} Port: {ListenServer.instance.tcp.port} MaxPlayers: {ListenServer.instance.maxPlayers}");
    }
    public static void GetMap(Packet _packet)//ID:2
    {
        
        int[] triangles = _packet.ReadTriangles();
        Vector3[] vertices = _packet.ReadVertices();
        Debug.Log($"GetMap tris {triangles.Length} verts {vertices.Length}");
        MapManager.instance.SpawnMesh(1,triangles,vertices);
    }
}
