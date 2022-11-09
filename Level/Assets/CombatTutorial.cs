using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TutorialManager.instance.objectivesComplete = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectiveCheck();
    }

    public void ObjectiveCheck()
    {
        if (TutorialManager.instance.meleeTrigger == true)
        {
            if (TutorialManager.instance.objectivesComplete == 2)
            {
                TutorialManager.instance.beginButton.SetActive(false);
                TutorialManager.instance.continueButton.SetActive(true);
                TutorialManager.instance.tutorialProgress = 4;

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                gameManager.instance.cameraScript.enabled = false;
                TutorialManager.instance.objectiveText.text = "That was a swing and a hit, but perhaps a little close for comfort. Let's move onto some ranged attacks.";

                TutorialManager.instance.objectivesComplete = 0;
            }
        }
        else if(TutorialManager.instance.rangedTrigger == true)
        {
            if (TutorialManager.instance.objectivesComplete == 2)
            {
                TutorialManager.instance.beginButton.SetActive(false);
                TutorialManager.instance.completeButton.SetActive(true);
                TutorialManager.instance.tutorialProgress = 5;

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                gameManager.instance.cameraScript.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialManager.instance.meleeTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TutorialManager.instance.meleeTrigger = false;
    }
}
