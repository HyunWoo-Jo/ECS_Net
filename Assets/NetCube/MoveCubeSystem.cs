using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Collections;
using Unity.Burst;
using Unity.Physics;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
[BurstCompile]
public partial struct MoveCubeSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        var builder = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<Simulate>()
            .WithAll<CubeInput>()

            .WithAllRW<LocalTransform>();

        var query = state.GetEntityQuery(builder);
        state.RequireForUpdate(query);
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var moveJob = new MoveCubeJob {
            tick = SystemAPI.GetSingleton<NetworkTime>().ServerTick,
            fixedCubeSpeed = SystemAPI.Time.DeltaTime
        };
        state.Dependency = moveJob.ScheduleParallel(state.Dependency);
    }

    [BurstCompile]
    [WithAll(typeof(Simulate))]
    partial struct MoveCubeJob : IJobEntity
    {
        public NetworkTick tick;
        public float fixedCubeSpeed;


        private void Execute(CubeInput moveRefRO, RefRW<PhysicsVelocity> velocityRefRW) {
            float3 direciton = new float3(moveRefRO.Horizontal, 0, moveRefRO.Vertical);
            float3 accel = math.normalizesafe(direciton, float3.zero) * fixedCubeSpeed * 30;
            float3 liner = velocityRefRW.ValueRO.Linear;
            float3 maxVelocity = 10;
            liner = math.clamp(liner + accel, -maxVelocity, maxVelocity);
            if (direciton.x == 0) liner.x = 0;
            if (direciton.z == 0) liner.z = 0;
            velocityRefRW.ValueRW.Linear = liner;
        }

    }
}
