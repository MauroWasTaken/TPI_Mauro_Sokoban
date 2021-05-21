using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameManagerScript : MonoBehaviour
{
    private int gameState = 2;
    [SerializeField]
    GameObject gameCamera;
    //game variables
    private List<Level> levels = new List<Level>();
    private int movecounter = 0;
    private float timecounter = 0;
    private bool isPaused = false;
    private bool nextLevel = false;
    //lvlmanager variables
    private int levelSelected = 0;
    [SerializeField]
    private float transitionTime=2;
    private float transitionTimer;
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
        LoadLevels();
        //levels.Add(new Level(testLvl()));
        InstantiateLevel();
    }
    List<List<string>> testLvl()
    {
        List<List<string>> result = new List<List<string>>();
        List<string> testLine1 = new List<string>();
        testLine1.Add("");
        testLine1.Add("0");
        testLine1.Add("1");
        testLine1.Add("2");
        testLine1.Add("3");
        testLine1.Add("4");
        testLine1.Add("");
        result.Add(testLine1);
        result.Add(testLine1);
        return result;
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
    /// 
    /// state 1 
    /// </summary>
    /// <param name="stateToChange"></param>
    public void ChangeGameState(int stateToChange)
    {
        switch (stateToChange)
        {
            default:
                break;
        }
    }
    private void UpdateGameState()
    {
        switch (gameState)
        {
            case 2:
                
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
                            gameObject.transform.position = new Vector3(-1 * (x + 0.5f), 0.5f, (z + 0.5f) * -1);
                            break;
                        //player
                        case 0:
                            gameObject.transform.position = new Vector3(-1 * (x + 0.5f), 0, (z + 0.5f) * -1);
                            break;
                    }
                }
                z++;
            }
            x++;
            z = 0;
        }
        if(teleporter1 != null & teleporter2 != null)
        {
            teleporter1.GetComponent<TeleporterScript>().LinkToTeleporter(teleporter2);
            teleporter2.GetComponent<TeleporterScript>().LinkToTeleporter(teleporter1);
        }
    }
    void LoadLevels()
    {
        int levelindex = 0;
        while (File.Exists(levelindex + ".csv"))
        {
            string data = File.ReadAllText(levelindex + ".csv");
            string[] lines = data.Split("\n"[0]);
            List<List<string>> formatedData = new List<List<string>>();
            int index = 0;
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
}
