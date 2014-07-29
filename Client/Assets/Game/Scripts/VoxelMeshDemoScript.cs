using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Yuuki;
using System;

public class VoxelMeshDemoScript : MonoBehaviour
{
    public Vector3 QuadVertexToVector3(QuadVertex v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public void CreateMesh(string dataSource)
    {
        //Debug.Log(dataSource);
        //Debug.Log(AuthenticationMethod.FORMS.ToString());
        VoxelExampleFactory factory = new VoxelExampleFactory();
        //Chunk chunk = factory.Cube1x1x1();
        //Chunk chunk = factory.Cube2x2x2();
        Chunk chunk = factory.Cube2x2x2();
        IMesher mesher = new StupidMesher();
        List<Quad> quadList = mesher.Build(chunk.voxels, chunk.dims);
   
        //clear mesh
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        Vector3[] vertices = new Vector3[quadList.Count * 4];
        for (int i = 0; i < quadList.Count; ++i)
        { 
            Quad quad = quadList[i];
            vertices[i * 4 + 0] = QuadVertexToVector3(quad.v1);
            vertices[i * 4 + 1] = QuadVertexToVector3(quad.v2);
            vertices[i * 4 + 2] = QuadVertexToVector3(quad.v3);
            vertices[i * 4 + 3] = QuadVertexToVector3(quad.v4);
        }

        int[] triangles = new int[quadList.Count * 6];
        for(int i = 0 ; i < 6 ; ++i)
        {
            triangles[i * 6 + 0] = i * 4 + 0;
            triangles[i * 6 + 1] = i * 4 + 1;
            triangles[i * 6 + 2] = i * 4 + 2;

            triangles[i * 6 + 3] = i * 4 + 0;
            triangles[i * 6 + 4] = i * 4 + 2;
            triangles[i * 6 + 5] = i * 4 + 3;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        meshFilter.mesh = mesh;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
