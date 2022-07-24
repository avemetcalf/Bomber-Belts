using UnityEngine;
using System.Collections;

public class AIScript_AveryMetcalf : MonoBehaviour {

    public CharacterScript mainScript;

    public float[] bombSpeeds;
    public float[] buttonCooldowns;
    public float playerSpeed;
    public int[] beltDirections;
    public float[] buttonLocations;
	
	
	
    float minPushDistance = 1.0f;
    float buttonToPress;
	bool isMoving = false;

	
	
	// Use this for initialization
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
        
		//If were not moving, push a button
		if(!isMoving){
			FindClosestButton();
		//Otherwise move to a button to push
		}else{
			MoveToButton(buttonToPress);
		}
    }

    void MoveToButton(float button){
		float minDistance = Mathf.Abs(button - mainScript.getCharacterLocation());
        //If minimum distance is less than the minimum push distance then push the button
		if (minDistance < minPushDistance){
            mainScript.push();
            
            //switch to pushing
			isMoving = false;
        }else{
			// Depending on character location move up or down and push
			if ((button - mainScript.getCharacterLocation()) > 0){      
                mainScript.moveUp();
				mainScript.push();
            }else if ((button - mainScript.getCharacterLocation()) < 0){
                mainScript.moveDown();
				mainScript.push();
            }
        }
    }

	// Finding the closest button
    void FindClosestButton(){
        float minDistance = 100f;
        float targetButton = 0;

		//Look for closest button
        for (int i = 0; i < buttonLocations.Length; i++){
			
			// If button is done cooling down and belt is not moving or if belt is moving towards me
            if (buttonCooldowns[i] <= 0 && (beltDirections[i] == -1 || beltDirections[i] == 0)){
				
				// If button location - character location is less that the minimum distance
				if ((Mathf.Abs(buttonLocations[i] - mainScript.getCharacterLocation())) < minDistance){
					
					minDistance = Mathf.Abs(buttonLocations[i] - mainScript.getCharacterLocation());
					targetButton = buttonLocations[i];
                }
            }
        }

		// If moving press the closest button
        buttonToPress = targetButton;
		isMoving = true;
    }		
}
