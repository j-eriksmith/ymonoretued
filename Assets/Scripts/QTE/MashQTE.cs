using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashQTE : BaseQTE{
    public int minMashLength;
    public int maxMashLength;

    private string[] validEvents = {"A", "B", "X", "Y"};
    private int pressesRemaining;
    private string mashButton;
    private SpriteRenderer promptRenderer;
    public GameObject mashPrompt;

    public override void Initialize(){
        base.Initialize();
        
        pressesRemaining = Mathf.FloorToInt(Random.Range(minMashLength, maxMashLength));
        mashButton = validEvents[Mathf.FloorToInt(Random.Range(0, validEvents.Length))];

        Debug.Log("mashButton: " + mashButton);
        Debug.Log(pressesRemaining + " presses remaining");

        // Create gameobject where the prompt holder will live
        GameObject promptHolder = new GameObject("QTE Prompt Holder");
        promptHolder.transform.position = new Vector3(blacksmithCenter.position.x, blacksmithCenter.position.y, transform.position.z);
        promptRenderer = promptHolder.AddComponent<SpriteRenderer>();        

        // Show the first button prompt
        DisplayButtonPrompt(mashButton);
    }

    public override void ReceiveInput(string buttonPressed){
        base.ReceiveInput(buttonPressed);

        if(buttonPressed == mashButton){
            --pressesRemaining;
            Debug.Log(pressesRemaining + " presses remaining");
        }

        if(pressesRemaining == 0){
            Debug.Log("Finished QTE!");
            Destroy(promptRenderer.gameObject);
            mashPrompt.SetActive(false);
            if (activatingPlayer)
            {
                activatingPlayer.GetComponent<BlacksmithController>().qteSucceed();
            }
        }
    }

    void DisplayButtonPrompt(string buttonToDisplay){
        mashPrompt.SetActive(true);
        switch(buttonToDisplay){
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
