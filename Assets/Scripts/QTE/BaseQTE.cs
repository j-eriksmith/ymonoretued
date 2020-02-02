using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseQTE : MonoBehaviour
{
    public Sprite[] buttonSprites;

    public Transform blacksmithCenter;

    [HideInInspector]
    public GameObject activatingPlayer;

    protected float timeOfInitialization;
    protected float timeSinceInitialization;

    [SerializeField]
    protected AudioClip eventWinSound;
    [SerializeField]
    protected AudioClip eventFailSound;
    protected AudioSource qteAudioSource;

    public void Start()
    {
        //Initialize();
        qteAudioSource = transform.parent.gameObject.GetComponent<AudioSource>();
    }

    public void Update()
    {
/*          if (Input.GetButtonDown("A"))
         {
             ReceiveInput("A");
         }
         if (Input.GetButtonDown("B"))
         {
             ReceiveInput("B");
         }
         if (Input.GetButtonDown("X"))
         {
             ReceiveInput("X");
         }
         if (Input.GetButtonDown("Y"))
         {
             ReceiveInput("Y");
         } */
    }

    public virtual void Initialize()
    {
        timeOfInitialization = Time.time;
        DisplayInitialGraphics();
    }

    public virtual void ReceiveInput(string buttonPressed)
    {
        timeSinceInitialization = Time.time - timeOfInitialization;
    }

    protected virtual void DisplayInitialGraphics() {}
}
