using System;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityStandardAssets._2D
{
    public class Multiplayer2DUserControl : NetworkBehaviour
    {
        private MultiplayerCharacter2D _Character;
        private bool _Jumping;

        [SerializeField] private String _PullOutWeapon;     //   INPUT 
        [SerializeField] private String _Skill1;            //   INPUT 
        [SerializeField] private String _Skill2;            //   INPUT 
        [SerializeField] private String _SpellPower1;       //   INPUT 
        [SerializeField] private String _SpellPower2;       //   INPUT 
        [SerializeField] private String _SpellPower3;       //   INPUT 
        [SerializeField] private String _GetAxis;           //   INPUT  
        [SerializeField] private String _Jump;              //   INPUT 
        [SerializeField] private String _Attack;            //   INPUT 

        [SyncVar] public bool facingRight = true;

        private void Start()
        {
            _Character = GetComponent<MultiplayerCharacter2D>();
            SetAuthority();
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (Input.GetButtonDown(_PullOutWeapon)) _Character.PullOutSword();
            if (Input.GetButtonDown(_SpellPower1)) _Character.CmdSpellPower(1F);
            if (Input.GetButtonDown(_SpellPower2)) _Character.CmdSpellPower(2F);
            if (Input.GetButtonDown(_SpellPower3)) _Character.CmdSpellPower(4F);


            if ((Input.GetAxis(_GetAxis) > 0 && !facingRight) || (Input.GetAxis(_GetAxis) < 0 && facingRight))
            {
                facingRight = !facingRight;
                GetComponent<MultiplayerCharacter2D>().CmdFlip(facingRight);
            }

            //     do zrobienia cooldown przy spellach 1,2 ect....
            if (Input.GetButtonDown(_Skill1) && Time.time > _Character._NextSpell && _Character._PullOutSword == false && _Character._MANA >= _Character.waterBall.GetComponent<MultiplayerSpell_1>().costOfUseSpell * _Character._SpellPower)
            {
                _Character.CmdSpell1();
            }

            if (Input.GetButtonDown(_Skill2) && Time.time > _Character._NextSpell && _Character._PullOutSword == false && _Character._MANA >= _Character.waterImplosion.GetComponent<MultiplayerSpell_2>().CostOfUseSpell * _Character._SpellPower)
            {
                _Character.CmdSpell2();
            }
            if (Input.GetButtonDown(_Attack) && _Character._PullOutSword == true && _Character.GetComponent<MultiplayerPlayerAttacks>()._CanAttack == true)
            {
                _Character.GetComponent<MultiplayerPlayerAttacks>().Attack();
            }
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            //Calculate movement velocity as a 3D vector
            float move = Input.GetAxis(_GetAxis);
            float jump = Input.GetAxis(_Jump);
            _Character.Move(move, jump);
            _Jumping = false;
        }

        public void SetAuthority()
        {
            if (isServer)
            {
                GetComponent<NetworkIdentity>().localPlayerAuthority = true;
            }
            if (!isLocalPlayer && !isServer)
            {
                GetComponent<NetworkIdentity>().localPlayerAuthority = false;
                return;
            }
            if (isLocalPlayer && isClient)
            {
                GetComponent<NetworkIdentity>().localPlayerAuthority = true;
            }


        }
    }
}
