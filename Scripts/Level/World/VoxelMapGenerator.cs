using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class VoxelMapGenerator : MonoBehaviour
{
    public int ChunksRenderDistance = 5;  
    public byte HillsHeight = 16;
    public int HillsScale = 6;
    public byte GrassStartPosition = 120;
    public WorldResources worldResources;
    private Chunk chunk;
    private Vector2 ChunkArea = new Vector2(16 , 16);

   
    void Start()
    {
        HillsHeight = (byte)Random.Range(2 , HillsHeight);
        HillsScale = Random.Range(1, HillsScale);
        worldResources.WorldMap = new Chunk[ChunksRenderDistance, ChunksRenderDistance];
        GenerateWorld(ChunksRenderDistance , new Point(Random.Range(500 , 999999) , Random.Range(500, 999999)));
        //chunk = new Chunk(PerlinNoizeGen.GetPerlinHeightMap(new Point(Random.Range(1, 10000), Random.Range(1, 10000)), new Size((int)ChunkArea.x, (int)ChunkArea.y), HillsScale) , 120 , HillsHeight , worldResources , transform.position);        
    }
    public void GenerateWorld(int chunksCount , Point worldStartPos)
    {
        for (var y = 0; y < chunksCount; y ++) 
        {
            for (var x = 0; x < chunksCount; x++)
            {
                worldResources.WorldMap[y, x] = new Chunk(PerlinNoizeGen.GetPerlinHeightMap(new Point((int)(x * HillsScale + worldStartPos.X),(int)(y * HillsScale + worldStartPos.Y)), new Size((int)ChunkArea.x, (int)ChunkArea.y), HillsScale), GrassStartPosition, HillsHeight, worldResources, new Vector3(transform.position.x + (x * ChunkArea.x), transform.position.y, transform.position.z + (y * ChunkArea.y)) , new Point(x , y));
                worldResources.WorldMap[y, x].GenerateChunkMap();
            }
        }

        for (var y = 0; y < chunksCount; y++)
        {
            for (var x = 0; x < chunksCount; x++)
            {
                worldResources.WorldMap[y, x].CreateMeshes();
                worldResources.WorldMap[y, x].SetMaterialMaps();              
                worldResources.WorldMap[y, x].SetupMeshes();
            }
        }
        
    }
    
}
public class ArrayDimensions
{
    public int X = 0;
    public int Y = 0;
    public int Z = 0;

    public ArrayDimensions(int z, int y, int x)
    {
        X = x;
        Y = y;
        Z = z;
    }
}
public class BlockMap 
{
    public List<SideParams> Map;

    public BlockMap(List<SideParams> resoursMap)
    {
        this.Map = resoursMap;
    }
    public BlockMap()
    {
        this.Map = new List<SideParams>();
    }
    public int FindAdressInListByPosition(Vector3 position)
    {
        Debug.LogWarning(position.ToString());
        int counter = 0;
        foreach (var i in Map)
        {
            Debug.Log(i.Position.ToString());
            if (i.Position.x == position.x && i.Position.y == position.y && i.Position.z == position.z)
            {
                Debug.LogWarning(position.ToString() + " / " + counter);
                return counter;
            }
            counter++;
        }
        return 0;
    }
}

public class Chunk
{
    public int[,,] ChunkMap;   
    public Mesh[] MaterialMeshes;
    public BlockMap[] MaterialMaps;
    public Vector3 Position;
    public Point IndexInWorldArray;
    public ArrayDimensions Size
    {
        get { return size; }
    }
    private ArrayDimensions size;

