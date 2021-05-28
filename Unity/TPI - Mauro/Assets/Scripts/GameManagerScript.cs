using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using TMPro;

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

    public int GameState { get => gameState; } 
    public bool IsPaused { get => isPaused; }
    public int LevelSelected { get => levelSelected; }
    /// <summary>
    /// fonction de base de unity qui est appelée quand l'objet player est instancié
    /// lance le menu principale
    /// </summary>
    void Start()
    {
        ChangeGameState(2);
    }

    public void ChangeGameState(int stateToChange)
    {
        clearGameObjects();
        switch (stateToChange)
        {
            
            case 2:
                try
                {
                    LoadLevels();
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
                break;
        }
        gameState = stateToChange;
    }
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
    private void UpdateGameState()
    {
        switch (gameState)
        {
            case 2:
                if (Input.GetKeyDown(KeyCode.R))
                {
                    restartLevel = true;
                }
                if (levelTransition)
                {
                    clearGameObjects();
                    levelSelected++;
                    levelUi.SetActive(true);
                    GameObject.Find("LevelLable").GetComponent<TextMeshProUGUI>().text = "Level " + levelSelected;
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
                    restartLevel = false;
                }
                transitionTimer += Time.deltaTime;
                break;
        }

    }
    public void AddMoveCounter()
    {
        movecounter++;

    }
    /// <summary>
    /// 
    /// 
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
    /// 
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
    public void BoxOnSpot(int value)
    {
        boxesOnSpot += value;
        if (boxesOnSpot == spotsInLevel)
        {
            if (levelSelected < levels.Count - 1)
            {
                levelTransition = true;
            }
            else
            {
                ChangeGameState(3);
            }
        }
    }
}
