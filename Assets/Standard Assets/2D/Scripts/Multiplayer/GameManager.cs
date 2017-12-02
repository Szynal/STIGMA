using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using System.Linq;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private const string _PlayerIdPrefix = "Player ";
    private static Dictionary<string, MPlayer> players = new Dictionary<string, MPlayer>();
    public static Dictionary<string, MultiplayerSpell_1> spells = new Dictionary<string, MultiplayerSpell_1>();

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
    }

    public static void RegisterPlayer(string netID, MPlayer player)
    {
        string _playerID = _PlayerIdPrefix + netID;
        players.Add(_playerID, player);
        player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static MPlayer GetPlayer(string playerID)
    {
        return players[playerID];
    }

    public static MPlayer[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

}