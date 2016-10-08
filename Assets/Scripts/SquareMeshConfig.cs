using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum SquareVertexType
{
    TopLeft         = 0,
    TopCenter       = 1,
    TopRight        = 2,
    RightCenter     = 3,
    BottomRight     = 4,
    BottomCenter    = 5,
    BottomLeft      = 6,
    LeftCenter      = 7
}

//Utility Triangle class
public struct Triangle
{
    public int v1;
    public int v2;
    public int v3;
}

public class SquareMeshConfig {

    //The static lookup table
    private static SquareMeshConfig[] configMap;

    //A 8-bit flag where the LSB denotes the top left corner and goes clockwise from there to the MSB
    public byte VertexFlags { get; private set; }

    //List of triangles
    public List<Triangle> triangles;

    public SquareMeshConfig()
    {
        triangles = new List<Triangle>();
        VertexFlags = 0x00;
    }

    public static void InitConfigMap()
    {
        configMap = new SquareMeshConfig[16];

        for (int i = 0; i < 16; i++)
        {
            configMap[i] = SquareMeshConfig.CreateMeshConfig(i);
        }
    }

    public static SquareMeshConfig GetMeshConfig(int squareIndex)
    {
        if (configMap == null)
        {
            SquareMeshConfig.InitConfigMap();
        }

        return configMap[squareIndex];
    }

    //Creates a mesh config for a specified index
    private static SquareMeshConfig CreateMeshConfig(int squareIndex)
    {
        SquareMeshConfig config = new SquareMeshConfig();

        //Add three vertexTypes to create a triangle
        //AddTriangle automatically keeps track of dulicates using the temp dictionary and creates vertex indices accordingly
        switch (squareIndex)
        {
            case 1:
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.LeftCenter
                    );
                break;
            case 2:
                config.AddTriangle(
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.RightCenter
                    );
                break;
            case 3:
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.RightCenter,
                        (int)SquareVertexType.LeftCenter
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.RightCenter
                    );
                break;
            case 4:
                config.AddTriangle(
                        (int)SquareVertexType.BottomCenter,
                        (int)SquareVertexType.RightCenter,
                        (int)SquareVertexType.BottomRight
                    );
                break;
            case 5:
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.RightCenter
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.RightCenter,
                        (int)SquareVertexType.BottomRight
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.BottomRight,
                        (int)SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.BottomCenter,
                        (int)SquareVertexType.LeftCenter
                    );
                break;
            case 6:
                config.AddTriangle(
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.BottomRight,
                        (int)SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.BottomRight
                    );
                break;
            case 7:
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.BottomRight,
                        (int)SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.BottomCenter,
                        (int)SquareVertexType.LeftCenter
                    );  
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.BottomRight
                    );
                break;
            case 8:
                config.AddTriangle(
                       (int)SquareVertexType.LeftCenter,
                       (int)SquareVertexType.BottomCenter,
                       (int)SquareVertexType.BottomLeft
                   );
                break;
            case 9:
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.BottomCenter,
                        (int)SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.BottomCenter
                    );
                break;
            case 10:
                config.AddTriangle(
                        (int)SquareVertexType.LeftCenter,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        (int)SquareVertexType.BottomLeft,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.TopRight
                    );
                config.AddTriangle(
                        (int)SquareVertexType.BottomLeft,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        (int)SquareVertexType.BottomCenter,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.RightCenter
                    );
                break;
            case 11:
                config.AddTriangle(
                        (int)SquareVertexType.BottomLeft,
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopRight
                    );
                config.AddTriangle(
                        (int)SquareVertexType.BottomLeft,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        (int)SquareVertexType.BottomCenter,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.RightCenter
                    );
                break;
            case 12:
                config.AddTriangle(
                        (int)SquareVertexType.BottomLeft,
                        (int)SquareVertexType.LeftCenter,
                        (int)SquareVertexType.BottomRight
                    );
                config.AddTriangle(
                        (int)SquareVertexType.LeftCenter,
                        (int)SquareVertexType.RightCenter,
                        (int)SquareVertexType.BottomRight
                    );
                break;
            case 13:
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.BottomRight,
                        (int)SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.BottomRight
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.RightCenter,
                        (int)SquareVertexType.BottomRight
                    );
                break;
            case 14:
                 config.AddTriangle(
                        (int)SquareVertexType.LeftCenter,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        (int)SquareVertexType.BottomLeft,
                        (int)SquareVertexType.TopCenter,
                        (int)SquareVertexType.TopRight
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.BottomRight,
                        (int)SquareVertexType.BottomLeft
                    );
                break;
            case 15:
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.BottomRight,
                        (int)SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        (int)SquareVertexType.TopLeft,
                        (int)SquareVertexType.TopRight,
                        (int)SquareVertexType.BottomRight
                    );
                break;
             default: break;
        }

        return config;
    }


    private void AddTriangle(int _v1, int _v2, int _v3)
    {
        Triangle newTri = new Triangle
        {
            v1 = _v1,
            v2 = _v2,
            v3 = _v3
        };

        addVertex(_v1);
        addVertex(_v2);
        addVertex(_v3);

        triangles.Add(newTri);

    }

    private bool addVertex(int vertIndex)
    {
        byte mask = (byte)(0x01 << vertIndex);
        if( (mask & VertexFlags) == 0)
        {
            VertexFlags |= mask;
            return true;
        }

        return false;
    }

    public bool isVertex(int vertIndex)
    {
        byte mask = (byte)(0x01 << vertIndex);
        if ((mask & VertexFlags) == 0)
        {
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        string toRet = "{ vertices: [\n";
        toRet += Convert.ToString(VertexFlags,2).PadLeft(8, '0');
        toRet += "] , triangles : [ \n";
        foreach (Triangle tri in triangles)
        {
            toRet += string.Format(" [{0}, {1} , {2}] ,", tri.v1, tri.v2, tri.v3);
        }
        toRet += "]}";
        return toRet;
    }


    public static void Test()
    {
        
        for (int i = 0; i < 16; i++)
        {
            Debug.Log(SquareMeshConfig.GetMeshConfig(i));
        }
    }
}
