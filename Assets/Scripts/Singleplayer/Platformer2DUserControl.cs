using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jumping;

        public string GetAxis;
        public string Jump;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!jumping)
            {
                // Read the jump input in Update so button presses aren't missed.
                jumping = Input.GetButtonDown(Jump);
            }
        }

        private void FixedUpdate()
        {
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = Input.GetAxis(GetAxis);
            // Pass all parameters to the character control script.

            character.Move(h, crouch, jumping);
            jumping = false;
        }
    }
}
