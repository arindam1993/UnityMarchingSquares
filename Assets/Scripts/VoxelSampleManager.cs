using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

public class VoxelSampleManager : MonoBehaviour {
    
    public static int NUM_THREADS = 2;

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
    private Vector2[] _sampleableCenters;

    MeshFilter[] _mf;
    MeshRenderer[] _mr;

    MarchingSquaresMeshGenerator[] _meshGen;
    ParallelHelper _parallelMeshGenerator;


    ParallelHelper _parallelSampler;



    int runCtr;
    Stopwatch timePerSample;
    long totalTime;
    void Awake()
    {
        NumVoxelsX = Mathf.CeilToInt(width / voxelSize);
        NumVoxelsY = Mathf.CeilToInt(height / voxelSize);
        UnityEngine.Debug.Log(NumVoxelsY);


        initMeshGeneration();





        voxelSamples = new float[NumVoxelsY, NumVoxelsX];
        _sampleableCenters = new Vector2[sampleableElements.Count];
        _parallelSampler = new ParallelHelper(NUM_THREADS);

        runCtr = 0;
        timePerSample = new Stopwatch();
        totalTime = 0;
    }

    void Start()
    {
        
    }
    public Vector2 GetVoxelCenter(int x, int y)
    {
        Vector2 worldTopLeft = center - new Vector2(width / 2, -height / 2) + new Vector2(voxelSize / 2, -voxelSize / 2);
        return worldTopLeft + new Vector2(x * voxelSize, -y * voxelSize);

    }

    void initMeshGeneration()
    {
        _mf = new MeshFilter[NUM_THREADS];
        _mr = new MeshRenderer[NUM_THREADS];
        _meshGen = new MarchingSquaresMeshGenerator[NUM_THREADS];
        _parallelMeshGenerator = new ParallelHelper(NUM_THREADS);

        for(int childIndex = 0; childIndex < NUM_THREADS; childIndex++)
        {
            GameObject child = new GameObject("MeshChild" + childIndex);
            child.transform.parent = this.transform;
            child.transform.localPosition = Vector3.zero;

            _mf[childIndex] = child.AddComponent<MeshFilter>();
            _mr[childIndex] = child.AddComponent<MeshRenderer>();
            _mr[childIndex].sharedMaterial = material;

            _meshGen[childIndex] = new MarchingSquaresMeshGenerator();

        }

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

            _sampleableCenters[sampleIndex] = sample;
            sampleIndex++;

        }

        _parallelSampler.For(0, NumVoxelsY, 1, (y, threadIndex) =>
        {
            for (int x = 0; x < NumVoxelsX; x++)
            {
                
                float total = 0f;
                for( int i = 0; i < _sampleableCenters.Length; i++)
                {
                    Vector2 vox = GetVoxelCenter(x, y);
                    Vector2 samp = _sampleableCenters[i];
                    total += sampleableElements[0].GetSampleAt(vox, samp);
                }
                voxelSamples[y, x] = total;

            }
        },null);

      
    }


    void GenerateMesh()
    {
        timePerSample.Reset();
        timePerSample.Start();
        for (int genIndex = 0; genIndex < NUM_THREADS; genIndex++)
        {
            _meshGen[genIndex].Reset();
        }

        _parallelMeshGenerator.For(0, NumVoxelsY - 1, 1, (y, threadIndex) =>
            {
                for (int x = 0; x < NumVoxelsX - 1; x++)
                {
                    Square sq = new Square(x, y, this);
                    _meshGen[threadIndex].AddSquareToMesh(sq, threshold);
                }
                _meshGen[threadIndex].StartNewRow();
            },null);

        for (int genIndex = 0; genIndex < NUM_THREADS; genIndex++)
        {
            _meshGen[genIndex].Generate(_mf[genIndex]);
        }
        timePerSample.Stop();
        totalTime += timePerSample.ElapsedMilliseconds;
        runCtr++;
    }

	// Update is called once per frame
	void Update () {
        
        SampleVoxels();
        GenerateMesh();
    

        if (runCtr == 10000)
        {
            UnityEngine.Debug.Log("TimeTaken :" + totalTime);
        }
        //Debug.Break();
        // _debugDrawSquares();
    }
}
