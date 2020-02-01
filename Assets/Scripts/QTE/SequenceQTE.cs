using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceQTE : BaseQTE
{
    public int minSequenceLength;
    public int maxSequenceLength;

    private string[] validEvents = {"A", "B", "X", "Y"};
    private Queue<string> inputSequence = new Queue<string>();
    private SpriteRenderer promptRenderer;

    public override void Initialize()
    {
        base.Initialize();

        int eventsToGenerate = Mathf.FloorToInt(Random.Range(minSequenceLength, maxSequenceLength + 1));
        for (int i = 0; i < eventsToGenerate; ++i)
        {
            string eventToEnqueue = validEvents[Mathf.FloorToInt(Random.Range(0, validEvents.Length))];
            inputSequence.Enqueue(eventToEnqueue);            
        }

        // Create gameobject where the prompt holder will live
        GameObject promptHolder = new GameObject("QTE Prompt Holder");
        promptHolder.transform.position = blacksmithCenter.position;
        promptRenderer = promptHolder.AddComponent<SpriteRenderer>();        

        // Show the first button prompt
        DisplayButtonPrompt(inputSequence.Peek());
    }

    public override void ReceiveInput(string buttonPressed)
    {
        base.ReceiveInput(buttonPressed);

        if (buttonPressed == inputSequence.Peek())
        {
            inputSequence.Dequeue();

            if (inputSequence.Count == 0)
            {
                // TODO(smith): Call Player.QTESucceed()
                Debug.Log("Finished QTE!");
                Destroy(promptRenderer.gameObject);
            }
            else 
            {
                DisplayButtonPrompt(inputSequence.Peek());
            }
        }
        // No else case for now ... gotta finish this game
    }

    void DisplayButtonPrompt(string buttonToDisplay)
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
