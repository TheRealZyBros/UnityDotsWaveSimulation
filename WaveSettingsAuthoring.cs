using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public class WaveSettingsAuthoring : MonoBehaviour
{
    public float Speed = 1, Height = 3, Smoothness = 50;
}
public struct WaveSettings : IComponentData
{ 
    public float Speed, Height, Smoothness;
}
public class WaveSettingsBaker : Baker<WaveSettingsAuthoring>
{
    public override void Bake(WaveSettingsAuthoring authoring)
    {
        AddComponent(GetEntity(TransformUsageFlags.Dynamic), new WaveSettings { Speed = authoring.Speed, Height = authoring.Height, Smoothness = authoring.Smoothness});
    }
}