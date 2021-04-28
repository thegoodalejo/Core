using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public Dictionary<int, Procedural> maps = new Dictionary<int, Procedural>();
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
        _procedMesh.GetComponent<Procedural>().Initialize(_id, _triangles, _vertices);
        maps.Add(_id,_procedMesh.GetComponent<Procedural>());
        Debug.Log("SpawnMesh");

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
