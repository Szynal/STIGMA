using UnityEngine;

namespace Assets.Scripts.SinglePlayer.Character
{
    public class CharacterUserControl : MonoBehaviour
    {
        private CharacterPhysics physics;
        private bool jumpInput;
        private Vector3 moveDelta;
        private void Awake()
        {
            physics = GetComponent<CharacterPhysics>();
        }

        private void Update()
        {
            moveDelta = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            jumpInput = Input.GetKey(KeyCode.Space);
        }

        private void FixedUpdate()
        {
            physics.Move(moveDelta, jumpInput);
        }
    }
}
