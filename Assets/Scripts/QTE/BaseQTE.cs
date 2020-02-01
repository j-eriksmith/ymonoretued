using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseQTE : MonoBehaviour
{
    protected Transform blacksmithCenter;
    protected Transform blacksmithLeft;

    protected float timeOfInitialization;
    protected float timeSinceInitialization;
    protected GameObject activatingPlayer; // Todo: should be the Player component

    public void Start()
    {
        Initialize();
    }

    public void Update()
    {
        if (Input.GetButtonDown("A"))
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
        }
    }

    public virtual void Initialize()
    {
        timeOfInitialization = Time.time;
        DisplayGraphics();
    }

    public virtual void ReceiveInput(string buttonPressed)
    {
        timeSinceInitialization = Time.time - timeOfInitialization;
    }

    protected abstract void DisplayGraphics();
}
