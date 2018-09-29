using UnityEngine;

namespace Assets.Scripts.SinglePlayer.Camera
{
    public class CameraNavigation : MonoBehaviour
    {
        public Transform Target;
        public float Damping = 1;
        public float LookAheadFactor = 3;
        public float LookAheadReturnSpeed = 0.5f;
        public float LookAheadMoveThreshold = 0.1f;

        private float offsetZ;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPosition;

        private void Start()
        {
            if (Target != null)
            {
                lastTargetPosition = Target.position;
                offsetZ = (transform.position - Target.position).z;
            }
            else
            {
                Debug.LogWarning("Camera Target is missing ");
            }
        }

        private void Update()
        {
            CameraNavigationSystem();
        }

        public void CameraNavigationSystem()
        {
            CalculateLookAheadPosition();
            transform.position = CalculateNewPosition(CalculateAheadTargetPosition());
            lastTargetPosition = Target.position;
        }

        private void CalculateLookAheadPosition()
        {
            var xMoveDelta = (Target.position - lastTargetPosition).x;
            var updateLookAheadTarget = Mathf.Abs(xMoveDelta) > LookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                lookAheadPosition = LookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                lookAheadPosition = Vector3.MoveTowards(lookAheadPosition, Vector3.zero, Time.deltaTime * LookAheadReturnSpeed);
            }
        }

        private Vector2 CalculateAheadTargetPosition()
        {
            return Target.position + lookAheadPosition + Vector3.forward * offsetZ;
        }

        private Vector3 CalculateNewPosition(Vector2 aheadTargetPosition)
        {
            var target = new Vector3(aheadTargetPosition.x, aheadTargetPosition.y, transform.position.z);
            return Vector3.SmoothDamp(transform.position, target, ref currentVelocity, Damping);
        }
    }
}
