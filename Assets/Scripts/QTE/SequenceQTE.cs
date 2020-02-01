using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceQTE : BaseQTE
{
    public int minSequenceLength;
    public int maxSequenceLength;

    private string[] validEvents = {"A", "B", "X", "Y"};
    private Queue<string> inputSequence = new Queue<string>();

    public override void Initialize()
    {
        base.Initialize();

        int eventsToGenerate = Mathf.FloorToInt(Random.Range(minSequenceLength, maxSequenceLength + 1));
        for (int i = 0; i < eventsToGenerate; ++i)
        {
            string eventToEnqueue = validEvents[Mathf.FloorToInt(Random.Range(0, validEvents.Length))];
            inputSequence.Enqueue(eventToEnqueue);            
            Debug.Log(eventToEnqueue);
        }
    }

    public override void ReceiveInput(string buttonPressed)
    {
        Debug.Log(buttonPressed);
        base.ReceiveInput(buttonPressed);

        if (buttonPressed == inputSequence.Peek())
        {
            inputSequence.Dequeue();
            // TODO(smith): Hide the current button sprite

            if (inputSequence.Count == 0)
            {
                // TODO(smith): Call Player.QTESucceed()
                Debug.Log("Finished QTE!");
            }
        }
        // No else case for now ... gotta finish this game
    }

    protected override void DisplayGraphics()
    {
        
    }
}
