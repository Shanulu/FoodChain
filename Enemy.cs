using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;



public class Enemy : NetworkBehaviour {

    private player myPlayerDest = null; //the player I am attacking
    private Vector3 myDestination = Vector3.zero; //my nav destination
    [SerializeField]
    private NavMeshAgent myNavMesh;

    private float speed = 20f;
    public int maxHealth;
    [SyncVar]
    private int currentHealth;
    [SyncVar]
    private bool _isDead = false;
    //need a type variable for my wave controller
    public enum MonsterType { LIGHT, MEDIUM, HEAVY };
    [SerializeField]
    public MonsterType myType;

    public MonsterType getMyType()
    {
        return myType;
    }


    public bool isDead //not sure what we just did here
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    void Start() {
        currentHealth = maxHealth;
        isDead = false;
        myNavMesh.speed = speed;
    }


    [ClientRpc]
    public void RpcTakeDamage(int _damage, string _playerOrigin)
    {
        if (isDead)
        {
            return;
        }
        currentHealth -= _damage;

        Debug.Log(transform.name + "now has" + currentHealth + "health.");

        if (currentHealth <= 0)
        {
            //Die();
            //tell everyone im dead
            Debug.Log(transform.name + " is DED!");
            Network.Destroy(gameObject);
            isDead = true;
            Debug.Log(gameObject.transform.name + " was killed by " + _playerOrigin);
        }

    }
    
  
    private void Die()
    {

    }

    void Awake()
    {
        findPlayer(); //find us a player
    }

    void Update()
    {
            findPlayer(); //find one
            myNavMesh.destination = myDestination; //move to my destination
    }

    private void findPlayer()
    {
        player[] players = GameObject.FindObjectsOfType<player>(); //find all the players
        if (myPlayerDest == null) //if i dont have a player
        {
            List<player> potentialTargets = new List<player>(); //create a temporary list
            Debug.Log("Finding player...");

            if (players != null) //make sure we have some palyers
            {

                foreach (player p in players)
                {
                    if (!p.isDead)//are they dead
                    {
                        potentialTargets.Add(p); //if not add them to the list
                        Debug.Log("Adding players to a list");
                    }
                }
                //now we have a list of alive players
                //select one at random from all the elements and take the transform
                myPlayerDest = potentialTargets[Random.Range(0, potentialTargets.Count)];
                setDestination(myPlayerDest); //set our destination to our target
                Debug.Log("Player Found");
            }
        }
        else //i already have a target lets update the position
        {
            Debug.Log("checking for matching player");
            foreach (player p in players)
            {
                if(p.name == myPlayerDest.name) //is the player still around?
                {
                    Debug.Log("updating position");
                    setDestination(p);
                }
                else
                {
                    Debug.Log("player not found");
                    myPlayerDest = null; //if not hes dead and we should find a new one
                    findPlayer();
                }
            }
        }
    }

    private void setDestination(player _destination)
    {
        myDestination = _destination.transform.position; //set the destiantion to the targets position
        Debug.Log("setting position");
    }
}
