using UnityEngine;

namespace Assets.Scripts.Singleplayer.Character
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Character : MonoBehaviour
    {
        public Sprite MainSkin;

        private void Start()
        {
            SetCharacterMainSprite();
        }

        private void SetCharacterMainSprite()
        {
            if (MainSkin != null)
            {
                GetComponent<SpriteRenderer>().sprite = MainSkin;
            }
            else
            {
                Debug.LogWarning("MainSkin Sprite is missing");
            }
        }

    }
}
