using UnityEngine;
using UnityEngine.Networking;

public class playerShoot : NetworkBehaviour {

    public playerWeapon myWeapon; //our weapon
    private const string PLAYER_TAG = "Player";
    private const string ENEMY_TAG = "Enemy";
    //my spawner test
    private GameObject mySpawner;

    [SerializeField]
    private Camera myCam;


    //layer mask for our raycast
    [SerializeField]
    private LayerMask myMask;

    void Start()
    {
        if (myCam == null) 
        {
            Debug.LogError("Player Shoot: No Camera Referenced");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            shootWeapon();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Spawning enemy maybe...");
            if(GameObject.FindGameObjectWithTag("enemySpawner") != null)
            {
                Debug.Log("we found a spawner");
                mySpawner = GameObject.FindGameObjectsWithTag("enemySpawner")[0];

                mySpawner.GetComponent<EnemyController>().SpawnEnemy();

            }
        }
    }

    //client side
    [Client]
    void shootWeapon()
    {
        RaycastHit _hit; //new raycast variable
        //start of ray location, direction, variable, distance, layer mask
        if (Physics.Raycast(myCam.transform.position, myCam.transform.forward, out _hit, myWeapon.range, myMask))
        {
            if (_hit.collider.tag == PLAYER_TAG) //does it hit a player?
            {
                CmdPlayerHit(_hit.collider.name, myWeapon.damage, gameObject.name); //send the playerID to server side playerhit
            }
            else if (_hit.collider.tag == ENEMY_TAG) // does it hit an enemy?
            {
                CmdEnemyHit(_hit.transform.gameObject, myWeapon.damage, gameObject.name);
            }
            else
            {
                Debug.Log(_hit.collider.name); //what did we hit??
            }
        }


    }


    //server side
    [Command]
    void CmdPlayerHit(string _playerID, int _damage, string _playerOrigin)
    {
        Debug.Log(_playerID + " has been shot.");

       player _player = gameManager.GetPlayer(_playerID); //who did we shoot?

        _player.RpcTakeDamage(_damage, _playerOrigin); //tell them to take damage! and from who
       
    }


    [Command]
    public void CmdEnemyHit(GameObject _enemyHit, int _damage, string _playerOrigin)
    {
        Debug.Log("I have been hit by" + _playerOrigin);
        _enemyHit.GetComponent<Enemy>().RpcTakeDamage(_damage, _playerOrigin);
    }
}
