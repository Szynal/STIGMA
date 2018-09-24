using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    public class PlayZone_Function : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == ("Player"))
            {
                PlatformerCharacter2D player = other.GetComponent<PlatformerCharacter2D>();
                player.TakeHp(1000);
            }
            else if (other.tag == "Spells")
            {
                Destroy(other.gameObject);
            }
        }
    }
}
