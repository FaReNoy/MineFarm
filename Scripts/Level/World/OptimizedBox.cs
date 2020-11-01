using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class OptimizedBox : MonoBehaviour
{
    public Texture2D Texture;
    private void Start()
    {
        //CreateTrueBox(new SideParams(true, true, true, true, true, true), Texture);
    }

    public static void DeleteSidesInMesh(ref int[,,] chunkMap  , Vector3 blockPos) 
    {
        chunkMap[(int)blockPos.z, (int)blockPos.y, (int)blockPos.x] = 0;
        //chunkMap[(int)blockPos.z, (int)blockPos.y, (int)blockPos.x]
    }
  

    public static Mesh GetMeshFromBlockMap(SideParams[] sideParams)
    {
        Mesh mesh = new Mesh();
        
        int verticesLength = mesh.vertices.Length;
        int i = 0;
        int b = verticesLength;
        int c = mesh.triangles.Length;

        int allSidesCount = 0;
        foreach (var k in sideParams)
        {
            allSidesCount += k.GetSidesCount();
        }

        Vector3[] vertices = new Vector3[b + (allSidesCount * 4)];
        int[] triangles = new int[c + (allSidesCount * 6)];
        Vector2[] uvMap = new Vector2[b + (allSidesCount * 4)];
        for (var g = 0; g < sideParams.Length; g++)
        {

            if (sideParams[g].Backward)
            {

                //Debug.Log($"i = {i}");
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                i += 4;
                c += 6;
                AddTriangles(ref triangles, verticesLength + i, c);
                
                AddUVToSide(ref uvMap, verticesLength + i , SideType.Backward);
            }

            if (sideParams[g].Down)
            {

                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                i += 4;
                c += 6;
                AddTriangles(ref triangles, verticesLength + i, c);

                AddUVToSide(ref uvMap, verticesLength + i , SideType.Down);
            }
            if (sideParams[g].Forward)
            {
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                i += 4;
                c += 6;
                AddTriangles(ref triangles, verticesLength + i, c);

                AddUVToSide(ref uvMap, verticesLength + i , SideType.Forward);
            }
            if (sideParams[g].Up)
            {

                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                i += 4;
                c += 6;
                AddTriangles(ref triangles, verticesLength + i, c);

                AddUVToSide(ref uvMap, verticesLength + i , SideType.Up);
            }

            if (sideParams[g].Left)
            {


                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(-0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                i += 4;
                c += 6;
                AddTriangles(ref triangles, verticesLength + i, c);

                AddUVToSide(ref uvMap, verticesLength + i ,SideType.Left);
            }

            if (sideParams[g].Right)
            {

                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, 0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, 0.5f + sideParams[g].Position.z));
                b++;
                vertices[b] = (new Vector3(0.5f + sideParams[g].Position.x, -0.5f + sideParams[g].Position.y, -0.5f + sideParams[g].Position.z));
                b++;
                i += 4;
                c += 6;
                AddTriangles(ref triangles, verticesLength + i, c);
                AddUVToSide(ref uvMap, verticesLength + i , SideType.Right);

            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvMap;
        return mesh;
    }
    private static void PlaceArrayInArray<T>(ref T[] a , T[] b)
    {
        for (var i = 0; i < b.Length; i++)
        {
            a[i] = b[i];
        }
    }
    private static void AddTriangles(ref int[] triangles, int lastVertsCount , int lastTriangle)
    {

        triangles[lastTriangle - 6] = (lastVertsCount - 4);
        triangles[lastTriangle - 5] = (lastVertsCount - 3);
        triangles[lastTriangle - 4] = (lastVertsCount - 2);
        triangles[lastTriangle - 3] = (lastVertsCount - 2);
        triangles[lastTriangle - 2] = (lastVertsCount - 1);
        triangles[lastTriangle - 1] = (lastVertsCount - 4);
    }
    private static void AddUVToSide(ref Vector2[] uvMap , int lastVertsCount , SideType side = SideType.Default)
    {
        
        switch (side) 
        { 
            case SideType.Default:
                uvMap[lastVertsCount - 4] = (new Vector2(0, 1));
                uvMap[lastVertsCount - 3] = (new Vector2(1, 1));
                uvMap[lastVertsCount - 2] = (new Vector2(1, 0));
                uvMap[lastVertsCount - 1] = (new Vector2(0, 0));
                break;
            case SideType.Up:
                AddUV(ref uvMap, lastVertsCount, 1, 1);
                break;
            case SideType.Down:
                AddUV(ref uvMap, lastVertsCount, 0, 2);
                break;
            case SideType.Left:
                AddUV(ref uvMap, lastVertsCount, 0, 1);
                break;
            case SideType.Right:
                AddUV(ref uvMap, lastVertsCount, 2, 1);
                break;

            case SideType.Forward:
                AddUV(ref uvMap, lastVertsCount, 1, 2);
                break;

            case SideType.Backward:
                AddUV(ref uvMap, lastVertsCount, 1, 0);
                break;
            
        }
       
        
    }
    private static void AddUV(ref Vector2[] uvMap , int lastVertsCount , int posX , int posY) 
    {
        uvMap[lastVertsCount - 4] = (new Vector2((posX / 3f), ((posY + 1) / 3f)));
        uvMap[lastVertsCount - 3] = (new Vector2(((posX + 1) / 3f), ((posY + 1) / 3f)));
        uvMap[lastVertsCount - 2] = (new Vector2(((posX + 1) / 3f), ((posY) / 3f)));
        uvMap[lastVertsCount - 1] = (new Vector2((posX / 3f), (posY / 3f)));
    }
}
public class SideParams
{
    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;
    public bool Forward;
    public bool Backward;
    public Vector3 Position;
    public byte ID;
    public SideParams(bool left, bool right, bool up, bool down, bool forward, bool backward, Vector3 position , byte id)
    {
        Left = left;
        Right = right;
        Up = up;
        Down = down;
        Forward = forward;
        Backward = backward;
        Position = position;
        ID = id;
    }
    public SideParams(bool left, bool right, bool up, bool down, bool forward, bool backward)
    {
        Left = left;
        Right = right;
        Up = up;
        Down = down;
        Forward = forward;
        Backward = backward;
    }
    
    public SideParams(Vector3 position)
    {
        Position = position;
    }
    public SideParams()
    {

    }
    public bool IsFullDisabled()
    {
        if (Left == false && Right == false && Up == false && Down == false && Forward == false && Backward == false)
            return true;
        else
            return false;
    }
    public void ThrowDebugInfo()
    {
        UnityEngine.Debug.Log($"l - {Left} ; r - {Right} ; u - {Up} d - {Down} ; f - {Forward} ; b - {Backward}");
    }
    public int GetSidesCount() 
    { 
        int coef = 0;
        if (Left)
            coef++;
        if(Right)
            coef++;
        if(Forward)
            coef++;
        if(Backward)
            coef++;
        if(Up)
            coef++;
        if(Down)
            coef++;
        return coef;
    }
    public static SideParams GetSideParamsFromBlock(WorldResources worldRes , Point chunkPosInWorldArray, Vector3 blockPos , bool chunkEdges = true) 
    {
        
        ArrayDimensions CMSize = new ArrayDimensions(worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap.GetLength(0), worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap.GetLength(1), worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap.GetLength(2));

        SideParams sp = new SideParams();
        if (blockPos.x - 1 < 0)
        {
            if (chunkPosInWorldArray.X - 1 < 0) 
            {
                sp.Left = chunkEdges;
            }
            else
            {
               
                if (worldRes.WorldMap[(int)chunkPosInWorldArray.Y, (int)chunkPosInWorldArray.X - 1].ChunkMap[(int)blockPos.z, (int)blockPos.y, (int)CMSize.X - 1] == 0)
                {
                    sp.Left = true;
                }
                else
                {
                    sp.Left = false;
                }
               
               
            }
           
        }
        else
        {
            if (worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z, (int)blockPos.y, (int)blockPos.x - 1] == 0) { sp.Left = true; }
        }

        if (blockPos.y - 1 < 0)
        {
            if (chunkPosInWorldArray.Y - 1 < 0)
            {
                sp.Backward = chunkEdges; //false
            }
            else
            {

                if (worldRes.WorldMap[(int)chunkPosInWorldArray.Y - 1, (int)chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z, (int)CMSize.Y - 1, (int)blockPos.x] == 0)
                {
                    sp.Backward = true;
                }
                else
                {
                    sp.Backward = false;
                }


            }
        }
        else
        {
            if (worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z, (int)blockPos.y - 1, (int)blockPos.x] == 0) { sp.Backward = true; }
        }

        if (blockPos.z - 1 < 0)
        {
            sp.Down = false;
        }
        else
        {
            if (worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z - 1, (int)blockPos.y, (int)blockPos.x] == 0) { sp.Down = true; }
        }

        if (blockPos.x + 1 >= CMSize.X)
        {
            if (chunkPosInWorldArray.X + 1 >= worldRes.WorldMap.GetLength(1))
            {
                sp.Right = chunkEdges; // false
            }
            else
            {
                if (worldRes.WorldMap[(int)chunkPosInWorldArray.Y, (int)chunkPosInWorldArray.X + 1].ChunkMap[(int)blockPos.z, (int)blockPos.y, 0] == 0)
                {
                    sp.Right = true;
                }
                else
                {
                    sp.Right = false;
                }

            }
        }
        else
        {
            if (worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z, (int)blockPos.y, (int)blockPos.x + 1] == 0) { sp.Right = true; }
        }

        if (blockPos.y + 1 >= CMSize.Y)
        {
            if (chunkPosInWorldArray.Y + 1 >= worldRes.WorldMap.GetLength(0))
            {
                sp.Forward = chunkEdges; // false
            }
            else
            {
                if (worldRes.WorldMap[(int)chunkPosInWorldArray.Y + 1, (int)chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z , 0 , (int)blockPos.x] == 0)
                {
                    sp.Forward = true;
                }
                else
                {
                    sp.Forward = false;
                }

            }
        }
        else
        {
            if (worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z, (int)blockPos.y + 1, (int)blockPos.x] == 0) { sp.Forward = true; }
        }

        if (blockPos.z + 1 >= CMSize.Z)
        {
            sp.Up = false;
        }
        else
        {
            if (worldRes.WorldMap[chunkPosInWorldArray.Y, chunkPosInWorldArray.X].ChunkMap[(int)blockPos.z + 1, (int)blockPos.y, (int)blockPos.x] == 0) { sp.Up = true; }
        }
        return sp;
    }
}

public enum SideType 
{ 
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down,
    Default
}

