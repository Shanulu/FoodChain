using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class player : NetworkBehaviour {

    [SerializeField]
    private int maxHealth = 100;

    //we will want all the clients to know this info
    [SyncVar] //This attribute syncs it to all the clients!
    private int currentHealth;
    [SyncVar]
    private bool _isDead = false; //are we dead?
    public bool isDead //no sure what we just did here
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    //my kill streak
    public int myKillStreak;
    

    [SerializeField]
    private Behaviour[] disableOnDeath; //list of stuff to disable
    private bool[] wasEnabled; //list of things that were enabled to start with


    //we need to call this at a particular time, through player setup
    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length]; //make our wasEnabled array the same length of disabledOnDeath
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled; //go through the entirety of the array and enable everything again
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;

        //My killstreak counter
        myKillStreak = 0;

        currentHealth = maxHealth; //when we spawn set our health 

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i]; //iterate through our disabledOnDeath array and store it away
        }
        //make sure our collider is on because we cant use colliders in Behaviour arrays
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }

    }

    [ClientRpc] //will do the function on all clients
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
            Die();
            Debug.Log("I was killed by " + _playerOrigin);
            gameManager.instance.getKiller(_playerOrigin); //need to find my gameManager instance??
        }
    
    }


    private void Die()
    {
        //tell everyone im dead
        isDead = true;
        myKillStreak = 0;

        Debug.Log(transform.name + " is DED!");

        //we will need to disable components so they cant move or shoot while dead, etc
        //going to use a similar array setup as we did with the network disabling
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false; //disable our components
        }
        //turn our collider off cause we are dead.
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }
        //CALL RESPAWN FUNCTION
        //we will use an iEnumerator to wait a period of time
        StartCoroutine(Respawn());

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(gameManager.instance.gameSettings.respawnTime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition(); //find a new spawn locationm
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        Debug.Log(transform.name + " Respawned");
    }


    public void StreakLevelOne()
    {
        Debug.Log("I am gaining POWER!!!!");
    }

}
