using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyPrefabs;

    // The betas of the exponential distributions for new enemy spawn times
    // (in seconds)
    public float[] spawnScales;

    private GameObject nextEnemy;
    private float nextTime;
    private float timeSince;

    private System.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(enemyPrefabs.Length == spawnScales.Length);
        rng = new System.Random();
        ScheduleNextEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        AccumulateTime();
        SpawnEnemy();
    }

    public void AccumulateTime()
    {
        timeSince += Time.deltaTime;
    }

    public void ScheduleNextEnemy()
    {
        int i = rng.Next(0, enemyPrefabs.Length);
        nextEnemy = enemyPrefabs[i];
        nextTime = RandomWait(spawnScales[i]);
        timeSince = 0f;
    }

    public void SpawnEnemy()
    {
        if (timeSince < nextTime)
            return;
        Instantiate(nextEnemy, RandomPosition(), RandomOrientation());
        ScheduleNextEnemy();
    }

    public float RandomWait(float beta)
    {
        // Sample uniformly from the inverse CDF of the exponential distribution
        return -beta * Mathf.Log((float)rng.NextDouble());
    }

    public Vector2 RandomPosition()
    {
        return new Vector3(0, 0, 0);  // z will always be zero
    }

    public Quaternion RandomOrientation()
    {
        return Quaternion.identity;
    }
}
