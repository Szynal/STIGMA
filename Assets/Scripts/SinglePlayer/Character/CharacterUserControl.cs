using UnityEngine;

namespace Assets.Scripts.SinglePlayer.Character
{
    public class CharacterUserControl : MonoBehaviour
    {
        private CharacterPhysics physics;

        private void Awake()
        {
            physics = GetComponent<CharacterPhysics>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                physics.Jump();
            }
        }

        private void FixedUpdate()
        {
            physics.Move(Input.GetAxis("Horizontal"));
        }
    }
}
