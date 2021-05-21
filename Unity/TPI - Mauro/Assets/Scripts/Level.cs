using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    List<List<string>> levelData = new List<List<string>>();
    public Level (List<List<string>> levelData)
    {
        this.levelData = levelData;
    }

    public List<List<string>> LevelData { get => levelData;}

}
