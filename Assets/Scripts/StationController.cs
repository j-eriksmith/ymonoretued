using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            Debug.Log("PLAYER ENTER");
            collider.gameObject.SendMessage("updateNearbyStation", gameObject);
            gameObject.GetComponent<BaseQTE>().activatingPlayer = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            Debug.Log("PLAYER EXIT");
            collider.gameObject.SendMessage("resetNearbyStation");
        }
    }
}
