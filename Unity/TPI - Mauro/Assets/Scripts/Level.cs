using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe level utilisée pour stoquer les information de chaque niveau du jeu
/// </summary>
public class Level
{
    List<List<string>> levelData = new List<List<string>>();
    public Level (List<List<string>> levelData)
    {
        this.levelData = levelData;
    }

    public List<List<string>> LevelData { get => levelData;}

}
