using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffZoneController : MonoBehaviour{
    enum State{
        HOLDING_ITEM,
        NOT_HOLDING_ITEM
    }

    //Create a custom struct and apply [Serializable] attribute to it
    [System.Serializable]
    private class DurabilityThreshold
    {
        public int threshold;
        public int eventsToGenerate;
    }

    //Make the private field of our PlayerStats struct visible in the Inspector
    //by applying [SerializeField] attribute to it
    [SerializeField]
    private DurabilityThreshold[] durabilityThresholds;

    [SerializeField]
    private State state;
    private GameObject itemInstance;

    public GameObject item;
    public List<GameObject> qteObjects;
    [HideInInspector]
    public Queue<GameObject> qteQueue = new Queue<GameObject>();
    [HideInInspector]
    public int durability;

    void Start(){
        state = State.NOT_HOLDING_ITEM;
        durability = 0;
    }

    public void Dropoff(int durability){
        Debug.Log("attempting dropoff");
        if(state == State.NOT_HOLDING_ITEM){
            state = State.HOLDING_ITEM;
            // Todo: activate child object instead of instantiating
            itemInstance = Instantiate(item, gameObject.transform);
            itemInstance.transform.localScale = new Vector3(3, 3, 1);
            Vector3 pos = itemInstance.transform.position;
            itemInstance.transform.position = new Vector3(pos.x, pos.y, -1);
            // Everything up to here

            this.durability = durability;

            for (int i = 0; i < durabilityThresholds.Length - 1; ++i)
            {
                if (durability < durabilityThresholds[i].threshold && durability >= durabilityThresholds[i+1].threshold)
                {
                    FillQueue(durabilityThresholds[i].eventsToGenerate);
                }
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

    private void FillQueue(int numEvents){
        for(int i = 0; i < numEvents; ++i){
            int eventIndex = -1;
            if (i > 0)
            {
                eventIndex = Mathf.FloorToInt(Random.Range(0, qteObjects.Count - 1));
            }
            else
            {
                eventIndex = Mathf.FloorToInt(Random.Range(0, qteObjects.Count));
            }
            qteQueue.Enqueue(qteObjects[eventIndex]);
            qteObjects.Add(qteObjects[eventIndex]);
            qteObjects.RemoveAt(eventIndex);
            Debug.Log("Enqeued " + qteObjects[eventIndex].name);
        }
    }
}
