using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface surface;
    void Start()
    {
        surface.BuildNavMesh();
    }

}
