using UnityEngine;

namespace Assets.Scripts.SinglePlayer.Character
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Character : MonoBehaviour
    {
        public Sprite MainSkin;

        private SpriteRenderer spriteRenderer;
        private readonly CharacterState characterState;

        public enum CharacterState
        {
            Idle,
            Run,
            Jump
        }

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetCharacterMainSprite();
        }

        private void SetCharacterMainSprite()
        {
            if (MainSkin != null)
            {
                spriteRenderer.sprite = MainSkin;
            }
            else
            {
                Debug.LogWarning("MainSkin Sprite is missing");
            }
        }
    }
}
