using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// All the information needed for a given level. (Use event manger to make thigns happen in the level.)
/// </summary>
public class LevelManager : MonoBehaviour
{

	private int tileDeathPerMissingLive = 3;

    private static LevelManager _LevelManager;
    public static LevelManager levelManager
    {
        get
        {
            if (_LevelManager == null)
            {
                _LevelManager = new GameObject("LevelManager").AddComponent<LevelManager>();
            }
            return _LevelManager;
        }
    }
    private int timeAtLevelStart = 0;

    public int levelPlayTime
    {
        get
        {
            int levelPlayTime = (int)Time.realtimeSinceStartup - timeAtLevelStart;
            return levelPlayTime;
        }
    }

    private string _LevelName;
    public string levelDetails
    {
        get
        {
            _LevelName = ""; //null
            _LevelName = Application.loadedLevelName + ":" + Application.loadedLevel.ToString() +" " + GameManagerC.gameManager.gameID;
            return _LevelName;
        }
    }

    private int _sceneWidthInUnits;
    public int sceneWidthInUnits
    {
        get
        {
            int pixelsPerUnit = 30;

            _sceneWidthInUnits = Screen.width / pixelsPerUnit;
            return _sceneWidthInUnits;
        }
    }

    public int sceneHeightInUnits
    {
        get
        {
            int pixelsPerUnit = 30;

            _sceneHeightInUnits = Screen.height / pixelsPerUnit;
            return _sceneHeightInUnits;
        }
    }
    private int _sceneHeightInUnits;

    private List<GameObject> _InteractableObjects = new List<GameObject>();
    public List<GameObject> interactableObjects
    {
        get
        {
            return _InteractableObjects;
        }
    }

    //lists all the corrupted objects
    private List<GameObject> _CorruptedObjects = new List<GameObject>();
    public List<GameObject> corruptedObjects
    {
        get
        {
            return _CorruptedObjects;
        }
    }

    //lists all the nonCorrupted objects
    private List<GameObject> _NonCorruptedObjects = new List<GameObject>();
    public List<GameObject> nonCorruptedObjects
    {
        get
        {
            return _NonCorruptedObjects;
        }
    }

    //Used for creating a list of certain objects for specific heals
    private List<GameObject> _GroupOfObjects = new List<GameObject>();
    public List<GameObject> groupOfObjects
    {
        get
        {
            return _GroupOfObjects;
        }
    }

    public bool healingFinished = true;

	
	List<InteractableObject> CanStartCorruptList = new List<InteractableObject> ();

	private void Awake()
    {
        foreach (InteractableObject obj in GameObject.FindObjectsOfType(typeof(InteractableObject)))
        {
            _InteractableObjects.Add(obj.gameObject);                   //fills a list full of all the interactive objects
            _NonCorruptedObjects.Add(obj.gameObject);                  //Fills the list at start full of all the objects. Ones that start corrupted will be taken of in Start() on Interactable object after.

			if (obj.canStartCorrupt)
			{
				CanStartCorruptList.Add(obj);
			}
        }

        EventManager.eventManager.enabled = true;         //creates event manager
        StartLevelTimer();
    }

	//use the start fucntion so it can tell grass to corrupt which tells teh levelmanager to do stuff.
	//by start the level amanger already exists so it should work
	private void Start()
	{
		int levelsCorrupt = 4 - PlayerPrefs.GetInt("lives");
		int numbStartingCorrupt = levelsCorrupt * tileDeathPerMissingLive;

		//if there are more to corrup than what is tagged tehn play safe and make them equal
		if (numbStartingCorrupt > CanStartCorruptList.Count) 
		{
			numbStartingCorrupt = CanStartCorruptList.Count; 
			//TODO: developer logs this issue
		}

		//sets random tiles as corrupt if mareked as safe in editor and if previous levels were done incorrectly
		for (int i = 1; i < numbStartingCorrupt; i++)
		{
			numbStartingCorrupt--;
			//tell it to corrupt on new object created becasue the lsit lost reference. (other way around casues issues becasue 
			CanStartCorruptList[i].StartsCorrupt();
		}

	}
		
	public void StartLevelTimer()
    {
        timeAtLevelStart = (int)Time.realtimeSinceStartup;
    }

    public void OnApplicationQuit()
    {
        _LevelManager = null;
    }

    public void LogHeatMap(float playerPosX, float playerPosY)
    {
        int pixelsPerUnit = 30;

        //x
        int numberOfTilesX = Screen.width / pixelsPerUnit;

        float factorX = numberOfTilesX / playerPosX;

        playerPosX = Screen.width / factorX;

        //y
        int numberOfTilesY = Screen.height / pixelsPerUnit;

        float factorY = numberOfTilesY / playerPosY;

        playerPosY = Screen.height / factorY;

        playerPosY = Screen.height - playerPosY;				//sets y the right way up

    }
}