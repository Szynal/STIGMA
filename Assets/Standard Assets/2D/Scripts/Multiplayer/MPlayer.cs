using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(MPlayerSetup))]
public class MPlayer : NetworkBehaviour
{
    [SerializeField] private LayerMask m_WhatIsGround;   // A mask determining what is ground to the character
    [SerializeField] public GameObject Canvas;
    private Animator _Animator;

    [SyncVar] public float _MANA;      //  magic points
    private float _AmountOfMana;
    [SerializeField] public float showMana;
    private GameObject _ManaBar;  // Reference to Mana bar 

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


    [ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourceID)
    {/*
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die(_sourceID);
        }
        */
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

}
