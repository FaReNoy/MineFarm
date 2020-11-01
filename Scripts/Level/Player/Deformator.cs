using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;


public class Deformator : MonoBehaviour
{
   
    
    public static void InstantiateBlockToChunk(BlockSelectingType typeOfInstance , byte instantiateBlockId , ref Vector3 globalPointer , ref VoxelMapGenerator _VMG) 
    {
        globalPointer = GetPointerPos(globalPointer, typeOfInstance);
        ArrayDimensions pointerInChunckMap = GetCorrectBlockPos(globalPointer);
        Point chunkIndex = GetChunkIndexFromPointer(globalPointer, pointerInChunckMap);
        Chunk selectedChunk = _VMG.worldResources.WorldMap[chunkIndex.Y, chunkIndex.X];
        selectedChunk.ChunkMap[pointerInChunckMap.Y, pointerInChunckMap.Z, pointerInChunckMap.X] = instantiateBlockId;
        selectedChunk.UpdateChunk();
        TryToUpdateNeighborChuncks(pointerInChunckMap, chunkIndex, ref _VMG);        
    }
    private static Point GetChunkIndexFromPointer(Vector3 globalPointer , ArrayDimensions pointerInChunckMap) 
    {
        int chunkX = (int)(globalPointer.x / 16f);
        int chunkZ = (int)(globalPointer.z / 16f);
        if (pointerInChunckMap.X >= 16)
        {
            chunkX++;
            pointerInChunckMap.X = 0;
        }
        if (pointerInChunckMap.Z >= 16)
        {
            chunkZ++;
            pointerInChunckMap.Z = 0;
        }
        return new Point(chunkX , chunkZ);
    }
    private static Vector3 GetPointerPos(Vector3 pointer, BlockSelectingType type = BlockSelectingType.ThisBlock) // Возвращает позицию блока в чанке
    {
        var heading = pointer - Camera.main.transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;
        return pointer - ((direction / 100) * (int)type);
    } 
    
    private static void TryToUpdateNeighborChuncks(ArrayDimensions pointer , Point currentChunckIndex , ref VoxelMapGenerator VMG)
    {     
        if (pointer.X == 0)
        {
            if (currentChunckIndex.X - 1 >= 0)
            {
                VMG.worldResources.WorldMap[currentChunckIndex.Y, currentChunckIndex.X - 1].UpdateChunk();              
            }
        }

        if (pointer.X == 15)
        {
            if (currentChunckIndex.X + 1 < VMG.ChunksRenderDistance)
            {
                VMG.worldResources.WorldMap[currentChunckIndex.Y, currentChunckIndex.X + 1].UpdateChunk();               
            }
        }

        if (pointer.Z == 0)
        {
            if (currentChunckIndex.Y - 1 >= 0)
            {
                VMG.worldResources.WorldMap[currentChunckIndex.Y - 1, currentChunckIndex.X].UpdateChunk();            
            }
        }

        if (pointer.Z == 15)
        {
            if (currentChunckIndex.Y + 1 < VMG.ChunksRenderDistance)
            {
                VMG.worldResources.WorldMap[currentChunckIndex.Y + 1, currentChunckIndex.X].UpdateChunk();             
            }
        }
    }
   
   
    public static ArrayDimensions GetCorrectBlockPos(Vector3 pointer ) 
    {
        
        int x = ((int)Mathf.Ceil(16 * ((pointer.x / 16) - ((int)(pointer.x / 16))) + 0.5f) - 1);
        int Height = (int)Mathf.Ceil(255 * ((pointer.y / 255) - ((int)(pointer.y / 255))) + 0.5f) - 1;
        int z = (int)Mathf.Ceil(16 * ((pointer.z / 16) - ((int)(pointer.z / 16))) + 0.5f) - 1;
        return new ArrayDimensions(z , Height , x);
    }
    public static void AddStructureToChunk(ref VoxelMapGenerator _VMG , int structureID , Vector3 spawnPoint)
    {
        _VMG.worldResources.Structures[structureID].BuildPointer = spawnPoint;
        var chunks = Chunk.SetStructure(ref _VMG.worldResources, _VMG.worldResources.Structures[structureID]);
        foreach (var i in chunks)
        {
           i.UpdateChunk();
        }
    }

}
public enum BlockSelectingType 
    { 
        ThisBlock = -1,                   // блок на который указывает рейкаст
        OutOfThisBlock = 1               // блок сосед нормали на которую указывает рейкаст
    }