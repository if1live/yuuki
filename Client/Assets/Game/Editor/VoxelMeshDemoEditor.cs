using UnityEngine;
using UnityEditor;
using System.Collections;
using Yuuki;

/*
 * http://mikolalysenko.github.io/MinecraftMeshes/index.html
 * 와 같은 테스트용 메시를 에디터에서 생성하는것이 목적
 */
[CustomEditor(typeof(VoxelMeshDemoScript))]
public class VoxelMeshDemoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        VoxelMeshDemoScript script = (VoxelMeshDemoScript)target;
        string[] dataSourceList = new string[]
        {
            "1x1x1",
            "2x2x2",
        };
        foreach(string dataSource in dataSourceList)
        {
            if(GUILayout.Button(dataSource))
            {
                script.CreateMesh(dataSource);
            }
        }
    }
}
