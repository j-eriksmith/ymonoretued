using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpiderCreate : MonoBehaviour
{
    public GameObject spider;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(spider, new Vector2(3f, 3f), Quaternion.identity);
        Vector3 help;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
