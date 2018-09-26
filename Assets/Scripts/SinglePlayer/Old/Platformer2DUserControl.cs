using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        public bool Jumping { get; set; }

        public string GetAxis;
        public string Jump;

        private PlatformerCharacter2D character;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!Jumping)
            {
                // Read the jump input in Update so button presses aren't missed.
                Jumping = Input.GetButtonDown(Jump);
            }
        }

        private void FixedUpdate()
        {
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = Input.GetAxis(GetAxis);
            // Pass all parameters to the character control script.

            character.Move(h, crouch, Jumping);
            Jumping = false;
        }
    }
}
