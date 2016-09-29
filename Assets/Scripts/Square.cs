using UnityEngine;
using System.Collections;

public struct Square
{
    //Corner sample values
    public float TopLeftSample { get; private set; }
    public float TopRightSample { get; private set; }
    public float BtmLeftSample { get; private set; }
    public float BtmRightSample { get; private set; }

    //Corner positions
    public Vector2 TopLeftPos { get; private set; }
    public Vector2 TopRightPos { get; private set; }
    public Vector2 BtmLeftPos { get; private set; }
    public Vector2 BtmRightPos { get; private set; }

    //Edge point positions
    public Vector2 LeftEdgePos
    {
        get
        {
            return getLerpEdgePt(TopLeftPos, TopLeftSample, BtmLeftPos, BtmLeftSample);
        }
    }
    public Vector2 TopEdgePos
    {
        get
        {
            return getLerpEdgePt(TopLeftPos, TopLeftSample, TopRightPos, TopRightSample);
        }
    }
    public Vector2 RightEdgePos
    {
        get
        {
            return getLerpEdgePt(TopRightPos, TopRightSample, BtmRightPos, BtmRightSample);
        }
    }
    public Vector2 BtmEdgePos
    {
        get
        {
            return getLerpEdgePt(BtmLeftPos, BtmLeftSample, BtmRightPos, BtmRightSample);
        }
    }

    public Vector2 Center
    {
        get
        {
            return (TopLeftPos + TopRightPos + BtmLeftPos + BtmRightPos) / 4.0f;
        }
    }



    public Square(int x, int y, VoxelSampleManager map)
    {

        float[,] grid = map.voxelSamples;
        int width = grid.GetLength(1);
        int height = grid.GetLength(0);

        if (x == width || y == height) Debug.LogError("You cannot get a square at the edge of the voxel map");
        //store samples
        this.TopLeftSample = grid[y, x];
        this.TopRightSample = grid[y, x + 1];
        this.BtmLeftSample = grid[y + 1, x];
        this.BtmRightSample = grid[y + 1, x + 1];

        //store corner positions from the map
        this.TopLeftPos = map.GetVoxelCenter(x, y);
        this.TopRightPos = map.GetVoxelCenter(x + 1, y);
        this.BtmLeftPos = map.GetVoxelCenter(x, y + 1);
        this.BtmRightPos = map.GetVoxelCenter(x + 1, y + 1);
    }

    //Bit-masking logic which generates the index to reference to the appripriate SquareMeshCOnfig
    public int GetSquareIndex(float threshold)
    {
        int topLeftVal = (TopLeftSample > threshold) ? 1 : 0;
        int topRightVal = (TopRightSample > threshold) ? (1 << 1) : 0;
        int btmRightVal = (BtmRightSample > threshold) ? (1 << 2) : 0;
        int btmLeftVal = (BtmLeftSample > threshold) ? (1 << 3) : 0;

        return topLeftVal | topRightVal | btmRightVal | btmLeftVal;
    }

    public void DebugDraw()
    {
        Debug.DrawLine(this.TopLeftPos, this.TopRightPos, Color.green);
        Debug.DrawLine(this.TopRightPos, this.BtmRightPos, Color.green);
        Debug.DrawLine(this.BtmRightPos, this.BtmLeftPos, Color.green);
        Debug.DrawLine(this.BtmLeftPos, this.TopLeftPos, Color.green);
    }

    //Calculates linearly interpolated edge points.
    private Vector2 getLerpEdgePt(Vector2 v1, float s1, Vector2 v2, float s2)
    {
        float fracFromv1 = (1.0f - s1) / (s2 - s1);
        return v1 + (v2 - v1) * fracFromv1;
    }



}