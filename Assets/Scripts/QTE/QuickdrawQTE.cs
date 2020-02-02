using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickdrawQTE : BaseQTE
{
    public class QuickdrawEvent
    {
        public string key;
        public float timeToStart;
        public float timeToClose;
        public QuickdrawEvent(string inKey, float inTimeToStart, float inTimeToClose)
        {
            key = inKey;
            timeToStart = inTimeToStart;
            timeToClose = inTimeToClose;
        }
    }

    public float minTimeToClose;
    public float maxTimeToClose;

    public float minTimeToStart;
    public float maxTimeToStart;

    public Sprite circleSprite;
    public Color circleColor;

    public GameObject helpPrompt;

    private string[] validEvents = {"A", "B", "X", "Y"};
    private QuickdrawEvent quickdrawEvent;
    private SpriteRenderer buttonPromptRenderer;
    private SpriteRenderer closingCircleRenderer;
    private GameObject closingCirclePrompt;

    private float closingCircleTimer;
    private float initialClosingCircleTimer;

    private float timeToDraw;

    private float closingCircleSize;

    new public void Update()
    {
        base.Update();

        if (quickdrawEvent == null) return; // not initialized yet & unlikely chance of landing exactly on 0.0f

        // Decrement the waiting time
        if (timeToDraw > 0.0f)
        {
            timeToDraw -= Time.deltaTime;
        }

        if (timeToDraw <= 0.0f)
        {
            // Unhide the button prompt and the closing circle
            if (!buttonPromptRenderer.enabled)
            {
                buttonPromptRenderer.enabled = true;
                closingCircleRenderer.enabled = true;
                helpPrompt.GetComponent<TextMesh>().text = "DRAW!";
            }

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

    }

    public override void Initialize()
    {
        base.Initialize();
        
        float timeToStartToEnqueue = Random.Range(minTimeToStart, maxTimeToStart);
        float timeToCloseToEnqueue = Random.Range(minTimeToClose, maxTimeToClose);
        string keyToEnqueue = validEvents[Mathf.FloorToInt(Random.Range(0, validEvents.Length))];
        quickdrawEvent = new QuickdrawEvent(keyToEnqueue, timeToStartToEnqueue, timeToCloseToEnqueue);

        // Create gameobjects where the prompt holder will live
        GameObject promptHolder = new GameObject("QTE Prompt Holder");
        GameObject buttonPrompt = new GameObject("ButtonPrompt");
        closingCirclePrompt = new GameObject("ClosingCircle");
        closingCirclePrompt.transform.parent = promptHolder.transform;
        buttonPrompt.transform.parent = promptHolder.transform;

        promptHolder.transform.position = blacksmithCenter.position;
        promptHolder.transform.position = new Vector3(blacksmithCenter.position.x, blacksmithCenter.position.y, transform.position.z);

        buttonPromptRenderer = buttonPrompt.AddComponent<SpriteRenderer>();        
        buttonPromptRenderer.enabled = false;

        closingCircleRenderer = closingCirclePrompt.AddComponent<SpriteRenderer>();
        closingCircleRenderer.sprite = circleSprite;
        //closingCircleRenderer.color = circleColor;
        closingCircleRenderer.enabled = false;

        // Show the first button prompt
        DisplayButtonPrompt(quickdrawEvent);
    }

    public override void ReceiveInput(string buttonPressed)
    {
        base.ReceiveInput(buttonPressed);
         
        if (timeToDraw <= 0.0f && closingCircleTimer >= 0.0f && buttonPressed == quickdrawEvent.key)
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
            // Fail the prompt - stun the player? insta-retry?
            Debug.Log("Failed QTE!");
            FailQTE();
        }
    }

    void DisplayButtonPrompt(QuickdrawEvent eventToDisplay)
    {
        string buttonToDisplay = eventToDisplay.key;
        initialClosingCircleTimer = closingCircleTimer = eventToDisplay.timeToClose;
        timeToDraw = eventToDisplay.timeToStart;
        helpPrompt.GetComponent<TextMesh>().text = "Wait...";
        helpPrompt.SetActive(true);

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
        quickdrawEvent = null;
        helpPrompt.SetActive(false);
    }
}
