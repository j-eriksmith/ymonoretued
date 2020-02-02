using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dimension
{
    X,
    Y
}

// A horizontal (dimension 0) or vertical (dimension 1) line, with a starting
// point in the orthogonal dimension and an extension from a to b in its own
public struct Line
{
    public Dimension dimension;
    public float point;
    public float a;
    public float b;
}

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyPrefabs;

    // The betas of the exponential distributions for new enemy spawn times
    // (in seconds)
    public float[] spawnScales;

    public Line[] spawnAreas;

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

    private void AccumulateTime()
    {
        for (int i = 0; i < timesSince.Length; i++)
            timesSince[i] += Time.deltaTime;
        intensity += Time.deltaTime;
    }

    private void ScheduleNextEnemy(int i)
    {
        nextTimes[i] = RandomWait(IntensityScaled(spawnScales[i]));
        timesSince[i] = 0.0f;
        Debug.Log("Next enemy scheduled for t+" + nextTimes[i].ToString());
    }

    private void SpawnEnemy()
    {
        Vector3 p;
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (timesSince[i] < nextTimes[i])
                continue;
            Debug.Log("Spawning enemy, intensity level = " + IntensityScaled(1.0f).ToString());
            p = RandomPosition();
            Instantiate(enemyPrefabs[i], p, RandomOrientation(p));
            ScheduleNextEnemy(i);
        }
    }

    private float IntensityScaled(float beta)
    {
        return beta / (1.0f + Mathf.Exp(intensityScale * (intensity - intensityMidgame)));
    }

    private float RandomWait(float beta)
    {
        // Sample uniformly from the inverse CDF of the exponential distribution
        return -beta * Mathf.Log((float)rng.NextDouble());
    }

    private Vector3 RandomPosition()
    {
        float[] sampleSpace = new float[spawnAreas.Length + 1];
        float selectedPoint;
        int i;
        sampleSpace[0] = 0.0f;
        for (i = 0; i < spawnAreas.Length; i++)
            sampleSpace[i + 1] = sampleSpace[i] + (spawnAreas[i].b - spawnAreas[i].a);
        selectedPoint = (float)rng.NextDouble() * sampleSpace[i];
        for (i = 0; sampleSpace[i + 1] > selectedPoint; i++);
        selectedPoint -= sampleSpace[i];
        switch (spawnAreas[i].dimension)
        {
            case Dimension.X:  // x-extended line (horizontal)
                return new Vector3(
                    spawnAreas[i].a + selectedPoint,
                    spawnAreas[i].point,
                    0
                );
            case Dimension.Y:  // y-extended line (vertical)
                return new Vector3(
                    spawnAreas[i].point,
                    spawnAreas[i].a + selectedPoint,
                    0
                );
            default:  // This code path should be inaccessible
                Debug.Assert(false);
                return new Vector3(0, 0, 0);
        }
    }

    private Quaternion RandomOrientation(Vector3 p)
    {
        return Quaternion.LookRotation(-p);
    }
}
