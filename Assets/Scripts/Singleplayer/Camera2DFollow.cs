using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform Target;
        public float Damping = 1;
        public float LookAheadFactor = 3;
        public float LookAheadReturnSpeed = 0.5f;
        public float LookAheadMoveThreshold = 0.1f;

        private float offsetZ;
        private Vector3 lastTargetPosition;
        private Vector3 currentVelocity;
        private Vector3 lookAheadPos;

        private void Start()
        {
            lastTargetPosition = Target.position;
            offsetZ = (transform.position - Target.position).z;
            transform.parent = null;
        }

        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            var xMoveDelta = (Target.position - lastTargetPosition).x;
            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > LookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                lookAheadPos = LookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * LookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = Target.position + lookAheadPos + Vector3.forward * offsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, Damping);

            transform.position = newPos;
            lastTargetPosition = Target.position;
        }
    }
}
