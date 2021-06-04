using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// script du gameManager
/// </summary>
public class GameManagerScript : MonoBehaviour
{
    private int gameState = 1;
    [SerializeField]
    CameraScript gameCamera;
    //UI elements
    [SerializeField]
    GameObject levelUi;
    [SerializeField]
    GameObject GameOverUi;
    [SerializeField]
    GameObject mainMenuUi;
    [SerializeField]
    GameObject pauseMenuUi;
    //game UI
    [SerializeField]
    GameObject TimeLabelUi;
    [SerializeField]
    GameObject GameLevelLabelUi;
    [SerializeField]
    GameObject MovesLabelUi;

    //game variables (gamestate 2)
    private List<Level> levels = new List<Level>();
    private int movecounter = 0;
    private float timecounter = 0;
    private bool isPaused = false;
    private int spotsInLevel = 0;
    private int boxesOnSpot = 0;

    //lvlmanager variables (gamestate 2)
    private int levelSelected = -1;
    private bool levelTransition = false;
    private bool restartLevel = false;
    private float transitionTimer=0;
    [SerializeField]
    private GameObject[] prefabs;
    //sound management
    [SerializeField]
    private AudioSource[] audioSources;
    //getters
    public bool RestartLevel { set { restartLevel = value; } }
    public int GameState { get => gameState; } 
    public bool IsPaused { get => isPaused; }
    public int LevelSelected { get => levelSelected; }

    /// <summary>
    /// fonction de base de unity qui est appelée quand l'objet player est instancié
    /// lance le menu principale
    /// </summary>
    void Start()
    {
        ChangeGameState(1);
    }
    /// <summary>
    /// fonction qui me permet de changer le statut du jeu 
    /// </summary>
    /// <param name="stateToChange">1 - menu principal, 2 - jeu , 3 - Gameover</param>
    public void ChangeGameState(int stateToChange)
    {
        clearGameObjects();
        switch (stateToChange)
        {
            case 2:
                try
                {
                    movecounter = 0;
                    timecounter = 0;
                    LoadLevels();
                    levelSelected = -1;
                    levelTransition = true;
                }
                catch (IOException)
                {
                    ChangeGameState(1);
                }
                break;
            case 3:
                GameOverUi.SetActive(true);
                break;
            default:
                mainMenuUi.SetActive(true);
                GameObject.Find("PlayButton").GetComponent<Button>().Select();
                break;
        }
        gameState = stateToChange;
    }
    /// <summary>
    /// Fonction qui supprime tous les games objects utilisés à un seul gamestate
    /// </summary>
    private void clearGameObjects()
    {
        gameCamera.Player = null;
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject singleObject in allObjects)
        {
            if (singleObject.name.Contains("character") | singleObject.name.Contains("Enemy") | singleObject.name.Contains("SpotLocator") | singleObject.name.Contains("Box") | singleObject.name.Contains("BrickWall") | singleObject.name.Contains("Teleporter"))
            {
                Destroy(singleObject);
            }
        }
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUi.SetActive(false);

