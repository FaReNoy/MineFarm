using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUVInfo : MonoBehaviour
{
    public Vector3[] Verticles;
    public int[] Triangles;
    void Update()
    {
        Verticles = GetComponent<MeshFilter>().mesh.vertices;
        Triangles = GetComponent<MeshFilter>().mesh.triangles;
        //foreach (var i in uv)
        //{
        //    Debug.Log(i.ToString());
        //}
    }

   
}
