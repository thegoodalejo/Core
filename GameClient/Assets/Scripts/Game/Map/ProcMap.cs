using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcMap : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;
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
}
