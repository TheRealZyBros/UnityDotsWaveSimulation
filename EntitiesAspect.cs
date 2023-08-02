using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
public readonly partial struct EntitiesAspect : IAspect
{
    private readonly RefRW<LocalTransform> transform;
    private readonly RefRO<WaveSettings> waveSettings;

    public void MoveEntity(float ElapsedTime)
    {
        float y = math.sin(ElapsedTime * waveSettings.ValueRO.Speed + (transform.ValueRO.Position.x + transform.ValueRO.Position.z) * waveSettings.ValueRO.Smoothness) * waveSettings.ValueRO.Height;
        transform.ValueRW.Position = new float3(transform.ValueRO.Position.x, y, transform.ValueRO.Position.z);
    }
}
