using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VoxelSampleManager : MonoBehaviour {
    
    //Dimensions of the world
    public static float width   = 15f;
    public static float height  = 8f;

    //Size of each voxel in our grid
    public float voxelSize;
    public float threshold;

    //Center of the grid/world
    public Vector2 center;

    //Array which stores the samples
    public float[,] voxelSamples;

    public Material material;

    public bool DebugDraw;

    private int numVoxelsX;
    private int numVoxelsY;
    public List<CircularSampleable> sampleableElements;


    MeshFilter _mf;
    MeshRenderer _mr;

    MarchingSquaresMeshGenerator _meshGen;
    

    void Awake()
    {
        numVoxelsX = Mathf.CeilToInt(width / voxelSize);
        numVoxelsY = Mathf.CeilToInt(height / voxelSize);

        voxelSamples = new float[numVoxelsY, numVoxelsX];

        _mf = gameObject.AddComponent<MeshFilter>();
        _mr = gameObject.AddComponent<MeshRenderer>();

        _mr.material = material;

        _meshGen = new MarchingSquaresMeshGenerator();
    }

    void Start()
    {
        
    }
    public Vector2 GetVoxelCenter(int x, int y)
    {
        Vector2 worldTopLeft = center - new Vector2(width / 2, -height / 2) + new Vector2(voxelSize / 2, -voxelSize / 2);
        return worldTopLeft + new Vector2(x * voxelSize, -y * voxelSize);

    }



    void _debugDrawVoxels()
    {
        for (int y = 0; y < numVoxelsY; y++)
        {
            for (int x = 0; x < numVoxelsX; x++)
            {
                Vector3 center = GetVoxelCenter(x,y);
                Vector3 size = new Vector3(voxelSize *0.5f , voxelSize*0.5f, voxelSize*0.5f);
                Gizmos.color = Color.white;
                if (voxelSamples[y, x] > threshold) Gizmos.color = Color.black;
                Gizmos.DrawCube(center, size);
                
                

            }
        }
    }

    void _debugDrawSquares()
    {
        for (int y = 0; y < numVoxelsY - 1 ; y++)
        {
            for (int x = 0; x < numVoxelsX - 1; x++)
            {
                Vector3 center = GetVoxelCenter(x, y);
                new Square(x, y, this).DebugDraw();


            }
        }
    }


    void _debugDrawSamples()
    {
        for (int y = 0; y < numVoxelsY; y++)
        {
            for (int x = 0; x < numVoxelsX; x++)
            {
                Vector3 center = GetVoxelCenter(x, y);
                center.y *= -1;
                Vector2 labelPos = Camera.main.WorldToScreenPoint(center);
                GUI.Label(new Rect(labelPos.x - 20, labelPos.y, 100, 100), Math.Truncate(100 * voxelSamples[y, x]) / 100 + " ");

                //Square sq = new Square(x, y, this);
                //int val = sq.GetSquareIndex(0.0f);
               // Vector3 sqCenter = sq.Center;
                //sqCenter.y*=-1;
                //Vector2 sqLabelPos = Camera.main.WorldToScreenPoint(sqCenter);
               // GUI.Label(new Rect(sqLabelPos.x - 20, sqLabelPos.y, 100, 100), val + " ");


                //GUI.Label(new Rect(labelPos.x, labelPos.y, 100, 100), Math.Truncate(100 * voxelSamples[y, x]) / 100 + " ");
                //GUI.Label(new Rect(labelPos.x- 20, labelPos.y, 100, 100), _trunc(center));
            }
        }
    }

    string _truncStringify(Vector2 f)
    {
        float x = (float) Math.Truncate(100 * f.x)/100;
        float y = (float) Math.Truncate(100 * f.y)/100;
        return "{" + x + "," + y + "}";
    }
    

    void OnDrawGizmos()
    {
        if (DebugDraw)
        {
            _debugDrawVoxels();
        }
        
    }

    void OnGUI()
    {
        if (DebugDraw)
        {
            _debugDrawSamples();
        }
    }

    void SampleVoxels()
    {
        for (int y = 0; y < numVoxelsY; y++)
        {
            for (int x = 0; x < numVoxelsX; x++)
            {
                float total = 0f;
                foreach (CircularSampleable _circle in sampleableElements)
                {
                    float sample = _circle.GetSampleAt(GetVoxelCenter(x, y));
                    
                    //if( sample >= threshold)    
                        total += sample;
                                      
                }
                //total = sampleableElements.Count;
                voxelSamples[y, x] = total;

            }
        }
    }


    void GenerateMesh()
    {

        _meshGen.Reset();

        for (int y = 0; y < numVoxelsY - 1; y++)
        {
            for (int x = 0; x < numVoxelsX - 1; x++)
            {
                Square sq = new Square(x, y, this);
                _meshGen.AddSquareToMesh(sq, threshold);
            }
        }
        _meshGen.Generate(_mf);
    }

	// Update is called once per frame
	void Update () {

        SampleVoxels();
        GenerateMesh();
        // _debugDrawSquares();
	}
}
