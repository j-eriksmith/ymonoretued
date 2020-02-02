using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderController : MonoBehaviour{
    public float passiveSpeed = 0.1f;
    public float activeSpeed = 1f;
    public float minPosition;
    public float maxPosition;
    public GameObject markerPrefab;
    public GameObject playerPrefab;

    private GameObject player;


    private float markerPosition;
    private float ourXVelocity;

    void Start(){
        markerPosition = Random.Range(minPosition, maxPosition);
        ourXVelocity = 0.0f;

        GameObject centerMarker = Instantiate(markerPrefab, new Vector3(markerPosition, 0f, -1f), Quaternion.identity, gameObject.transform);
        player = Instantiate(playerPrefab, gameObject.transform);
    }

    void Update(){
        //float newXPos = player.transform.position.x + (player.transform.position.x * passiveSpeed);
        float newXPos = player.transform.position.x + (Mathf.Abs(player.transform.position.x) / 5.0f) * passiveSpeed;

        if(Input.GetAxis("LT") != 0.0f){
            Debug.Log("LT");
            ourXVelocity -= (Time.deltaTime * activeSpeed);
        }
        if(Input.GetAxis("RT") != 0.0f){
            Debug.Log("RT" + Input.GetAxis("RT"));
            ourXVelocity += (Time.deltaTime * activeSpeed);
        }
        ourXVelocity = Mathf.Clamp(ourXVelocity, -150f, 150f);

        player.transform.position = new Vector3(Mathf.Clamp(newXPos + ourXVelocity, -6.875f, 6.875f), 0f, -1f);
        if (player.transform.position.x == minPosition || player.transform.position.x == maxPosition)
        {
            ourXVelocity = 0.0f;
        }
    }
}