    private WorldResources worldResources;
    private HeightMap GrassMap;
    private GameObject[] ChunkParts;
    private byte surfaceHeight = 20;
    private byte hillsHeight = 6;
    public Chunk(HeightMap grassMap , byte surfaceHeight , byte hillsHeight , WorldResources resources , Vector3 chunkPosition , Point indexInWorldArray) 
    {
        IndexInWorldArray = indexInWorldArray;
        Position = chunkPosition;
        this.worldResources = resources;
        GrassMap = grassMap;
        this.surfaceHeight = surfaceHeight;
        if (surfaceHeight + hillsHeight < 256)
        {
            this.hillsHeight = hillsHeight;
        }
        else
        {
            this.hillsHeight = (byte)(255 - surfaceHeight);
        }
        //GenerateChunkMap();
        //CreateMeshes();
        //SetMaterialMaps();
        //EditMeshesOfMatMaps();
        //SetupMeshes();
    }
    
    public void ClearMaterialMaps() 
    {
        foreach (var i in MaterialMaps) 
        {
            i.Map.Clear();
        }
    }
    public void SetMaterialMaps()
    {
        
        for (var z = Size.Z - 1; z >= 0; z--)
        {

            for (var y = 0; y < Size.Y; y++)
            {
                for (var x = 0; x < Size.X; x++)
                {
                    byte ID = (byte)ChunkMap[z, y, x];
                    if (ID != 0)
                    {
                        SideParams sp = SideParams.GetSideParamsFromBlock(worldResources , IndexInWorldArray , new Vector3(x, y, z));
                        sp.ID = ID;
                        sp.Position = new Vector3(Position.x + x, Position.y + z, Position.z + y);
                        if (sp.IsFullDisabled() == false)
                        {
                            MaterialMaps[ID - 1].Map.Add(sp);
                        }
                                                          
                    }
                }
            }

        }      

    }
    public ArrayDimensions GetChunkMapSize() 
    {
        return new ArrayDimensions(ChunkMap.GetLength(0), ChunkMap.GetLength(1), ChunkMap.GetLength(2));       
    }
    public void GenerateChunkMap()
    {
        ChunkMap = new int[255, GrassMap.GetSize().Height, GrassMap.GetSize().Width];
        size = new ArrayDimensions(255, GrassMap.GetSize().Height, GrassMap.GetSize().Width);
             
        for (var z = size.Z - 1; z >= 0; z--)
        {
            if (z < surfaceHeight + hillsHeight)
            {
                for (var y = 0; y < size.Y; y++)
                {
                    for (var x = 0; x < size.X; x++)
                    {
                        if (z < surfaceHeight)
                        {
                            ChunkMap[z, y, x] = 3;                            
                        }
                        else
                        {
                             //Debug.Log($"i = {surfaceHeight + (int)((float)(GrassMap.Map[y, x] / 255f) * hillsHeight) - 1}; z = {z} z = {z + 1}}");
                            if (surfaceHeight + (int)((float)(GrassMap.Map[y, x] / 255f) * hillsHeight) == z)
                            {
                                ChunkMap[z, y, x] = 1;
                            }
                            else
                            {
                                if (ChunkMap[z + 1, y, x] != 0)
                                {

                                    ChunkMap[z, y, x] = 2;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public static Chunk[] SetStructure(ref WorldResources worldRes, BlockStructure structure)
    {
        List<Chunk> modifiedChunks = new List<Chunk>();
        int x = ((int)Mathf.Ceil(16 * ((structure.BuildPointer.x / 16) - ((int)(structure.BuildPointer.x / 16))) + 0.5f) - 1);
        int Height = (int)Mathf.Ceil(255 * ((structure.BuildPointer.y / 255) - ((int)(structure.BuildPointer.y / 255))) + 0.5f) - 1;
        int z = (int)Mathf.Ceil(16 * ((structure.BuildPointer.z / 16) - ((int)(structure.BuildPointer.z / 16))) + 0.5f) - 1;
        int chunkX = (int)(structure.BuildPointer.x / 16f);
        int chunkZ = (int)(structure.BuildPointer.z / 16f);   

        Chunk selectedChunk;
     
        Vector3 spawnPointer;
        for (int i = 0; i < structure.Structure.Length; i++) 
        {
            int chunkMx = 0;
            int chunkMz = 0;
            spawnPointer = new Vector3((structure.Structure[i].WorldPos.x + x), (structure.Structure[i].WorldPos.y + Height), (structure.Structure[i].WorldPos.z + z));
            if (spawnPointer.x >= 16)
            {
                chunkMx += (int)(spawnPointer.x / 16f);
                
                spawnPointer.x = spawnPointer.x - 16;
            }
            if (spawnPointer.z >= 16)
            {
                spawnPointer.z = spawnPointer.z - 16;
                chunkMz += (int)(spawnPointer.z / 16f);              
                
            }

            if (spawnPointer.x < 0)
            {
                spawnPointer.x = -(spawnPointer.x - 16);
                chunkMx -= (int)(Mathf.Abs(spawnPointer.x) / 16f);
               
               
                //Debug.Log($"{Mathf.Abs(spawnPointer.x)} / 16 = {chunkX + chunkMx} ; {spawnPointer.x}");
            }
            if (spawnPointer.z < 0)
            {
                spawnPointer.z = -(spawnPointer.z - 16);
                chunkMz -= (int)(Mathf.Abs(spawnPointer.z) / 16f);
                
                
            }
            if ((chunkZ + chunkMz) < 0 || (chunkZ + chunkMz) >= worldRes.VMG.ChunksRenderDistance || (chunkX + chunkMx) < 0 || (chunkX + chunkMx) >= worldRes.VMG.ChunksRenderDistance) 
            {
                continue;
            }
            
            selectedChunk = worldRes.WorldMap[chunkZ + chunkMz, chunkX + chunkMx];
            
            try
            {
                selectedChunk.ChunkMap[(int)spawnPointer.y, (int)spawnPointer.z, (int)spawnPointer.x] = structure.Structure[i].ID;
            }
            catch 
            {
                Debug.Log($"{spawnPointer.ToString()}");
            }
            if (!IsChunkInList(ref modifiedChunks, ref selectedChunk) == true)
            {
                modifiedChunks.Add(selectedChunk);
            }
        }
        return modifiedChunks.ToArray();
    }
    private static bool IsChunkInList(ref List<Chunk> chunkList , ref Chunk chunk) 
    {
        foreach (var i in chunkList) 
        {
            if (i.Position == chunk.Position) 
            {
                return true;
            }
        }
        return false;
    }
    public void CreateMeshes()
    {
        int resLength = worldResources.Resources.Length;
        MaterialMeshes = new Mesh[resLength];
        MaterialMaps = new BlockMap[resLength];
        ChunkParts = new GameObject[resLength];

        for (var i = 0; i < worldResources.Resources.Length; i++)
        {
            MaterialMaps[i] = new BlockMap();
            ChunkParts[i] = new GameObject();
            ChunkParts[i].tag = "Chunk";
            MeshFilter meshFilter = ChunkParts[i].AddComponent(typeof(MeshFilter)) as MeshFilter;
            MeshRenderer meshRenderer = ChunkParts[i].AddComponent(typeof(MeshRenderer)) as MeshRenderer;
            meshFilter.mesh = new Mesh();
            
            MaterialMeshes[i] = meshFilter.mesh;
            ChunkParts[i].GetComponent<Renderer>().material = worldResources.Resources[i].Material;
            ChunkParts[i].AddComponent(typeof(MeshCollider));
           // ChunkParts[i].AddComponent(typeof(GetUVInfo));
        }
    }
    public void SetupMeshes()
    {        
        for (var i = 0; i < worldResources.Resources.Length; i++)
        {
            MaterialMeshes[i] = OptimizedBox.GetMeshFromBlockMap(MaterialMaps[i].Map.ToArray());
            ChunkParts[i].GetComponent<MeshFilter>().mesh = MaterialMeshes[i];
            ChunkParts[i].GetComponent<MeshCollider>().sharedMesh = null;
            ChunkParts[i].GetComponent<MeshCollider>().sharedMesh = MaterialMeshes[i];
        }
    }
    public void UpdateChunk()
    {
        ClearMaterialMaps();
        SetMaterialMaps();
        SetupMeshes();
    }
  
}