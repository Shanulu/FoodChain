using UnityEngine;
using System.Collections.Generic;

public class gameManager : MonoBehaviour {

    //we only want one instance of this at any given time
    public static gameManager instance;

    public gameSettings gameSettings;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one gameManager in scene");
        }
        else 
        {
            instance = this; //set it to ourself
        }
    }


    #region Player registering



    private const string PLAYER_ID_PREFIX = "Player ";
    //dictionary has a Key of string, and a Value of a player 
    private static Dictionary<string, player> playerDictionary = new Dictionary<string, player>();
   
    
    
    
    
     //bring in a netID and a player
    public static void RegisterPlayer(string _netID, player _player)
    {
        //add some players to our Dictionary
        string _playerID = PLAYER_ID_PREFIX + _netID; //give our players a full name
        playerDictionary.Add(_playerID, _player); //add it to the dictionary (ie: player 1, player entity)
        _player.transform.name = _playerID; //change the entities name

    }

    public static void UnregisterPlayer(string _playerID)
    {
        playerDictionary.Remove(_playerID); //remove our player from the dictionary
    }

    public static player GetPlayer(string _playerID)
    {
        return playerDictionary[_playerID]; //return the player on the id we passed in
    }

    /*void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500)); //x, y, width, heigt
        GUILayout.BeginVertical();


        foreach (string _playerID in playerDictionary.Keys)
        {
            GUILayout.Label(_playerID + "  -  " + playerDictionary[_playerID].transform.name);
        }


        GUILayout.EndVertical();
        GUILayout.EndArea();


    }*/

    #endregion



    //Kill tracking
    public void getKiller(string _playerID) //does this need to be static?
    {
        player killer = GetPlayer(_playerID); //find our killer with our dictionary
        killer.myKillStreak += 1; //increment our killstreak
        if (killer.myKillStreak >= 5)
        {
            Debug.Log("KILLSTREAK REWARD LEVEL 1");
            killer.StreakLevelOne();
        }
    }
}
