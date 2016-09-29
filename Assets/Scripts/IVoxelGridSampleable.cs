using UnityEngine;
using System.Collections;


public interface IVoxelGridSampleable  {

     float GetSampleAt(Vector2 voxelCenter);
}
