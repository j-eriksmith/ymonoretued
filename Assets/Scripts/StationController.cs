﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            Debug.Log("PLAYER ENTER");
            collider.gameObject.SendMessage("updateNearbyStation", gameObject);

            BaseQTE baseQTE = gameObject.GetComponent<BaseQTE>();
            if(baseQTE){
                baseQTE.activatingPlayer = collider.gameObject;
            }
        }
        else if(collider.gameObject.tag == "Hero"){
            collider.gameObject.GetComponent<HeroController>().enterDropoff();
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Player"){
            Debug.Log("PLAYER EXIT");
            collider.gameObject.SendMessage("resetNearbyStation");
        }
        else if(collider.gameObject.tag == "Hero"){
            collider.gameObject.GetComponent<HeroController>().exitDropoff();
        }
    }
}
