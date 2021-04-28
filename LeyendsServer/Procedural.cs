using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using MongoDB.Bson;

namespace LeyendsServer
{
    class Procedural
    {
        public static int xSize = 100;
        public static int zSize = 100;

        /// <summary>
        /// Get Vertices of the cube
        /// </summary>
        /// <returns></returns>
        public static Vector3[] GetVertices()
        {
            var rng = new Random();
            float MIN_VALUE = -10.0f;
            float MAX_VALUE = 10.0f;
            Vector3[] vertices = new Vector3[(xSize+1)*(zSize+1)];
            
            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    float y = Convert.ToSingle(rng.NextDouble() * (MAX_VALUE - MIN_VALUE) + MIN_VALUE);
                    vertices[i] = new Vector3(x,y,z);
                    i++;
                }
            }

            return vertices;
        }
        public static int[] GetTriangles()
        {
            int[] triangles = new int[xSize * zSize * 6];
            for (int ti = 0, vi = 0, y = 0; y < zSize; y++, vi++) {
                for (int x = 0; x < xSize; x++, ti += 6, vi++) {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;
                }
		}
                            
            return triangles;
        }
    }
}