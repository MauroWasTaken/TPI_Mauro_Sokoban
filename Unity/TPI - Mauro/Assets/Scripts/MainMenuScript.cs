using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// classe qui gere le menu principal du jeu
/// </summary>
public class MainMenuScript : MonoBehaviour
{
    /// <summary>
    /// fonction que lance le jeu 
    /// </summary>
    public void PlayGame()
    {
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
        gameScript.ChangeGameState(2);
    }
    /// <summary>
    /// fonction permettant d'ouvrir le menu options
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
    /// fonction permettant de quitter le jeu
    /// </summary>
    public void CloseGame()
    {
        GameManagerScript gameScript = UnityEngine.Object.FindObjectOfType<GameManagerScript>();
        Application.Quit();
    }
}
