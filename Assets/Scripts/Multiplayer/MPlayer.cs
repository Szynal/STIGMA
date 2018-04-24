using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(MPlayerSetup))]
public class MPlayer : NetworkBehaviour
{
    [SerializeField] private LayerMask _WhatIsGround;   // A mask determining what is ground to the character
    [SerializeField] public GameObject Canvas;
    private Animator _Animator;


    private GameObject _HeroBar;
    [SerializeField] public float AmountOfHP = 1000;
    [SerializeField] public float AmountOfMANA = 100;
    /// <summary>
    /// The amount of mana regeneration per second
    /// </summary> 
    [SerializeField] public float ManaRegen;
    /// <summary>
    /// Magic points
    /// </summary>
    public float _Mana;
    private float _AmountOfMana;
    private bool _manaCD = true;
    [SerializeField] public float ShowMana;
    /// <summary>
    ///  Reference to Mana bar 
    /// </summary>
    private GameObject _ManaBar;

    [SyncVar] public float _HP; // hit points / health    
    private float _AmountOfHealth;
    [SerializeField] public float showHp;
    private GameObject _HpBar;    // Reference to HP bar 

    private bool _OnRegenMagicPoints;

    private Transform _GroundCheck;    // A position marking where to check if the player is grounded.
    const float _GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded


    public bool netFacingRight = true;

    public bool _PullOutSword = false;  // Checks if the sword is pulling out

    private string _netID;
    private MPlayer _player;



    public float GetHealthPct()
    {
        return (float)_HP / _AmountOfHealth;
    }
    //---------------------------------------------------------------

    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField] private GameObject[] disableGameObjectsOnDeath;

    [SerializeField] private GameObject deathEffect;

    [SerializeField] private GameObject spawnEffect;

    private bool firstSetup = true;
    //---------------------------------------------------------------


    private void Start()
    {

        
        _Animator = GetComponent<Animator>();
        transform.name = "Player " + GetComponent<NetworkIdentity>().netId.ToString();
        _HeroBar = GameObject.Find("Canvas").transform.GetChild((int)GetComponent<NetworkIdentity>().netId.Value - 1).gameObject;
        _HeroBar.SetActive(true);
        _Mana = AmountOfMANA;
        _HP = AmountOfHP;
    }


    private void Update()
    {
        CanCast();
        RegenMana();

        ShowMana = _Mana;
    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            //Switch cameras
            //     GameManager.instance.SetSceneCameraActive(false);
            //     GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }
        //  CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }

        SetDefaults();
    }


    public void PullOutSword()
    {
        _PullOutSword = !_PullOutSword;
        _Animator.SetBool("PullingOutSword", _PullOutSword);
    }

    [Command]
    public void CmdTakeDamage(float amount)
    {
        RpcTakeDamage(amount);
    }


    [ClientRpc]
    public void RpcTakeDamage(float amount)
    {
        _HP -= amount;
        _HeroBar.transform.GetChild(0).gameObject.transform.GetComponent<Image>().fillAmount = _HP / AmountOfHP;
    }

    private void Die(string _sourceID)
    {/*
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;
            GameManager.instance.onPlayerKilledCallback.Invoke(username, sourcePlayer.username);
        }

        deaths++;

        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Disable GameObjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        //Disable the collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        //Spawn a death effect
        GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);

        //Switch cameras
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        Debug.Log(transform.name + " is DEAD!");

        StartCoroutine(Respawn());
        */
    }

    public void SetDefaults()
    {/*
        isDead = false;

        currentHealth = maxHealth;

        //Enable the components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //Enable the gameobjects
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        //Enable the collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;

        //Create spawn effect
        GameObject _gfxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    
    */
    }

    [Command]
    public void CmdSyncScale(float x)
    {
        RpcsyncScale(x);
    }

    [ClientRpc]
    public void RpcsyncScale(float x)
    {
        transform.localScale = new Vector3(x, 1, 1);
    }


    public void SetSpellPower(int power)
    {
        if (power == 1)
        {
            _HeroBar.transform.GetChild(2).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(3).gameObject.SetActive(false);
            _HeroBar.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (power == 2)
        {
            _HeroBar.transform.GetChild(2).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(3).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (power == 3)
        {
            _HeroBar.transform.GetChild(2).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(3).gameObject.SetActive(true);
            _HeroBar.transform.GetChild(4).gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// Takes (costOfUseSpell) mana and updates players manaBar
    /// </summary>
    /// <param name="costOfUseSpell"> </param>
    public void TakeMana(float costOfUseSpell)
    {
        _Mana -= costOfUseSpell;
        _HeroBar.transform.GetChild(1).gameObject.transform.GetComponent<Image>().fillAmount = _Mana / AmountOfMANA;
    }
    /// <summary>
    ///  Checks if Player has enough mana to use spells.
    /// </summary>
    /// <returns></returns>
    public bool CanCast()
    {
        if (_Mana > 0 && !_PullOutSword)
        {
            return true;
        }
        else
            return false;
    }
    /// <summary>
    ///  Regeneration of magic points ( +ManaRegen per one second).
    /// </summary>
    private void RegenMana()
    {
        if (_Mana < AmountOfMANA && _manaCD)
        {
            _manaCD = false;
            StartCoroutine(RegenMagicPoint());
        }
    }
    /// <summary>
    ///  Regeneration of magic points ( +ManaRegen per one second).
    /// </summary>
    /// <returns></returns>
    IEnumerator RegenMagicPoint()
    {
        _HeroBar.transform.GetChild(1).gameObject.GetComponent<Image>().fillAmount = _Mana / AmountOfMANA;
        _Mana += ManaRegen;
        yield return new WaitForSeconds(0.2F);
        _manaCD = true;


    }

}
