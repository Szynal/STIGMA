using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    public class Spell_2 : MonoBehaviour
    {
        public GameMaster Gm;
        public GameObject GameMaster;
        public int NumberOfPlayer;
        public int CostOfUseSpell;
        public float Demage;

        private float spellPower = 1F;
        private Animator spell2Animator;

        private void Start()
        {
            GameMaster = transform.parent.gameObject;
            Gm = GameMaster.GetComponent<GameMaster>();
            for (int i = 0; i < Gm.GetComponent<GameMaster>().ListSpell2.Count; i++)
            {
                NumberOfPlayer = Gm.GetComponent<GameMaster>().ListSpell2[i];
                Gm.GetComponent<GameMaster>().ListSpell2.RemoveAt(i);
            }

            spellPower = Gm.transform.GetChild(NumberOfPlayer).GetComponent<PlatformerCharacter2D>().SpellPower;
            if (spellPower == 4F)
            {
                spellPower = 3f;
            }

            Demage = Demage * spellPower;
            GetComponent<Transform>().localScale = new Vector3(spellPower, spellPower, spellPower);
            GetComponent<CircleCollider2D>().radius *= spellPower / 2;

            StartCoroutine(Destroy());
        }
        private void Awake()
        {
            spell2Animator = GetComponent<Animator>();
        }
        private void Update()
        {
            StartCoroutine(CastSpell_2());
        }

        private IEnumerator CastSpell_2()
        {
            yield return new WaitForSeconds(0.1F);
            gameObject.GetComponent<CircleCollider2D>().radius += 0.04F;
        }

        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(1.0F);
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            PlatformerCharacter2D player = other.collider.GetComponent<PlatformerCharacter2D>();
            if (GetComponent<Spell_2>().NumberOfPlayer != player.GetComponent<PlatformerCharacter2D>().NumberOfPlayer)
            {
                player.TakeHp(Demage);
            }
            if (GetComponent<Spell_2>().NumberOfPlayer == player.GetComponent<PlatformerCharacter2D>().NumberOfPlayer)
            {
                Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>(), true);
            }
        }
    }
}
