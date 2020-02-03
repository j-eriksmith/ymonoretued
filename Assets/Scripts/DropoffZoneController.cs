using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffZoneController : MonoBehaviour{
    enum State{
        HOLDING_ITEM,
        NOT_HOLDING_ITEM
    }

    private State state;
    private GameObject itemInstance;

    public GameObject item;
    public GameObject[] qteObjects;
    [HideInInspector]
    public Queue<GameObject> qteQueue = new Queue<GameObject>();
    [HideInInspector]
    public int durability;

    void Start(){
        state = State.HOLDING_ITEM;
        qteQueue.Enqueue(qteObjects[0]);
        durability = 0;
    }

    public void Dropoff(int durability){
        if(state == State.NOT_HOLDING_ITEM){
            Debug.Log("REEEEE");
            state = State.HOLDING_ITEM;
            itemInstance = Instantiate(item, gameObject.transform);
            itemInstance.transform.localScale = new Vector3(3, 3, 1);
            Vector3 pos = itemInstance.transform.position;
            itemInstance.transform.position = new Vector3(pos.x, pos.y, -1);

            this.durability = durability;

            if(durability >= 0 && durability < 20){
                fillQueue(4);
            }
            else if(durability >= 20 && durability < 50){
                fillQueue(3);
            }
            else if(durability >= 50 && durability < 80){
                fillQueue(2);
            }
            else if(durability >= 80 && durability < 100){
                fillQueue(1);
            }
        }
        else{
            Debug.Log("Already holding item!");
        }
    }

    public void Pickup(){
        if(state == State.HOLDING_ITEM){
            state = State.NOT_HOLDING_ITEM;
            Destroy(itemInstance);
            durability = 0;
        }
        else{
            Debug.Log("No item to pick up!");
        }
    }

    private void fillQueue(int numEvents){
        for(int i = 0; i < numEvents; ++i){
            int e = Mathf.FloorToInt(Random.Range(0, 4));

            qteQueue.Enqueue(qteObjects[e]);
            Debug.Log("Enqeued " + qteObjects[e].name);
        }
    }
}