        mainMenuUi.SetActive(false);
        levelUi.SetActive(false);
        GameOverUi.SetActive(false);
    }
    /// <summary>
    /// fonction de base de unity qui est appelée à chaque image
    /// appele la fonction qui gere le comportement du jeu
    /// </summary>
    void Update()
    {
        UpdateGameState();
        
    }
    /// <summary>
    /// Fonction appelée par la fonction update permettant d'avoir un comportement different en fonction du game status
    /// </summary>
    private void UpdateGameState()
    {
        switch (gameState)
        {
            case 2:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    restartLevel = true;
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    TogglePause();
                }
                if (levelTransition)
                {
                    clearGameObjects();
                    levelSelected++;
                    levelUi.SetActive(true);
                    GameObject.Find("LevelLable").GetComponent<TextMeshProUGUI>().text = "Level " + levelSelected;
                    GameLevelLabelUi.GetComponent<TextMeshProUGUI>().text = "Level : " + levelSelected;
                    transitionTimer = 0;
                    levelTransition = false;
                }
                if (levelUi.activeSelf & transitionTimer > 2)
                {
                    levelUi.SetActive(false);
                    levelTransition = false;
                    InstantiateLevel();
                }
                if (restartLevel)
                {
                    clearGameObjects();
                    InstantiateLevel();
                    if (IsPaused) TogglePause();
                    restartLevel = false;
                }
                timecounter += Time.deltaTime;
                TimeLabelUi.GetComponent<TextMeshProUGUI>().text = "Time : " + Mathf.Floor(timecounter) + " secs";
                transitionTimer += Time.deltaTime;
                break;
            case 3:
                if (Input.anyKeyDown)
                {
                    ChangeGameState(1);
                }
                break;
        }

    }
    /// <summary>
    /// Fonction appelée par je personnage principal qui incremente ne compteur de pas 
    /// </summary>
    public void AddMoveCounter()
    {
        movecounter++;
        MovesLabelUi.GetComponent<TextMeshProUGUI>().text = movecounter + " Moves";
    }
    /// <summary>
    /// Fonction qu'instantie les elements d'un niveau dans les bons endroits
    /// </summary>
    void InstantiateLevel()
    {
        int x = 0, z = 0;
        boxesOnSpot = 0;
        spotsInLevel = 0;
        GameObject teleporter1 = null, teleporter2 = null;
        foreach (List<string> line in levels[levelSelected].LevelData)
        {
            foreach(string square in line)
            {
                if (int.TryParse(square, out int type))
                {
                    GameObject gameObject = Instantiate(prefabs[type]);
                    switch (type)
                    {
                        //wall
                        case 1:
                            gameObject.transform.position = new Vector3(-1 * (x + 0.5f), 0.5f, (z + 0.5f) * -1);
                            break;
                        //box
                        case 2:
                            gameObject.transform.position = new Vector3(-1 * (x + 0.5f), 0, (z + 0.5f) * -1);
                            break;
                        //tp
                        case 3:
                            if (teleporter1==null)
                            {
                                teleporter1 = gameObject;
                            }
                            else
                            {
                                teleporter2 = gameObject;
                            }
                            gameObject.transform.position = new Vector3(-1 * (x + 0.5f), 0.5f, (z + 0.2f) * -1);
                            break;
                        //greenpoint
                        case 4:
                            spotsInLevel++;
                            gameObject.transform.position = new Vector3(-1 * (x + 0.5f), 0.5f, (z + 0.5f) * -1);
                            break;
                        //player
                        case 0:
                            gameObject.transform.position = new Vector3(-1 * (x + 0.5f), 0, (z + 0.5f) * -1);
                            gameCamera.Player = gameObject;
                            break;
                    }
                }
                z++;
            }
            x++;
            z = 0;
        }
        if(teleporter1 != null)
        {
            if(teleporter2 != null) 
            {
                teleporter1.GetComponent<TeleporterScript>().LinkToTeleporter(teleporter2);
                teleporter2.GetComponent<TeleporterScript>().LinkToTeleporter(teleporter1);
            }
            else
            {
                teleporter1.GetComponent<TeleporterScript>().LinkToTeleporter(teleporter1);
            }
            
        }
    }
    /// <summary>
    /// Charge les differents niveaux depuis les fichiers csv
    /// </summary>
    void LoadLevels()
    {
        int levelindex = 0;
        while (File.Exists(levelindex + ".csv"))
        {
            List<List<string>> formatedData = new List<List<string>>();
            int index = 0;
            string data = File.ReadAllText(levelindex + ".csv");
            string[] lines = data.Split("\n"[0]);
            foreach (string line in lines)
            {
                formatedData.Add(new List<string>());
                string[] lineData = line.Trim().Split(","[0]);
                foreach (string item in lineData)
                {
                    formatedData[index].Add(item);
                }
                index++;
            }
            
            levelindex++;
            levels.Add(new Level(formatedData));
        }
    }
    /// <summary>
    /// fonction appellée quand une boite interagit avec un point vert
    /// </summary>
    /// <param name="value">+1 pour quand la boite entre,-1 quand la boite sort</param>
    public void BoxOnSpot(int value)
    {
        boxesOnSpot += value;
        if (boxesOnSpot == spotsInLevel)
        {
            if (levelSelected < levels.Count - 1)
            {
                levelTransition = true;
                PlaySound(2);
            }
            else
            {
                ChangeGameState(3);
            }
            
        }
    }
    /// <summary>
    /// fonction qui mets/enleve la pause du jeu
    /// </summary>
    public void TogglePause()
    {
        PauseScript[] pause = Resources.FindObjectsOfTypeAll<PauseScript>();
        if (isPaused)
        {
            if (pause[0].gameObject.activeSelf)
            {
                Time.timeScale = 1f;
                pause[0].gameObject.SetActive(false);
                isPaused = false;
            }
        }
        else
        {
            Time.timeScale = 0f;
            pause[0].gameObject.SetActive(true);
            GameObject.Find("ResumeButton").GetComponent<Button>().Select();
            isPaused = true;
        }
    }
    /// <summary>
    /// Joue les sons du jeu
    /// </summary>
    /// <param name="soundID">0- marche du personnage, 1-deplacement de la boite, 2 - Fin de niveau</param>
    public void PlaySound(int soundID)
    {
        
        audioSources[soundID].Play();
    }
}
