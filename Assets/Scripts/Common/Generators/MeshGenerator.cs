using UnityEngine;

namespace GameEngine.Common
{
    public static class MeshGenerator
    {
        public static Mesh CreateCube(float size, float noiseStrength)
        {
            var cubeMesh = new Mesh();

            var vertices = new[]
            {
                new Vector3(-size / 2f, -size / 2f, -size / 2f),
                new Vector3(size / 2f, -size / 2f, -size / 2f),
                new Vector3(size / 2f, -size / 2f, size / 2f),
                new Vector3(-size / 2f, -size / 2f, size / 2f),
                new Vector3(-size / 2f, size / 2f, -size / 2f),
                new Vector3(size / 2f, size / 2f, -size / 2f),
                new Vector3(size / 2f, size / 2f, size / 2f),
                new Vector3(-size / 2f, size / 2f, size / 2f)
            };

            var triangles = new[]
            {
                0, 1, 2,
                0, 2, 3,
                4, 5, 1,
                4, 1, 0,
                7, 6, 5,
                7, 5, 4,
                3, 2, 6,
                3, 6, 7,
                1, 5, 6,
                1, 6, 2,
                4, 0, 3,
                4, 3, 7
            };
            
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] += Random.insideUnitSphere * noiseStrength;
            }

            cubeMesh.vertices = vertices;
            
            cubeMesh.triangles = triangles;
            
            cubeMesh.RecalculateNormals();
            
            return cubeMesh;
        }
    }
}