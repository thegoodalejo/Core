using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public static int zIndex = 1;
    public static int gForce = 1;
    public static int size = 1;
    private Vector3 core = new Vector3(0,0,0);
    public Vector3 gravity = new Vector3(0,0,0);
    public Vector3[] vertices = {};
    public int myIndex = 0;
    public bool faceFlip = true;
    public GameObject ground;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Zone.Start # {myIndex}");
    }
    public void Initialize(Vector3[] _vetices, Vector3 _zCentro, bool _flip)
    {
        Debug.Log("Zone.Initialize size: " + size);
        myIndex = zIndex;
        zIndex++;
        faceFlip = _flip;
        for (int i = 0; i < _vetices.Length; i++)
        {
            _vetices[i] = _vetices[i]*size;
        }
        Debug.Log($"Target face : {_zCentro.x},{_zCentro.y},{_zCentro.z}");
        Debug.Log($"Distance: {_zCentro.magnitude} , {_zCentro.normalized}");
        gravity.x = _zCentro.x/_zCentro.magnitude*gForce;
        gravity.y = _zCentro.y/_zCentro.magnitude*gForce;
        gravity.z = _zCentro.z/_zCentro.magnitude*gForce;
        Debug.Log($"Gravity: x: {gravity.x} y: {gravity.y} z: {gravity.z}");
        vertices = _vetices;
        UpdateColliderMesh();
        UpdateGroundMesh();
    }
    public void UpdateColliderMesh(){
        Mesh mesh = new Mesh();
        mesh.Clear();
        Vector3[] collVertices = new Vector3[vertices.Length+1];
        collVertices[0] = core;
        for (int i = 1; i < collVertices.Length; i++)
        {
            collVertices[i] = vertices[i-1];
        }
        mesh.vertices = collVertices;
        int[] trisTop = new int[] {1,2,3,3,4,1,0,1,2,0,2,3,0,3,4,0,4,1};
        int[] trisBot = new int[] {1,4,3,3,2,1,0,1,2,0,2,3,0,3,4,0,4,1};
        if(faceFlip){
            mesh.triangles = trisTop;
        }else{
            mesh.triangles = trisBot;
        }
        
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    public void UpdateGroundMesh(){
        Mesh mesh;
        mesh = new Mesh();
        ground.GetComponent<MeshFilter>().mesh = mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        int[] _top = new int[] {0,3,2,2,1,0};
        int[] _bot = new int[] {0,1,2,2,3,0};
        if(faceFlip){
            mesh.triangles = _top;
        }else{
            mesh.triangles = _bot;
        }
        mesh.RecalculateNormals();
        ground.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider obj){
        Debug.Log($"In # {myIndex}");
        obj.GetComponentInParent<PhysicsCore>().gravity = Vector3.zero;
        obj.GetComponentInParent<PhysicsCore>().gravity += gravity;
    }
    void OnTriggerStay(Collider obj){
        //Debug.Log("Stay");
    }

    void OnTriggerExit(Collider obj){
        Debug.Log($"Out # {myIndex}");
        // obj.GetComponentInParent<PhysicsCore>().gravity = Vector3.zero;
        // obj.GetComponentInParent<PhysicsCore>().gravity += -gravity;
    }

    private void OnDrawGizmos () {
        foreach (Vector3 v in vertices)
        {
            Gizmos.color = Color.green;
		    Gizmos.DrawSphere(v,0.3f);
        }
        foreach (Vector3 v in vertices)
        {
            Gizmos.color = Color.red;
		    Gizmos.DrawLine(core,v);
        }
	}
    
}
