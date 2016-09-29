using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//Types of vertices in each square
public enum SquareVertexType
{
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft,
    TopCenter,
    RightCenter,
    BottomCenter,
    LeftCenter
}

//Utility Triangle class
public class Triangle
{
    public int v1;
    public int v2;
    public int v3;
}

public class SquareMeshConfig {



   
    //The static lookup table
    private static SquareMeshConfig[] configMap;

    //List of vertices
    public List<SquareVertexType> vertices;
    //Temp dictionary used to make adding triangles to a config less manual
    Dictionary<SquareVertexType, int> vertexIndices;
    //List of triangles
    public List<Triangle> triangles;


    public SquareMeshConfig()
    {
        vertices = new List<SquareVertexType>(6);
        vertexIndices = new Dictionary<SquareVertexType, int>();
        triangles = new List<Triangle>();

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
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopCenter,
                        SquareVertexType.LeftCenter
                    );
                break;
            case 2:
                config.AddTriangle(
                        SquareVertexType.TopCenter,
                        SquareVertexType.TopRight,
                        SquareVertexType.RightCenter
                    );
                break;
            case 3:
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.RightCenter,
                        SquareVertexType.LeftCenter
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopRight,
                        SquareVertexType.RightCenter
                    );
                break;
            case 4:
                config.AddTriangle(
                        SquareVertexType.BottomCenter,
                        SquareVertexType.RightCenter,
                        SquareVertexType.BottomRight
                    );
                break;
            case 5:
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopCenter,
                        SquareVertexType.RightCenter
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.RightCenter,
                        SquareVertexType.BottomRight
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.BottomRight,
                        SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.BottomCenter,
                        SquareVertexType.LeftCenter
                    );
                break;
            case 6:
                config.AddTriangle(
                        SquareVertexType.TopCenter,
                        SquareVertexType.BottomRight,
                        SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        SquareVertexType.TopCenter,
                        SquareVertexType.TopRight,
                        SquareVertexType.BottomRight
                    );
                break;
            case 7:
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.BottomRight,
                        SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.BottomCenter,
                        SquareVertexType.LeftCenter
                    );  
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopRight,
                        SquareVertexType.BottomRight
                    );
                break;
            case 8:
                config.AddTriangle(
                       SquareVertexType.LeftCenter,
                       SquareVertexType.BottomCenter,
                       SquareVertexType.BottomLeft
                   );
                break;
            case 9:
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.BottomCenter,
                        SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopCenter,
                        SquareVertexType.BottomCenter
                    );
                break;
            case 10:
                config.AddTriangle(
                        SquareVertexType.LeftCenter,
                        SquareVertexType.TopCenter,
                        SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        SquareVertexType.BottomLeft,
                        SquareVertexType.TopCenter,
                        SquareVertexType.TopRight
                    );
                config.AddTriangle(
                        SquareVertexType.BottomLeft,
                        SquareVertexType.TopRight,
                        SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        SquareVertexType.BottomCenter,
                        SquareVertexType.TopRight,
                        SquareVertexType.RightCenter
                    );
                break;
            case 11:
                config.AddTriangle(
                        SquareVertexType.BottomLeft,
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopRight
                    );
                config.AddTriangle(
                        SquareVertexType.BottomLeft,
                        SquareVertexType.TopRight,
                        SquareVertexType.BottomCenter
                    );
                config.AddTriangle(
                        SquareVertexType.BottomCenter,
                        SquareVertexType.TopRight,
                        SquareVertexType.RightCenter
                    );
                break;
            case 12:
                config.AddTriangle(
                        SquareVertexType.BottomLeft,
                        SquareVertexType.LeftCenter,
                        SquareVertexType.BottomRight
                    );
                config.AddTriangle(
                        SquareVertexType.LeftCenter,
                        SquareVertexType.RightCenter,
                        SquareVertexType.BottomRight
                    );
                break;
            case 13:
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.BottomRight,
                        SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopCenter,
                        SquareVertexType.BottomRight
                    );
                config.AddTriangle(
                        SquareVertexType.TopCenter,
                        SquareVertexType.RightCenter,
                        SquareVertexType.BottomRight
                    );
                break;
            case 14:
                 config.AddTriangle(
                        SquareVertexType.LeftCenter,
                        SquareVertexType.TopCenter,
                        SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        SquareVertexType.BottomLeft,
                        SquareVertexType.TopCenter,
                        SquareVertexType.TopRight
                    );
                config.AddTriangle(
                        SquareVertexType.TopRight,
                        SquareVertexType.BottomRight,
                        SquareVertexType.BottomLeft
                    );
                break;
            case 15:
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.BottomRight,
                        SquareVertexType.BottomLeft
                    );
                config.AddTriangle(
                        SquareVertexType.TopLeft,
                        SquareVertexType.TopRight,
                        SquareVertexType.BottomRight
                    );
                break;
             default: break;
        }

        return config;
    }


    public void AddTriangle(SquareVertexType _v1, SquareVertexType _v2, SquareVertexType _v3)
    {
        //Initial vertex indices
        int v1_i = -1;
        int v2_i = -1;
        int v3_i = -1;

        //Check id specifed vertex is already added to Config
        bool v1Found = vertexIndices.TryGetValue(_v1, out v1_i);
        bool v2Found = vertexIndices.TryGetValue(_v2, out v2_i);
        bool v3Found = vertexIndices.TryGetValue(_v3, out v3_i);

        //If any vertex is already added then use its index from the dictionary, else append it to the end of the vertices array and update that new index into the temp dictionary.

        if ( !v1Found) 
        {

            v1_i = vertices.Count;
            vertices.Add(_v1);
            vertexIndices.Add(_v1, v1_i);
        }

        if ( !v2Found)
        {
            v2_i = vertices.Count;
            vertices.Add(_v2);
            vertexIndices.Add(_v2, v2_i);
        }

        if ( !v3Found)
        {
            v3_i = vertices.Count;
            vertices.Add(_v3);
            vertexIndices.Add(_v3, v3_i);
        }
        
        triangles.Add(new Triangle{
            v1 = v1_i,
            v2 = v2_i,
            v3 = v3_i
        });
    }

    public override string ToString()
    {
        string toRet = "{ vertices: [\n";
        foreach (SquareVertexType vert in vertices)
        {
            toRet += vert.ToString() + ",\n";
        }
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
