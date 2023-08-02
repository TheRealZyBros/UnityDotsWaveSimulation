using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
public partial struct EntitiesSystem : ISystem
{
    private bool Once, Once2;

    public void OnUpdate(ref SystemState state)
    {
        int EntitiesToMake = 160000;
        int WidthLength;
        float Size = 0.04f;

        if (!Once)
        {
            CubeRef cubeRef = SystemAPI.GetSingleton<CubeRef>();

            for (int i = 0; i < EntitiesToMake / 5; i++)
            {
                JobHandle handle = new SpawnJob { cubeRef = cubeRef, CommandBuffer = GetCommandBuffer(state) }.ScheduleParallel(state.Dependency);
                handle.Complete();
            }

            Once = true;
        }
        if (!Once2)
        {
            EntityQuery query = state.EntityManager.CreateEntityQuery(typeof(LocalTransform));

            WidthLength = (int)math.sqrt(EntitiesToMake);

            int x = 0, z = 0;
            if(query.CalculateEntityCount() > EntitiesToMake)
            {
                foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>())
                {
                    transform.ValueRW.Scale = Size;

                    transform.ValueRW.Position = new float3(x * Size, 0, z * Size);
                    if (x != WidthLength - 1) x++;
                    else
                    {
                        x = 0;
                        z++;
                    }
                }
                Once = true;
            }
        }

        new MoveJob {ElapsedTime = (float)SystemAPI.Time.ElapsedTime}.ScheduleParallel();
    }
    [BurstDiscard]
    EntityCommandBuffer.ParallelWriter GetCommandBuffer(SystemState state) 
    {
        return SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.World.Unmanaged).AsParallelWriter();
    }
}
[BurstCompile]
public partial struct SpawnJob : IJobEntity 
{
    public CubeRef cubeRef;

    public EntityCommandBuffer.ParallelWriter CommandBuffer;
    public void Execute([ChunkIndexInQuery] int Index) 
    {
        CommandBuffer.Instantiate(Index, cubeRef.Cube);
    }
}

public partial struct MoveJob : IJobEntity 
{
    public float ElapsedTime;

    public void Execute(EntitiesAspect aspect) 
    {
        aspect.MoveEntity(ElapsedTime);
    }
}

