using UnityEngine;
using Unity.Mathematics;
using Game.DesignPattern;
namespace Game.Mono
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed;

        private Camera _camera;


        private void OnEnable() {
            GetMainCamera();
        }

        private void LateUpdate() {
            _camera.transform.position = math.lerp(_camera.transform.position, _targetTransform.position + _offset , _smoothSpeed * Time.deltaTime);
        }

        public void GetMainCamera() {
            _camera = Camera.main;
        }

        public void SetTargetPos(float3 position) {
            _targetTransform.position = position;
        }

        private void ImmediatelyMove() {
            _camera.transform.position = _targetTransform.position + _offset;
        }
    }
}
