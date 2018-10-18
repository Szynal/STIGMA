using Assets.Scripts.UI.Input;
using UnityEngine;

namespace Assets.Scripts.SinglePlayer.Character
{
    [RequireComponent(typeof(CharacterPhysics))]
    public class CharacterUserControl : MonoBehaviour
    {
        private CharacterPhysics physics;
        private bool jumpInput;
        private float moveDelta;

        private void Awake()
        {
            physics = GetComponent<CharacterPhysics>();
        }

        private void Update()
        {
            UserControl();
        }

        private void UserControl()
        {
            moveDelta = GetHorizontalAxisRaw();
            jumpInput = Input.GetKey(InputManager.Input.Jump);
        }

        private static float GetHorizontalAxisRaw()
        {
            if (Input.GetKey(InputManager.Input.Left))
            {
                return -1;
            }
            return Input.GetKey(InputManager.Input.Right) ? 1 : 0;
        }

        private void FixedUpdate()
        {
            physics.Move(moveDelta, jumpInput);
        }
    }
}
