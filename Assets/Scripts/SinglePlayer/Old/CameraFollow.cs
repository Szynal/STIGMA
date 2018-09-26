using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    public class CameraFollow : MonoBehaviour
    {
        public float XMargin = 1f; // Distance in the x axis the player can move before the camera follows.
        public float YMargin = 1f; // Distance in the y axis the player can move before the camera follows.
        public float CameraSmoothAxisX = 8f;
        public float CameraSmoothAxisY = 8f;
        public Vector2 CameraMaxCoordinates;
        public Vector2 CameraMinCoordinates;

        private Transform player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private bool CheckXMargin()
        {
            // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
            return Mathf.Abs(transform.position.x - player.position.x) > XMargin;
        }

        private bool CheckYMargin()
        {
            // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
            return Mathf.Abs(transform.position.y - player.position.y) > YMargin;
        }

        private void Update()
        {
            TrackPlayer();
        }

        private void TrackPlayer()
        {
            // By default the target x and y coordinates of the camera are it's current x and y coordinates.
            var targetX = transform.position.x;
            var targetY = transform.position.y;

            // If the player has moved beyond the x margin...
            if (CheckXMargin())
            {
                // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
                targetX = Mathf.Lerp(transform.position.x, player.position.x, CameraSmoothAxisX * Time.deltaTime);
            }

            // If the player has moved beyond the y margin...
            if (CheckYMargin())
            {
                // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
                targetY = Mathf.Lerp(transform.position.y, player.position.y, CameraSmoothAxisY * Time.deltaTime);
            }

            // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
            targetX = Mathf.Clamp(targetX, CameraMinCoordinates.x, CameraMaxCoordinates.x);
            targetY = Mathf.Clamp(targetY, CameraMinCoordinates.y, CameraMaxCoordinates.y);

            // Set the camera's position to the target position with the same z component.
            transform.position = new Vector3(targetX, targetY, transform.position.z);
        }
    }
}
