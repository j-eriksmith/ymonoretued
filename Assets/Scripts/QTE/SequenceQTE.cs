using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceQTE : BaseQTE
{
    public int minSequenceLength;
    public int maxSequenceLength;
    public Transform blacksmithLeft;
    public float spaceBetweenButtonInputs;

    private string[] validEvents = {"A", "B", "X", "Y"};
    private Queue<string> inputSequence = new Queue<string>();
    private int inputsCompleted;
    private GameObject grandPromptHolder;
    private List<SpriteRenderer> promptRenderers;

    public override void Initialize()
    {
        base.Initialize();
        promptRenderers = new List<SpriteRenderer>();
        inputsCompleted = 0;

        int eventsToGenerate = Mathf.FloorToInt(Random.Range(minSequenceLength, maxSequenceLength + 1));

        // Create gameobject where the prompt holder will live
        grandPromptHolder = new GameObject("QTE Prompt Holder");
        grandPromptHolder.transform.position = new Vector3(blacksmithLeft.position.x, blacksmithLeft.position.y, transform.position.z);

        float shiftingButtonPlacement = 0.0f;
        for (int i = 0; i < eventsToGenerate; ++i)
        {
            string eventToEnqueue = validEvents[Mathf.FloorToInt(Random.Range(0, validEvents.Length))];
            inputSequence.Enqueue(eventToEnqueue);            

            GameObject promptHolder = new GameObject();
            promptHolder.transform.parent = grandPromptHolder.transform;
            promptHolder.transform.localPosition = new Vector3(shiftingButtonPlacement, 0, -1);
            promptRenderers.Add(promptHolder.AddComponent<SpriteRenderer>());
            DisplayButtonPrompt(eventToEnqueue, promptRenderers[promptRenderers.Count - 1]);

            shiftingButtonPlacement+= spaceBetweenButtonInputs;
        }

    }

    public override void ReceiveInput(string buttonPressed)
    {
        base.ReceiveInput(buttonPressed);

        if (buttonPressed == inputSequence.Peek())
        {
            inputSequence.Dequeue();

            if (inputSequence.Count == 0)
            {
                activatingPlayer.GetComponent<BlacksmithController>().qteSucceed();
                Debug.Log("Finished QTE!");
                Destroy(grandPromptHolder);
            }
            else 
            {
                // Hide the front of the sequence    
                promptRenderers[inputsCompleted].enabled = false;
                inputsCompleted++;
            }
        }
        // No else case for now ... gotta finish this game
    }

    void DisplayButtonPrompt(string buttonToDisplay, SpriteRenderer promptRenderer)
    {
        switch(buttonToDisplay)
        {
            case "A":
                promptRenderer.sprite = buttonSprites[0];
                break;
            case "B":
                promptRenderer.sprite = buttonSprites[1];
                break;
            case "X":
                promptRenderer.sprite = buttonSprites[2];
                break;
            case "Y":
                promptRenderer.sprite = buttonSprites[3];
                break;
            default:
                Debug.Log("What the hell is button " + buttonToDisplay);
                break;
        }
    }
}
