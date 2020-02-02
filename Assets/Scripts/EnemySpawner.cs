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
        spawnAreas = new Line[1];
        spawnAreas[0].dimension = Dimension.X;
        spawnAreas[0].point = 3.0f;
        spawnAreas[0].a = -3.0f;
        spawnAreas[0].b = 3.0f;  // REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE
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
            p = RandomPosition();
            Debug.Log("Spawning enemy at " + p.ToString() + ", intensity level = " + IntensityScaled(1.0f).ToString());
            Instantiate(enemyPrefabs[i], p, RandomOrientation(p));
            ScheduleNextEnemy(i);
        }
    }

    private float IntensityScaled(float beta)
    {
        // Intensity has two parameters:
        //  intensityMidgame: the point in the round (in seconds) at which sigmoid is 0.5
        //  intensityScale:   flattening out the sigmoid
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

        // Calculate, based on the lengths of every Line in spawnAreas, which
        // area (sampled uniformly) the new enemy should spawn in
        sampleSpace[0] = 0.0f;
        for (i = 0; i < spawnAreas.Length; i++)
            sampleSpace[i + 1] = sampleSpace[i] + (spawnAreas[i].b - spawnAreas[i].a);
        selectedPoint = (float)rng.NextDouble() * sampleSpace[i];
        for (i = 0; sampleSpace[i + 1] <= selectedPoint; i++)

        // selectedPoint now refers to the distance from the beginning of the
        // Line at which the spawn should happen
        selectedPoint -= sampleSpace[i];

        // Return the spawn location
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
