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

    public static int NumVoxelsX;
    public static int NumVoxelsY;
    public List<CircularSampleable> sampleableElements;
    private Vector2[] samples;

    MeshFilter _mf;
    MeshRenderer _mr;

    MarchingSquaresMeshGenerator _meshGen;

    ParallelHelper _parallelSampler;
    

    void Awake()
    {
        NumVoxelsX = Mathf.CeilToInt(width / voxelSize);
        NumVoxelsY = Mathf.CeilToInt(height / voxelSize);

        voxelSamples = new float[NumVoxelsY, NumVoxelsX];

        _mf = gameObject.AddComponent<MeshFilter>();
        _mr = gameObject.AddComponent<MeshRenderer>();

        _mr.material = material;

        samples = new Vector2[sampleableElements.Count];

        _meshGen = new MarchingSquaresMeshGenerator();

        _parallelSampler = new ParallelHelper(4);
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
        for (int y = 0; y < NumVoxelsY; y++)
        {
            for (int x = 0; x < NumVoxelsX; x++)
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
        for (int y = 0; y < NumVoxelsY - 1 ; y++)
        {
            for (int x = 0; x < NumVoxelsX - 1; x++)
            {
                Vector3 center = GetVoxelCenter(x, y);
                new Square(x, y, this).DebugDraw();
            }

        }
    }


    void _debugDrawSamples()
    {
        for (int y = 0; y < NumVoxelsY; y++)
        {
            for (int x = 0; x < NumVoxelsX; x++)
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
        int sampleIndex = 0;
        foreach (CircularSampleable _circle in sampleableElements)
        {
            Vector2 sample = _circle.transform.position;

            samples[sampleIndex] = sample;
            sampleIndex++;

        }
        _parallelSampler.For(0, NumVoxelsY, 1, (y) =>
        {
            for (int x = 0; x < NumVoxelsX; x++)
            {
                
                float total = 0f;
                for( int i = 0; i < samples.Length; i++)
                {
                    Vector2 vox = GetVoxelCenter(x, y);
                    Vector2 samp = samples[i];
                    total += sampleableElements[0].GetSampleAt(vox, samp);
                }
                //total = sampleableElements.Count;
                voxelSamples[y, x] = total;

            }
        });
        //for (int y = 0; y < NumVoxelsY; y++)
        //{
            
       // }
    }


    void GenerateMesh()
    {

        _meshGen.Reset();

        for (int y = 0; y < NumVoxelsY - 1; y++)
        {
            for (int x = 0; x < NumVoxelsX - 1; x++)
            {
                Square sq = new Square(x, y, this);
                _meshGen.AddSquareToMesh(sq, threshold);
            }
            _meshGen.StartNewRow();
        }
        _meshGen.Generate(_mf);
    }

	// Update is called once per frame
	void Update () {

        SampleVoxels();
        GenerateMesh();
        //Debug.Break();
        // _debugDrawSquares();
	}
}
