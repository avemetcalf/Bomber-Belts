using UnityEngine;
using System.Collections;

public class AIScript_AveryMetcalf : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;
	bool Move = false;
	float PressB;
	float minPushDistance = 1.0f;
    
	

	void Start () {
        mainScript = GetComponent<CharacterScript>();

        if (mainScript == null)
        {
            print("No CharacterScript found on " + gameObject.name);
            this.enabled = false;
        	}

        buttonLocations = mainScript.getButtonLocations();

        playerSpeed = mainScript.getPlayerSpeed();
		}

		
	void Update(){
        buttonCooldowns = mainScript.getButtonCooldowns();
        beltDirections = mainScript.getBeltDirections();
        playerSpeed = mainScript.getPlayerSpeed();
		buttonLocations = mainScript.getButtonLocations();
		 
			 
		//If we are not moving, find the closest button to push
		if(!Move){
			FindClosestButton();
		
		}else{
			// Move to button and press button
			MoveToButton(PressB);
			}
    	}

	void MoveToButton(float button){
		float minDistance = Mathf.Abs(button - mainScript.getCharacterLocation());
        //If minimum distance is less than the minimum push distance then push the button
		if (minDistance < minPushDistance){
            mainScript.push();
            
            //switch to pushing
			Move = false;
    }else{
			// Depending on character location move up or down and push
		if ((button - mainScript.getCharacterLocation()) > 0){      
                mainScript.moveUp();
				mainScript.push();
    }else if ((button - mainScript.getCharacterLocation()) < 0){
                mainScript.moveDown();			
            	}
        	}
    	}

	// Finding the closest button
    void FindClosestButton(){
        float minDistance = 100f;
        float targetButton = 0;

		//Look for closest button
        for (int j = 0; j < buttonLocations.Length; j++){
			
			// If button is done cooling down and belt is not moving or if belt is moving towards me
     	if (buttonCooldowns[j] <= 0 && (beltDirections[j] == -1 || beltDirections[j] == 0)){
				
				// If button location - character location is less that the minimum distance
		if ((Mathf.Abs(buttonLocations[j] - mainScript.getCharacterLocation())) < minDistance){					
					minDistance = Mathf.Abs(buttonLocations[j] - mainScript.getCharacterLocation());
					targetButton = buttonLocations[j];
                }
            }
        }
		
		// If moving press the closest button
        PressB = targetButton;
		Move = true;
    		}		
		}
