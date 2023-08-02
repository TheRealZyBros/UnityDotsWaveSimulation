using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CubeRefAuthoring : MonoBehaviour
{
    public GameObject Cube;
}
public struct CubeRef : IComponentData
{
    public Entity Cube;
}
public class CubeRefBaker : Baker<CubeRefAuthoring>
{
    public override void Bake(CubeRefAuthoring authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), new CubeRef { Cube = GetEntity(authoring.Cube, TransformUsageFlags.Dynamic) });
    }
}