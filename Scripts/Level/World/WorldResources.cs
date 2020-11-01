using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldResources : MonoBehaviour
{
    public Resource[] Resources;
    public Chunk[,] WorldMap;
    public List<BlockStructure> Structures;
    public VoxelMapGenerator VMG;
    public void Start()
    {
        
            
    }
}



[System.Serializable]
public class Resource
{
    public string Name;
    public Texture2D Texture;
    public Material Material;  
}