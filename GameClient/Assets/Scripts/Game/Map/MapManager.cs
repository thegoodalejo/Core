using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public Dictionary<int, ProcMap> maps = new Dictionary<int, ProcMap>();
    public GameObject mapPrefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnMesh(int _id, int[] _triangles, Vector3[] _vertices){
        GameObject _procedMesh;
        _procedMesh = Instantiate(mapPrefab, new Vector3(0,0,0), Quaternion.identity);
        _procedMesh.GetComponent<ProcMap>().Initialize(_id, _triangles, _vertices);
        maps.Add(_id,_procedMesh.GetComponent<ProcMap>());
        Debug.Log("SpawnMesh");

    }
}
