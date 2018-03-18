using System;
using UnityEngine;


namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D _Character;
        private bool _Jumping;

        [SerializeField] private String _GetAxis;   //   INPUT  
        [SerializeField] private String _Jump;      //   INPUT 

        private void Awake()
        {
            _Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!_Jumping)
            {
                // Read the jump input in Update so button presses aren't missed.
                _Jumping = Input.GetButtonDown(_Jump);
            }
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = Input.GetAxis(_GetAxis);
            // Pass all parameters to the character control script.
            
            _Character.Move(h, crouch, _Jumping);
            _Jumping = false;
        }
    }
}
