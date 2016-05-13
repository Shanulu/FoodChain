using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class EnemyController : NetworkBehaviour
{

    public GameObject enemyTest;

        public void SpawnEnemy()
        {        
            GameObject enemy = (GameObject)Instantiate(enemyTest, transform.position, transform.rotation);
            NetworkServer.Spawn(enemy);
            Debug.Log("spawned and enemy!");
        } 
 

  
}