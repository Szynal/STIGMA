using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MPlayer))]
public class MPlayerSetup : NetworkBehaviour
{

    [SerializeField] Behaviour[] componentsToDisable;
    public NetworkAnimator networkAnimator;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            GetComponent<Animator>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        if (isLocalPlayer)
        {
            gameObject.transform.GetChild(4).gameObject.SetActive(true);
            GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = gameObject.transform.GetChild(4).GetComponent<Camera>();
        }
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    public override void OnStartServer()
    {
        gameObject.GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        MPlayer player = GetComponent<MPlayer>();
        networkAnimator = gameObject.GetComponent<NetworkAnimator>();

        networkAnimator.SetParameterAutoSend(0, true);
        networkAnimator.SetTrigger("Speed");
        networkAnimator.SetParameterAutoSend(1, true);
        networkAnimator.SetTrigger("Ground");
        networkAnimator.SetParameterAutoSend(2, true);
        networkAnimator.SetParameterAutoSend(3, true);
        networkAnimator.SetParameterAutoSend(4, true);
        networkAnimator.SetParameterAutoSend(5, true);
        networkAnimator.SetParameterAutoSend(6, true);
        networkAnimator.SetParameterAutoSend(7, true);
        networkAnimator.SetParameterAutoSend(8, true);

        GameManager.RegisterPlayer(netID, player);
    }
}
