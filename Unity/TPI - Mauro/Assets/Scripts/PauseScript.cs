using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// classe qui gere le menu pause
/// </summary>
public class PauseScript : MonoBehaviour
{
    /// <summary>
    /// permets de retourner dans la partie en cours
    /// </summary>
    public void ResumeGame()
    {
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
        gameScript.TogglePause();
    }
    /// <summary>
    /// permets de recommencer une partie
    /// </summary>
    public void RestartGame()
    {
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
        gameScript.RestartLevel = true; 
        Time.timeScale = 1f;

    }
    /// <summary>
    /// permets d'ouvrir le menu options
    /// </summary>
    public void OpenOptions()
    {
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
        OptionsScript[] options = Resources.FindObjectsOfTypeAll<OptionsScript>();
        options[0].gameObject.SetActive(true);
        GameObject.Find("GoBackButton").GetComponent<Button>().Select();
        this.gameObject.SetActive(false);

    }
    /// <summary>
    /// permets retourner dans le menu
    /// </summary>
    public void CloseGame()
    {
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
        gameScript.ChangeGameState(1);
    }
}
