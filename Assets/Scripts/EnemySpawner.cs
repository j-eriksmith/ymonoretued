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
    public static Rect arena;

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

    void SetBoundaries()
    {
        // TODO: This could be unified for all the code that needs to use it
        arena = Rect.MinMaxRect(-5.0f, -3.0f, 5.0f, 3.0f);
        spawnAreas = new Line[3];

        // Top spawn area
        spawnAreas[0].dimension = Dimension.X;
        spawnAreas[0].point = arena.yMax;
        spawnAreas[0].a = arena.xMin;
        spawnAreas[0].b = arena.xMax;

        // Right spawn area
        spawnAreas[1].dimension = Dimension.Y;
        spawnAreas[1].point = arena.xMax;
        spawnAreas[1].a = arena.yMin;
        spawnAreas[1].b = arena.yMax;

        // Bottom spawn area
        spawnAreas[2].dimension = Dimension.X;
        spawnAreas[2].point = arena.yMin;
        spawnAreas[2].a = arena.xMin;
        spawnAreas[2].b = arena.xMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(enemyPrefabs.Length == spawnScales.Length);
        SetBoundaries();
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

    void AccumulateTime()
    {
        for (int i = 0; i < timesSince.Length; i++)
            timesSince[i] += Time.deltaTime;
        intensity += Time.deltaTime;
    }

    void ScheduleNextEnemy(int i)
    {
        nextTimes[i] = RandomWait(IntensityScaled(spawnScales[i]));
        timesSince[i] = 0.0f;
        Debug.Log("Next enemy scheduled for t+" + nextTimes[i].ToString());
    }

    void SpawnEnemy()
    {
        Vector3 p;
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (timesSince[i] < nextTimes[i])
                continue;
            p = RandomPosition();
            Debug.Log("Spawning enemy at " + p.ToString() + ", intensity level = " + IntensityScaled(1.0f).ToString());
            Instantiate(enemyPrefabs[i], p, RandomOrientation(p)).SetActive(true);
            ScheduleNextEnemy(i);
        }
    }

    float IntensityScaled(float beta)
    {
        // Intensity has two parameters:
        //  intensityMidgame: the point in the round (in seconds) at which sigmoid is 0.5
        //  intensityScale:   flattening out the sigmoid
        return beta / (1.0f + Mathf.Exp(intensityScale * (intensity - intensityMidgame)));
    }

    float RandomWait(float beta)
    {
        // Sample uniformly from the inverse CDF of the exponential distribution
        return -beta * Mathf.Log((float)rng.NextDouble());
    }

    Vector3 RandomPosition()
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

    Quaternion RandomOrientation(Vector3 p)
    {
        Vector3 delta = (Vector3)arena.center - p;
        return Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x));
    }
}
