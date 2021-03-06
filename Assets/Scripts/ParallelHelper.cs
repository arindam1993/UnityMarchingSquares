﻿using UnityEngine;
using System.Collections;
using System.Threading;

public delegate void ParallelForDelegate(int currIndex, int threadIndex);
public delegate void OnForFinishedDelegate(int threadIndex);

public class ParallelHelper  {

	public int ThreadCount { get; private set; }

    private Thread[] threadPool;

    private ParallelForDelegate forDelegate;
    private OnForFinishedDelegate onFinish;

    struct ThreadIndexData
    {
        public int Low;
        public int High;
        public int Step;
        public int ThreadIndex;
    }

    public ParallelHelper(int threadCount)
    {
        this.ThreadCount = threadCount;
        this.threadPool = new Thread[threadCount];

        for(int threadIndex=0; threadIndex < threadCount; threadIndex++)
        {
            this.threadPool[threadIndex] = new Thread(
                    new ParameterizedThreadStart(threadTask)
                );
        }
    }

    public void For(int start, int end, int step, ParallelForDelegate del, OnForFinishedDelegate fin)
    {
        this.forDelegate = del;
        this.onFinish = fin;

        int rem = (end - start) % this.ThreadCount;
        int perThread = (end - start) / this.ThreadCount;

        for (int threadIndex = 0; threadIndex < this.ThreadCount; threadIndex++)
        {
            int forStart = perThread * threadIndex;
            int forEnd = forStart + perThread;

            if (threadIndex == this.ThreadCount - 1)
            {
                forEnd += rem;
            }
            ThreadIndexData thIndices = new ThreadIndexData
            {
                Low = forStart,
                High = forEnd,
                Step = step,
                ThreadIndex = threadIndex
            };
            this.threadPool[threadIndex] = new Thread(
                    new ParameterizedThreadStart(threadTask)
                );
            this.threadPool[threadIndex].Start(thIndices);
           // Debug.Log("Strarting thread: " + threadIndex);
        }

        for (int threadIndex = 0; threadIndex < this.ThreadCount; threadIndex++)
        {
            //Debug.Log("Joining thread: " + threadIndex);
            this.threadPool[threadIndex].Join();
    
        }
      
    }

    private void threadTask(object thIndicesData) {

        ThreadIndexData indices = (ThreadIndexData)thIndicesData;

        int low = indices.Low;
        int high = indices.High;
        int step = indices.Step;
        int threadIndex = indices.ThreadIndex;    
        for( int currIndex=low; currIndex < high; currIndex += step)
        {
            forDelegate(currIndex, threadIndex);
        }

        if( this.onFinish != null)
            this.onFinish(threadIndex);
    }
}
