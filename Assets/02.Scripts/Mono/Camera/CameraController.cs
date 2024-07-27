using UnityEngine;
using Unity.Mathematics;
namespace Game.Mono
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed;

        private Camera _camera;
        

        private void Awake() {
            GetMainCamera();
        }

        private void LateUpdate() {
            _camera.transform.position = math.lerp(_camera.transform.position, _targetTransform.position + _offset , _smoothSpeed * Time.deltaTime);
        }

        public void GetMainCamera() {
            _camera = Camera.main;
        }

        private void SetTargetPos(float3 position) {
            _targetTransform.position = position;
        }

        private void ImmediatelyMove() {
            _camera.transform.position = _targetTransform.position + _offset;
        }
    }
}
