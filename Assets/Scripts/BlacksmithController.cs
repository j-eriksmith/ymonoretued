using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithController : MonoBehaviour{
    public float deadzone = 0.1f;
    public float speed = 100;

    private Rigidbody2D rb2D;
    private float horizontal;
    private float vertical;

    private GameObject nearbyStation;
    private BaseQTE nearbyStationQTE;

    // Start is called before the first frame update
    void Start(){
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Debug.Log(horizontal + ", " + vertical);

        // if(Mathf.Abs(horizontal) > deadzone){
        //     float transform = horizontal * speed * Time.deltaTime;
        //     gameObject.transform.position += new Vector3(transform, 0, 0);
        // }

        // if(Mathf.Abs(vertical) > deadzone){
        //     float transform = vertical * speed * Time.deltaTime;
        //     gameObject.transform.position += new Vector3(0, transform, 0);
        // }

        if(nearbyStation){
            if(Input.GetButtonDown("Fire1")){
                Debug.Log(nearbyStation.GetComponent<Transform>().position);
            }
        }
    }

    void FixedUpdate(){
        if(Mathf.Abs(horizontal) > deadzone){
            float transform = horizontal * speed * Time.fixedDeltaTime;
            rb2D.position += new Vector2(transform, 0);
        }

        if(Mathf.Abs(vertical) > deadzone){
            float transform = vertical * speed * Time.fixedDeltaTime;
            rb2D.position += new Vector2(0, transform);
        }
    }

    void updateNearbyStation(GameObject g){
        nearbyStation = g;
        nearbyStationQTE = g.GetComponent<BaseQTE>();
    }
    
    void resetNearbyStation(){
        nearbyStation = null;
        nearbyStationQTE = null;
    }
}
