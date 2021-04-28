using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Procedural : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;
    public Gradient gradient;
    public int id;
    public void Initialize(int _id, int[] _triangles, Vector3[] _vertices)
    {
        Debug.Log($"Initialize with tri {_triangles.Length} vert {_vertices.Length}");
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        mesh.Clear();
        id = _id;
        mesh.vertices = _vertices;
        mesh.triangles = _triangles;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    // private void OnDrawGizmos () {
	// 	Gizmos.color = Color.black;
	// 	for (int i = 0; i < mesh.vertices.Length; i++) {
	// 		Gizmos.DrawSphere(mesh.vertices[i], 0.1f);
	// 	}
	// }
    
}
