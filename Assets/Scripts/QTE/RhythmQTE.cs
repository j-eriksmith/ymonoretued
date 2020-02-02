using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmQTE : BaseQTE
{
    public struct RhythmEvent
    {
        public string key;
        public float timeToFail;
        public RhythmEvent(string inKey, float inTime)
        {
            key = inKey;
            timeToFail = inTime;
        }
    }

    public int minSequenceLength;
    public int maxSequenceLength;

    public float minTimeToGoalArea;
    public float maxTimeToGoalArea;

    [Tooltip("What scale factor larger the goal will be relative to the button size")]
    public float goalAreaSize = .5f;

    public Sprite circleSprite;
    public Color closingCircleColor;
    public Color goalCircleColor;

    private string[] validEvents = {"A", "B", "X", "Y"};
    private Queue<RhythmEvent> inputSequence = new Queue<RhythmEvent>();
    private SpriteRenderer buttonPromptRenderer;
    private GameObject closingCirclePrompt;
    private GameObject goalAreaCirclePrompt;

    private float closingCircleTimer;
    private float initialClosingCircleTimer;

    private float goalAreaCircleSize;
    private float closingCircleSize;

    new public void Update()
    {
        base.Update();

        if (inputSequence.Count == 0) return; // not initialized yet

        // Scale the closing circle
        // .25 scale = around the button
        if (closingCircleTimer > 0.0f)
        {
            closingCircleTimer -= Time.deltaTime;
            closingCircleSize = Mathf.Lerp(1f, 0.25f, 1 - (closingCircleTimer / initialClosingCircleTimer));
            closingCirclePrompt.transform.localScale = new Vector3(closingCircleSize, closingCircleSize, 1f);
        }
        if (closingCircleTimer <= 0.0f)
        {
            // Fail the prompt - stun the player? insta-retry?
            Debug.Log("Failed QTE!");
            FailQTE();
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        
        int eventsToGenerate = Mathf.FloorToInt(Random.Range(minSequenceLength, maxSequenceLength + 1));
        float timeToGoalAreaToEnqueue = Random.Range(minTimeToGoalArea, maxTimeToGoalArea); // this shouldn't vary between items
        for (int i = 0; i < eventsToGenerate; ++i)
        {
            string keyToEnqueue = validEvents[Mathf.FloorToInt(Random.Range(0, validEvents.Length))];
            inputSequence.Enqueue(new RhythmEvent(keyToEnqueue, timeToGoalAreaToEnqueue));
        }

        // Create gameobjects where the prompt holder will live
        GameObject promptHolder = new GameObject("QTE Prompt Holder");
        GameObject buttonPrompt = new GameObject("ButtonPrompt");
        closingCirclePrompt = new GameObject("ClosingCircle");
        goalAreaCirclePrompt = new GameObject("GoalCircle");
        buttonPrompt.transform.parent = promptHolder.transform;
        closingCirclePrompt.transform.parent = promptHolder.transform;
        goalAreaCirclePrompt.transform.parent = promptHolder.transform;

        promptHolder.transform.position = new Vector3(blacksmithCenter.position.x, blacksmithCenter.position.y, transform.position.z);

        buttonPromptRenderer = buttonPrompt.AddComponent<SpriteRenderer>();        
        SpriteRenderer closingCircleRenderer = closingCirclePrompt.AddComponent<SpriteRenderer>();
        closingCircleRenderer.sprite = circleSprite;
        closingCircleRenderer.color = closingCircleColor;

        SpriteRenderer goalCircleRenderer = goalAreaCirclePrompt.AddComponent<SpriteRenderer>();
        goalCircleRenderer.sprite = circleSprite;
        goalCircleRenderer.color = goalCircleColor;

        // Set the goal area circle
        goalAreaCircleSize = Mathf.Lerp(0.25f, 1f, goalAreaSize);
        goalAreaCirclePrompt.transform.localScale = new Vector3(goalAreaCircleSize, goalAreaCircleSize, 1f);

        // Show the first button prompt
        DisplayButtonPrompt(inputSequence.Peek());
    }

    public override void ReceiveInput(string buttonPressed)
    {
        base.ReceiveInput(buttonPressed);
         
        if (buttonPressed == inputSequence.Peek().key && closingCircleSize < goalAreaCircleSize)
        {
            inputSequence.Dequeue();

            if (inputSequence.Count == 0)
            {
                if (activatingPlayer)
                {
                    activatingPlayer.GetComponent<BlacksmithController>().qteSucceed();
                }
                Debug.Log("Finished QTE!");
                qteAudioSource.PlayOneShot(eventWinSound);
                ShutdownQTE();
            }
            else 
            {
                DisplayButtonPrompt(inputSequence.Peek());
            }
        }
        else
        {
            // Fail the prompt - stun the player? insta-retry?
            Debug.Log("Failed QTE!");
            FailQTE();
        }
    }

    void DisplayButtonPrompt(RhythmEvent eventToDisplay)
    {
        string buttonToDisplay = eventToDisplay.key;
        initialClosingCircleTimer = closingCircleTimer = eventToDisplay.timeToFail;

        // Show the prompt
        switch(buttonToDisplay)
        {
            case "A":
                buttonPromptRenderer.sprite = buttonSprites[0];
                break;
            case "B":
                buttonPromptRenderer.sprite = buttonSprites[1];
                break;
            case "X":
                buttonPromptRenderer.sprite = buttonSprites[2];
                break;
            case "Y":
                buttonPromptRenderer.sprite = buttonSprites[3];
                break;
            default:
                Debug.Log("What the hell is button " + buttonToDisplay);
                break;
        }
    }

    void FailQTE()
    {
        if (activatingPlayer)
        {
            activatingPlayer.GetComponent<BlacksmithController>().qteFail();
        }
        qteAudioSource.PlayOneShot(eventFailSound);
        ShutdownQTE();
    }

    void ShutdownQTE()
    {
        if (closingCirclePrompt)
        {
            Destroy(closingCirclePrompt.transform.parent.gameObject);
        }
        inputSequence.Clear();
    }
}
