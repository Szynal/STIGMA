using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Spell_1 : MonoBehaviour
{
    public GameObject player;
    public GameMaster GM;
    public GameObject gameMaster;

    [SerializeField] int numberOfPlayer;
    [SerializeField] public int costOfUseSpell;
    [SerializeField] float demage;

    private float _spellPower = 1F;
    private Animator _spell_1_Animator;
    public float spell_1_Speed = 15F;

    private void Start()
    {
        gameMaster = transform.parent.gameObject;
        GM = gameMaster.GetComponent<GameMaster>();

        for (int i = 0; i < GM.GetComponent<GameMaster>().listSpell_1.Count; i++)
        {
            numberOfPlayer = GM.GetComponent<GameMaster>().listSpell_1[i];
            GM.GetComponent<GameMaster>().listSpell_1.RemoveAt(i);
        }

        _spellPower = GM.transform.GetChild(numberOfPlayer).GetComponent<PlatformerCharacter2D>().spellPower;
        if (_spellPower == 4F) _spellPower = 3f;

        demage = demage * _spellPower;
        GetComponent<CircleCollider2D>().radius *= _spellPower / 2;
    }

    private void Awake()
    {
        _spell_1_Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (GetComponent<Transform>().transform.localScale == new Vector3(-1.0F, 1.0F, 1.0F)) // Fllip Spell (like player diraction)
        {
            GetComponent<Transform>().localScale = new Vector3(-_spellPower, _spellPower, _spellPower);
            GetComponent<Rigidbody2D>().velocity = transform.right * spell_1_Speed * (-1.0F);  // ijemna oś "x"
        }
        else if (GetComponent<Transform>().transform.localScale == new Vector3(1.0F, 1.0F, 1.0F))  
        {
            GetComponent<Transform>().localScale = new Vector3(_spellPower, _spellPower, _spellPower);
            GetComponent<Rigidbody2D>().velocity = transform.right * spell_1_Speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == ("Platform")) // objiekt jest usównay w przypadku dotkniećia z ikoljaderem trtypu platforma
        {
            Destroy(gameObject);
        }
        else if (other.tag == ("Player"))       // w przypadku doknięcia playera spell _1 ma zabierać HP & być uówany. 
        {
            PlatformerCharacter2D player = other.GetComponent<PlatformerCharacter2D>();
            if (GetComponent<Spell_1>().numberOfPlayer != player.GetComponent<PlatformerCharacter2D>().numberOfPlayer)
            {
                player.take_HP(demage);
                Destroy(gameObject);
            }
        }
    }

}
