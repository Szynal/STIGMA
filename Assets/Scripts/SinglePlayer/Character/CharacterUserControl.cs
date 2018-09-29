using UnityEngine;

namespace Assets.Scripts.SinglePlayer.Character
{
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
            moveDelta = Input.GetAxisRaw("Horizontal");
            jumpInput = Input.GetKeyDown(KeyCode.Space);
        }

        private void FixedUpdate()
        {
            physics.Move(moveDelta, jumpInput);
        }
    }
}
