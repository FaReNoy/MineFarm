using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Block 
{
    public Vector3 WorldPos;
    public int ID;

    public Block(Vector3 worldPos, int iD)
    {
        WorldPos = worldPos;
        ID = iD;
    }
}
[System.Serializable]
public class BlockStructure 
{
    public Block[] Structure;
    public Vector3 BuildPointer;
    public string Name;

    public BlockStructure(Block[] structure, Vector3 buildPointer, string name = "default")
    {
        Structure = structure;
        BuildPointer = buildPointer;
        Name = name;
    }
    
}