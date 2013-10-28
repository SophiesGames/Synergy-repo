using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Singleton to hold game information and control level switching.
/// </summary>
public class GameManagerC : MonoBehaviour
{
    public GameObject MusicManagerPrefab;
    
    [HideInInspector]
    public string userID;//Used for grouping metrics based of time of play

    [HideInInspector]
    public bool isConsoleActive;

    /// <Playtomics variables>
    /// Playtomics variables
    /// </summary>
    /// 
    ////release metrics
    private int gameid = 972551;
    private string guid = "0ed2b99fc8784b8b";
    private string apikey = "db3fa4092054440e83e946d866acd3";

    //workingcopy metrics
    //private int gameid = 951792;
    //private string guid = "beca8323786e4316";
    //private string apikey = "e12e65b837eb43d7bfc5ded71a3ac5";

    private static GameManagerC _gameManager = null;//script instance that will be used for this class
    private static GameObject _gameManagerTemplate = null;

    public int gameID
    {
        get
        {
            return gameid;
        }
    }

    public static GameObject gameManagerTemplate
    {
        set
        {
            if (_gameManagerTemplate == null)     //if it doesnt already exist
            {
                _gameManagerTemplate = value;     //store the new value
                Instantiate(_gameManagerTemplate); //fill it with the tempalte passed from the main script
            }
        }
    }
    public static GameManagerC gameManager
    {
        get
        {
            return _gameManager;
        }
    }

    public void Awake()
    {
        Application.targetFrameRate = 25;

        GetComponent<HUDFPS>().enabled = Debug.isDebugBuild;

        if (gameManager == null) //if the gameManger function returns null
        {
            _gameManager = this;    //make the return of that function = this instance
        }

        System.DateTime saveUtcNow = System.DateTime.UtcNow;//combine this line with next line
        userID = saveUtcNow.ToString();                     //create unique user id based on system universal time

        Playtomic.Initialize(gameid, guid, apikey);
        Playtomic.Log.View();

        DontDestroyOnLoad(this.gameObject);                     //should keep everything (But not children)
		
		Instantiate(MusicManagerPrefab);//creates the msuic manager. eventManager then handles new tracks on new levels

    }

    private void Update()
    {
        if (Input.GetButtonUp("DeveloperConsole"))
        {
            isConsoleActive = !isConsoleActive;
        }

        if (Input.GetButtonUp("New"))
        {
            NewGame();
        }

        if (Input.GetButtonUp("Reset"))
        {
            ReloadLevel();
        }

        if (Input.GetButtonUp("Mute"))
        {
            if (AudioListener.volume == 0) AudioListener.volume = 1;
            else AudioListener.volume = 0;
        }
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("levelNumber", 0);
        LoadLevelNumber(0);
    }

    public void ReloadLevel()
    {
        Playtomic.Log.LevelCounterMetric("Restarts", LevelManager.levelManager.levelDetails);
        LoadLevelNumber(Application.loadedLevel);
    }

    public void NextLevel()
    {
        int nextLevel = Application.loadedLevel;
        nextLevel++;
        LoadLevelNumber(nextLevel);
    }

    public void LoadLevelNumber(int levelToLoad)
    {
        MusicManager.musicManager.audio.volume = 1.0f; //reset sound to full
        PlayerPrefs.SetInt("levelNumber", levelToLoad);
        Playtomic.Log.LevelRangedMetric("CorruptedTiles", LevelManager.levelManager.levelDetails, LevelManager.levelManager.corruptedObjects.Count);
        Playtomic.Log.LevelAverageMetric("Time", LevelManager.levelManager.levelDetails, LevelManager.levelManager.levelPlayTime);
        Application.LoadLevel(levelToLoad);
    }
}