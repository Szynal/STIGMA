using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class GameMaster : MonoBehaviour
{
    public List<int> listSpell_1 = new List<int>();
    public List<int> listSpell_2 = new List<int>();

    public PlatformerCharacter2D player_1 = new PlatformerCharacter2D();
    public PlatformerCharacter2D player_2 = new PlatformerCharacter2D();

    private void Start()
    {
        listSpell_1.Clear();
        listSpell_2.Clear();
    }

    private void Update()
    {
        player_1 = transform.GetChild(0).GetComponent<PlatformerCharacter2D>();
        if (player_1._HP <= 0) player_1.transform.gameObject.SetActive(false);

        player_2 = transform.GetChild(1).GetComponent<PlatformerCharacter2D>();
        if (player_2._HP <= 0) player_2.transform.gameObject.SetActive(false);
    }

}
