using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffZoneController : MonoBehaviour{
    enum State{
        HOLDING_ITEM,
        NOT_HOLDING_ITEM
    }

    private State state;

    public GameObject item;
    private GameObject itemInstance;

    void Start(){
        state = State.HOLDING_ITEM;
    }

    public void Dropoff(){
        if(state == State.NOT_HOLDING_ITEM){
            state = State.HOLDING_ITEM;
            itemInstance = Instantiate(item, gameObject.transform);
            itemInstance.transform.localScale = new Vector3(3, 3, 1);
        }
        else{
            Debug.Log("Already holding item!");
        }
    }

    public void Pickup(){
        if(state == State.HOLDING_ITEM){
            state = State.NOT_HOLDING_ITEM;
            Destroy(itemInstance);
        }
        else{
            Debug.Log("No item to pick up!");
        }
    }
}
