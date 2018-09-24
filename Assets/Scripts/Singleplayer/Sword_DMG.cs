using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    public class Sword_DMG : MonoBehaviour
    {
        public readonly int MinDmg;
        public readonly int MaxDmg;
        private System.Random random = new System.Random();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player" && other.GetComponent<PlatformerCharacter2D>().NumberOfPlayer != transform.parent.GetComponent<PlatformerCharacter2D>().NumberOfPlayer)
            {
                other.GetComponent<PlatformerCharacter2D>().TakeHp(random.Next(MinDmg, MaxDmg));
            }
        }
    }
}
