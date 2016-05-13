using UnityEngine;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {

    //I want to store my prefabs and their type in an array set in the inspector
    [SerializeField]
    public Enemy[] Enemies;

    //ill sort all the enemies I have into lists
    private List<Enemy> lightWave = new List<Enemy>();
    private List<Enemy> mediumWave = new List<Enemy>();
    private List<Enemy> heavyWave = new List<Enemy>();

    //initialize all my lists
    void Awake()
    {
        setWaves();
        //Debug.Log("I am setting the waves up");
    }

    void setWaves()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i].getMyType() == Enemy.MonsterType.LIGHT) //put lights in the light list
            {
                lightWave.Add(Enemies[i]);
            } else
            if (Enemies[i].getMyType() == Enemy.MonsterType.MEDIUM) //mediums in medium
            {
                mediumWave.Add(Enemies[i]);
            } else
            if (Enemies[i].getMyType() == Enemy.MonsterType.HEAVY) //heavy in heavy
            {
                heavyWave.Add(Enemies[i]);
            }
            //could make a mixed list with a random assorment if we wanted.
        }
    }


    /* DBBUGGING
    public void returnWaves()
    {
        Debug.Log("Light Wave:");
        for (int i = 0; i < lightWave.Count; i++)
        {
            Debug.Log(lightWave[i]);
        }
        Debug.Log("medium Wave:");
        for (int i = 0; i < mediumWave.Count; i++)
        {
            Debug.Log(mediumWave[i]);
        }
        Debug.Log("Heavy Wave:");
        for (int i = 0; i < heavyWave.Count; i++)
        {
            Debug.Log(heavyWave[i]);
        }

    }
    */
}
