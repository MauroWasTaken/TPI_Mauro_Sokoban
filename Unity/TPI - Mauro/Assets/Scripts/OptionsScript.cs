using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// Classe qui gere le menu options
/// </summary>
public class OptionsScript : MonoBehaviour
{
    /// <summary>
    /// Fonction unity appelée quand la valeur du slider est changée
    /// </summary>
    public void SliderUpdate()
    {
        AudioListener.volume = GameObject.Find("VolumeSlider").GetComponent<Slider>().value;
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
    }
    /// <summary>
    /// fonction que nous fait retourner dans le menu précédent
    /// </summary>
    public void GoBack()
    {
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
        this.gameObject.SetActive(false);
        if (gameScript.GameState == 1)
        {
            MainMenuScript[] mainMenuScripts = Resources.FindObjectsOfTypeAll<MainMenuScript>();
            mainMenuScripts[0].gameObject.SetActive(true);
            GameObject.Find("PlayButton").GetComponent<Button>().Select();
        }
        else
        {
            PauseScript[] pauseMenu = Resources.FindObjectsOfTypeAll<PauseScript>();
            pauseMenu[0].gameObject.SetActive(true);
            GameObject.Find("ResumeButton").GetComponent<Button>().Select();
        }

    }
}
