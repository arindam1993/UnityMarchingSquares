﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MarchingSquaresMeshGenerator  {

    //the generated mesh object
    Mesh _generatedMesh;

    //Veretx and triangle buffers
    Vector3[] vertices;
    int[] triangles;

    int vertexCount = 0;
    int triCount = 0;

    //Temporary vertex index map
    int[] _currVertIndices;


    private VertexIndexCache vertCache;

    public MarchingSquaresMeshGenerator(){
        _generatedMesh = new Mesh();
        vertices = new Vector3[VoxelSampleManager.NumVoxelsY * 100 / VoxelSampleManager.NUM_THREADS];
        triangles = new int[3* VoxelSampleManager.NumVoxelsY * 100 / VoxelSampleManager.NUM_THREADS];
        _currVertIndices = new int[8];

        vertCache = new VertexIndexCache();
        Reset();
    }

    //Clear the buffers and reset counters, use this at the start of teh frame
    public void Reset()
    {
        _generatedMesh.Clear();
        Array.Clear(vertices, 0, vertexCount);
        Array.Clear(triangles,0, triCount);
        vertexCount = 0;
        triCount = 0;
    }

    //Use this once all the squares have been parsed to copy the buffers over for rendering
    public void Generate(MeshFilter mf)
    {
        _generatedMesh.vertices = vertices;
        _generatedMesh.triangles = triangles;
        mf.mesh = _generatedMesh;

        vertCache.EndOfFrame();
    }

    //Add a square to the mesh, this method looks up the configuration and calls the utility parser function
    public void AddSquareToMesh(Square square, float threshold)
    {
        SquareMeshConfig sqConfig = SquareMeshConfig.GetMeshConfig(square.GetSquareIndex(threshold));
        parseConfig(square, sqConfig, threshold);
    }

    //Utility function to add a triangle
    private void addTriangle(int v1, int v2, int v3)
    {
        triangles[triCount] = v1;
        triCount++;

        triangles[triCount] = v2;
        triCount++;

        triangles[triCount] = v3;
        triCount++;
    }

    //Utility function to add a vertex
    private void addVertex(Vector3 v)
    {
        vertices[vertexCount] = v;
        vertexCount++;
    }

    //This parses the config and generates the vertices
    private void parseConfig(Square square, SquareMeshConfig sqConfig, float threshold)
    {

        Array.Clear(_currVertIndices, 0, 8);
        byte cachedVertMask = vertCache.TryGetCachedIndices(square.X, sqConfig, _currVertIndices);
        //Since each SquareMeshConfig contains vertex indices for each square in isolation, we use the current vertex count to offset the indices.

        for (int vert_i = 0; vert_i < 8; vert_i++)
        {

            bool isVertCached = ((0x01 << vert_i) & cachedVertMask) > 0;
            
            if (sqConfig.isVertex(vert_i) && !isVertCached)
            {
                SquareVertexType sqVert = (SquareVertexType)vert_i;

                Vector3 vertPos = Vector3.zero;

                //Maps a SquareVertexType to a position from the Square
                switch (sqVert)
                {
                    case SquareVertexType.TopLeft:
                        vertPos = square.TopLeftPos;
                        break;
                    case SquareVertexType.TopRight:
                        vertPos = square.TopRightPos;
                        break;
                    case SquareVertexType.BottomRight:
                        vertPos = square.BtmRightPos;
                        break;
                    case SquareVertexType.BottomLeft:
                        vertPos = square.BtmLeftPos;
                        break;
                    case SquareVertexType.TopCenter:
                        vertPos = square.TopEdgePos;
                        break;
                    case SquareVertexType.RightCenter:
                        vertPos = square.RightEdgePos;
                        break;
                    case SquareVertexType.BottomCenter:
                        vertPos = square.BtmEdgePos;
                        break;
                    case SquareVertexType.LeftCenter:
                        vertPos = square.LeftEdgePos;
                        break;
                    default: break;
                }

                _currVertIndices[vert_i] = vertexCount;
                addVertex(vertPos);


                vertCache.SetIndicesAt(square.X, _currVertIndices);
            }

        }


        List<Triangle> sqTris = sqConfig.triangles;
        for (int tri_i = 0; tri_i < sqTris.Count; tri_i++)
        {
            Triangle sqTri = sqTris[tri_i];

            addTriangle(
                    _currVertIndices[sqTri.v1],
                    _currVertIndices[sqTri.v2],
                    _currVertIndices[sqTri.v3]
                );
        }
    }

    public void StartNewRow() {

        vertCache.StartNewRow();
    }
}
