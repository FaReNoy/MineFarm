using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PerlinNoizeGen : MonoBehaviour
{

    public static Texture2D GetPerlinNoize(Point startPosition , Size textureSize , float perlinScale = 6f) 
    {      
        Texture2D noiseTex = new Texture2D(textureSize.Width, textureSize.Height);
        UnityEngine.Color[] pix;       
        pix = new UnityEngine.Color[noiseTex.width * noiseTex.height];
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = startPosition.X + x / noiseTex.width * perlinScale;
                float yCoord = startPosition.Y + y / noiseTex.height * perlinScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new UnityEngine.Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        return noiseTex;
    }
    public static HeightMap GetPerlinHeightMap(Point startPosition, Size textureSize, float perlinScale = 6f)
    {
        HeightMap noiseHMap = new HeightMap(textureSize);    
        
        float y = 0;

        while (y < noiseHMap.GetSize().Height)
        {
            float x = 0;
            while (x < noiseHMap.GetSize().Width)
            {
                float xCoord = (float)startPosition.X + x / (float)(textureSize.Width) * (float)perlinScale;
                float yCoord = (float)startPosition.Y + y / (float)(textureSize.Height) * (float)perlinScale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                noiseHMap.Map[(int)y, (int)x] = (byte)(sample * 255f);

                //Debug.Log($"{sample} * 255f = {noiseHMap.Map[(int)y, (int)x]}; {startPosition.X} + {x} / {textureSize.Width} * {perlinScale} ,{startPosition.Y} + {y} / {textureSize.Height} * {perlinScale} = {xCoord}, {yCoord}");
                x++;
            }
            y++;
        }

          
        return noiseHMap;
    }
}
public class HeightMap
{
    public byte[,] Map;
    private Size Size;

    public HeightMap(Size hMapSize) 
    {
        this.Size = hMapSize;
        Map = new byte[hMapSize.Height , hMapSize.Width];
    }

    public HeightMap(Texture2D image)
    {
        this.Size = new Size(image.height, image.width);
        Map = new byte[image.height , image.width];
        for (var y = 0; y < image.height; y++)
        {
            for (var x = 0; x < image.width; x++)
            {
                byte pixelBrightnes = (byte)((image.GetPixel(x, y).r + image.GetPixel(x, y).g + image.GetPixel(x, y).b) / 3);
                Map[y, x] = pixelBrightnes;
            }
        }
    }
    public Size GetSize()
    {
        return Size;
    }
}

