using UnityEngine;

namespace Assets.Scripts.Singleplayer
{
    public class Spell1 : MonoBehaviour
    {
        public GameObject Player;
        public GameMaster Gm;
        public GameObject GameMaster;
        public int NumberOfPlayer;
        public int CostOfUseSpell;
        public float Spell1Speed = 15F;d

        private float demage;
        private float spellPower = 1F;
        private Animator spell1Animator;

        private void Start()
        {
            GameMaster = transform.parent.gameObject;
            Gm = GameMaster.GetComponent<GameMaster>();

            for (int i = 0; i < Gm.GetComponent<GameMaster>().ListSpell1.Count; i++)
            {
                NumberOfPlayer = Gm.GetComponent<GameMaster>().ListSpell1[i];
                Gm.GetComponent<GameMaster>().ListSpell1.RemoveAt(i);
            }

            spellPower = Gm.transform.GetChild(NumberOfPlayer).GetComponent<PlatformerCharacter2D>().SpellPower;
            if (spellPower == 4F)
            {
                spellPower = 3f;
            }

            demage = demage * spellPower;
            GetComponent<CircleCollider2D>().radius *= spellPower / 2;
        }

        private void Awake()
        {
            spell1Animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if (GetComponent<Transform>().transform.localScale == new Vector3(-1.0F, 1.0F, 1.0F)) // Fllip Spell (like player diraction)
            {
                GetComponent<Transform>().localScale = new Vector3(-spellPower, spellPower, spellPower);
                GetComponent<Rigidbody2D>().velocity = transform.right * Spell1Speed * (-1.0F);  // ijemna oś "x"
            }
            else if (GetComponent<Transform>().transform.localScale == new Vector3(1.0F, 1.0F, 1.0F))
            {
                GetComponent<Transform>().localScale = new Vector3(spellPower, spellPower, spellPower);
                GetComponent<Rigidbody2D>().velocity = transform.right * Spell1Speed;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == ("Platform"))
            {
                Destroy(gameObject);
            }
            else if (other.tag == ("Player"))
            {
                PlatformerCharacter2D player = other.GetComponent<PlatformerCharacter2D>();
                if (GetComponent<Spell1>().NumberOfPlayer != player.GetComponent<PlatformerCharacter2D>().NumberOfPlayer)
                {
                    player.TakeHp(demage);
                    Destroy(gameObject);
                }
            }
        }
    }
}
