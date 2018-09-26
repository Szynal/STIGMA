using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    public class GameMaster : MonoBehaviour
    {
        public List<int> ListSpell1 = new List<int>();
        public List<int> ListSpell2 = new List<int>();

        public PlatformerCharacter2D Player1 = new PlatformerCharacter2D();
        public PlatformerCharacter2D Player2 = new PlatformerCharacter2D();

        private void Start()
        {
            ListSpell1.Clear();
            ListSpell2.Clear();
        }

        private void Update()
        {
            Player1 = transform.GetChild(0).GetComponent<PlatformerCharacter2D>();
            if (Player1.Hp <= 0) Player1.transform.gameObject.SetActive(false);

            Player2 = transform.GetChild(1).GetComponent<PlatformerCharacter2D>();
            if (Player2.Hp <= 0) Player2.transform.gameObject.SetActive(false);
        }
    }
}
