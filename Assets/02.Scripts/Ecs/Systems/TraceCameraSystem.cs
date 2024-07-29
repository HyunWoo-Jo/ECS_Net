using Unity.Entities;
using Unity.NetCode;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using System;
using Game.Mono;

namespace Game.Ecs
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial class TraceCameraSystem : SystemBase
    {
        private Action<float3> traceCamera_handler;
        [BurstCompile]
        protected override void OnUpdate() {
            if (traceCamera_handler == null) {
                traceCamera_handler = CameraController.Instance.SetTargetPos;
            }
            foreach (var transform in SystemAPI.Query<LocalTransform>().WithAll<TraceCameraTag, GhostOwnerIsLocal>()) {
                traceCamera_handler?.Invoke(transform.Position);

            }
        }
    }
}
