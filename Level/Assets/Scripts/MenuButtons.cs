using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GunStats blunder;
    public TextMeshProUGUI dialogue;
    int currentPosition;
    int start = 0;

    public void Resume()
    {
        gameManager.instance.cursorUnlockUnpause();
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.isPaused = false;
        if (!gameManager.instance.crossHairVisible)
        {
            gameManager.instance.crossHairVisible = !gameManager.instance.crossHairVisible;
            gameManager.instance.Crosshair.SetActive(gameManager.instance.crossHairVisible);
        }
    }
    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Restart()
    {
        gameManager.instance.cursorUnlockUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Return()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
    public void respawn()
    {
        gameManager.instance.playerScript.respawn();
        gameManager.instance.cursorUnlockUnpause();
    }
    public void quit()
    {
        Application.Quit();
        Debug.Log("Game has ended");
    }

    public void WhereAmI()
    {
        dialogue.text = "Look around you! Paradise!";
    }

    public void Shop()
    {
        gameManager.instance.hint.SetActive(false);
        gameManager.instance.npcDialogue.SetActive(false);
        gameManager.instance.shopInventory.SetActive(true);
    }

    public void Bye()
    {
        gameManager.instance.npcDialogue.SetActive(false);
        gameManager.instance.cursorUnlockUnpause();
        dialogue.text = "What's that smell... Sniff Sniff... Huh I think that's me... Oh Hi! What can I do for you today?";
    }

    public void Left()
    {
        currentPosition = (int)gameManager.instance.shopPanels.transform.position.x;

        if (currentPosition < 0)
        {
            Debug.Log(currentPosition);
            gameManager.instance.shopPanels.transform.Translate(1619, 0, 0);
        }
    }

    public void BuyGun()
    {
        if(gameManager.instance.currencyNumber >= 10)
        {
            gameManager.instance.playerScript.GunPickup(blunder);
            gameManager.instance.currencyNumber -= 10;
        }
        else
        {
            return;
        }
    }

    public void BuyAmmo()
    {
        if (gameManager.instance.currencyNumber >= 2 && gameManager.instance.ammoCount != 5)
        {
            int ammoGone = 5 - gameManager.instance.ammoCount;

            if (ammoGone <= 0)
            {
                gameManager.instance.ammoCount = 5;
            }
            else
            {
                gameManager.instance.ammoCount += ammoGone;
            }

            gameManager.instance.currencyNumber -= 2;
        }
    }

    public void BuyHealth()
    {
        if (gameManager.instance.currencyNumber >= 2 && gameManager.instance.playerHPBar.fillAmount != 1)
        {
            float healthGone = 1 - gameManager.instance.playerHPBar.fillAmount;

            if (healthGone <= .5f)
            {
                gameManager.instance.playerHPBar.fillAmount += .5f;
            }
            else
            {
                gameManager.instance.playerHPBar.fillAmount += healthGone;
            }

            gameManager.instance.currencyNumber -= 2;
        }
    }

    public void NoBuy()
    {
        gameManager.instance.npcDialogue.SetActive(true);
        gameManager.instance.shopInventory.SetActive(false);
    }

    public void Right()
    {
        currentPosition = (int)gameManager.instance.shopPanels.transform.position.x;

        if (currentPosition > -690)
        {
            Debug.Log(currentPosition);
            gameManager.instance.shopPanels.transform.Translate(-1619, 0, 0);
        }
    }
   
}
