using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State{
    CARRYING,
    NOT_CARRYING,
    IN_QTE
}

public class BlacksmithController : MonoBehaviour{
    public float deadzone = 0.1f;
    public float speed = 100;

    public string blacksmithString;

    private Rigidbody2D rb2D;
    private float horizontal;
    private float vertical;

    private GameObject nearbyStation;
    private BaseQTE nearbyStationQTE;

    private State state;

    // Start is called before the first frame update
    void Start(){
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        //TODO: change back to State.NOT_CARRYING
        state = State.CARRYING;
        foreach (string s in Input.GetJoystickNames())
            Debug.Log(s);
        Debug.Log(Input.GetJoystickNames().Length);
    }

    // Update is called once per frame
    void Update(){
        // Debug.Log(horizontal + ", " + vertical);

        // if(Mathf.Abs(horizontal) > deadzone){
        //     float transform = horizontal * speed * Time.deltaTime;
        //     gameObject.transform.position += new Vector3(transform, 0, 0);
        // }

        // if(Mathf.Abs(vertical) > deadzone){
        //     float transform = vertical * speed * Time.deltaTime;
        //     gameObject.transform.position += new Vector3(0, transform, 0);
        // }

        if(state == State.IN_QTE){
            if(Input.GetButtonDown(blacksmithString + " A")){
                nearbyStationQTE.ReceiveInput("A");
            }
            if(Input.GetButtonDown(blacksmithString + " B")){
                nearbyStationQTE.ReceiveInput("B");
            }
            if(Input.GetButtonDown(blacksmithString + " X")){
                nearbyStationQTE.ReceiveInput("X");
            }
            if(Input.GetButtonDown(blacksmithString + " Y")){
                nearbyStationQTE.ReceiveInput("Y");
            }
        }
        else{
            horizontal = Input.GetAxis(blacksmithString + " Horizontal");
            vertical = -1 * Input.GetAxis(blacksmithString + " Vertical");
            //Debug.Log(horizontal);
            //Debug.Log(vertical);

            //Debug.Log(Input.GetJoystickNames());
            //Debug.Log(Input.GetJoystickNames());

            if(nearbyStation && state == State.CARRYING){
                if(Input.GetButtonDown(blacksmithString + " A")){
                    // Debug.Log(nearbyStation.GetComponent<Transform>().position);
                    Debug.Log("Initialize");
                    nearbyStationQTE.Initialize();
                    state = State.IN_QTE;
                }
            }
        }
    }

    void FixedUpdate(){
        if(state == State.CARRYING || state == State.NOT_CARRYING){
            if(Mathf.Abs(horizontal) > deadzone){
                float transform = horizontal * speed * Time.fixedDeltaTime;
                rb2D.position += new Vector2(transform, 0);
            }

            if(Mathf.Abs(vertical) > deadzone){
                float transform = vertical * speed * Time.fixedDeltaTime;
                rb2D.position += new Vector2(0, transform);
            }
        }
    }

    public void updateNearbyStation(GameObject g){
        nearbyStation = g;
        nearbyStationQTE = g.GetComponent<BaseQTE>();
    }
    
    public void resetNearbyStation(){
        nearbyStation = null;
        nearbyStationQTE = null;
    }

    public void qteSucceed(){
        state = State.CARRYING;
    }

    public void qteFail() {
        state = State.CARRYING;
    }
}
