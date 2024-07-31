using Unity.Entities;
using Unity.NetCode;
using Unity.Burst;   
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

namespace Game.Ecs
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    [BurstCompile]
    public partial class InputSystem : SystemBase, InputActionData.IPlayerStandardActions {
        private InputActionData _inputActionData;

        private float _inputUp;
        private float _inputRight;

        // InputActionData callback
        public void OnMoveUp(InputAction.CallbackContext context) {
            _inputUp = context.ReadValue<float>();
        }
        // InputActionData callback
        public void OnMoveRight(InputAction.CallbackContext context) {
            _inputRight = context.ReadValue<float>();
        }

        [BurstCompile]
        protected override void OnCreate() {
            base.OnCreate();
            // init
            _inputActionData = new InputActionData();
            _inputActionData.PlayerStandard.SetCallbacks(this);
            _inputActionData.PlayerStandard.Enable();
        }
        [BurstCompile]
        protected override void OnDestroy() {
            base.OnDestroy();
            _inputActionData.PlayerStandard.Disable();
            _inputActionData.Dispose();
        }

        [BurstCompile]
        protected override void OnUpdate() {
            foreach (var moveInput in SystemAPI.Query<RefRW<CubeInput>>().WithAll<GhostOwnerIsLocal>()) {
                moveInput.ValueRW.Horizontal = _inputRight;
                moveInput.ValueRW.Vertical = _inputUp;
            }
        }

 

    }
    
}
