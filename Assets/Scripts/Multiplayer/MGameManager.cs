using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MGameManager : MonoBehaviour
{

    public static MGameManager instance;
    private const string _PlayerIdPrefix = "Player ";
    private static Dictionary<string, MPlayer> players = new Dictionary<string, MPlayer>();

    //public static Dictionary<string, MSpell1> spells = new Dictionary<string, MSpell1>();

    private void Awake()
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