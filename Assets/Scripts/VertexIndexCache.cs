using UnityEngine;
using System;
using System.Collections;

public class VertexIndexCache
{
    int[,] prevRowCache;
    int[,] currRowCache;

    bool firstRowCached;


    public VertexIndexCache()
    {
        prevRowCache = new int[VoxelSampleManager.NumVoxelsX, 8];
        currRowCache = new int[VoxelSampleManager.NumVoxelsX, 8];
        initCache(prevRowCache);
        initCache(currRowCache);

        firstRowCached = false;
    }

    public void StartNewRow()
    {
        Array.Copy(currRowCache, prevRowCache, currRowCache.Length);
        initCache(currRowCache);
        firstRowCached = true;

    }

    public void EndOfFrame()
    {
        firstRowCached = false;
    }

    private void initCache(int[,] row)
    {

        for( int x=0; x < row.GetLength(0); x++)
        {
            for (int y= 0; y < row.GetLength(1); y++)
            {
                row[x, y] = -1;
            }
        }
    }

    public void SetIndicesAt(int sqX, int[] indices)
    {
        int startindex = sqX;
        for( int i=0; i < 8; i++)
        {
            currRowCache[sqX, i] = indices[i];
        }
    }



    public byte TryGetCachedIndices(int sqX, SquareMeshConfig sqCfg, int[] indices)
    {

        

        byte toRet = 0x00;
        if (firstRowCached)
        {
            #region TOP_SIDE_MATCHES
            //Top Left Corner match
            if (sqCfg.isVertex(0))
            {

                int index = prevRowCache[sqX, 6];

                if (index != -1)
                {
                    indices[0] = index;
                    byte mask = (byte)(0x01 << 0);
                    toRet |= mask;
                }
            }

            //Top Center match
            if (sqCfg.isVertex(1))
            {

                int index = prevRowCache[sqX, 5];

                if (index != -1)
                {
                    indices[1] = index;
                    byte mask = (byte)(0x01 << 1);
                    toRet |= mask;
                }
            }

            //Top Right match
            if (sqCfg.isVertex(2))
            {

                int index = prevRowCache[sqX, 4];

                if (index != -1)
                {
                    indices[2] = index;
                    byte mask = (byte)(0x01 << 2);
                    toRet |= mask;
                }
            }
            #endregion
        }

        if (sqX > 0)
        {
            #region LEFT_SIDE_MATCHES
            //Bottom left match
            if (sqCfg.isVertex(6))
            {

                int index = currRowCache[sqX - 1, 4];

                if (index != -1)
                {
                    indices[6] = index;
                    byte mask = (byte)(0x01 << 6);
                    toRet |= mask;
                }
            }


            //Left center match
            if (sqCfg.isVertex(7))
            {

                int index = currRowCache[sqX - 1, 3];

                if (index != -1)
                {
                    indices[7] = index;
                    byte mask = (byte)(0x01 << 7);
                    toRet |= mask;
                }
            }
            #endregion
        }
        return toRet;
    }


  
}
