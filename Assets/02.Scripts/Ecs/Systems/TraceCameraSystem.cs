using Unity.Entities;
using Unity.NetCode;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using System;
using Game.Mono;
using Codice.CM.Client.Differences;

namespace Game.Ecs
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial class TraceCameraSystem : SystemBase
    {
        private Action<float3> traceCamera_handler;

        protected override void OnCreate() {
            RequireForUpdate<TraceCameraTag>();
        }

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
