using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyPrefabs;

    // The betas of the exponential distributions for new enemy spawn times
    // (in seconds)
    public float[] spawnScales;

    public float intensityMidgame;
    public float intensityScale;

    private float[] nextTimes;
    private float[] timesSince;
    private float intensity;

    private System.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(enemyPrefabs.Length == spawnScales.Length);
        rng = new System.Random();
        intensity = 0;
        nextTimes = new float[enemyPrefabs.Length];
        timesSince = new float[enemyPrefabs.Length];
        for (int i = 0; i < enemyPrefabs.Length; i++)
            ScheduleNextEnemy(i);
    }

    // Update is called once per frame
    void Update()
    {
        AccumulateTime();
        SpawnEnemy();
    }

    public void AccumulateTime()
    {
        for (int i = 0; i < timesSince.Length; i++)
            timesSince[i] += Time.deltaTime;
        intensity += Time.deltaTime;
    }

    public void ScheduleNextEnemy(int i)
    {
        nextTimes[i] = RandomWait(IntensityScaled(spawnScales[i]));
        timesSince[i] = 0.0f;
        Debug.Log("Next enemy scheduled for t+" + nextTimes[i].ToString());
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (timesSince[i] < nextTimes[i])
                continue;
            Debug.Log("Spawning enemy, intensity level = " + IntensityScaled(1.0f).ToString());
            Instantiate(enemyPrefabs[i], RandomPosition(), RandomOrientation());
            ScheduleNextEnemy(i);
        }
    }

    public float IntensityScaled(float beta)
    {
        return beta / (1.0f + Mathf.Exp(intensityScale * (intensity - intensityMidgame)));
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
