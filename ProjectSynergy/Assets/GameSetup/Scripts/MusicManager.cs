using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Change tracks and prepare music streams
/// //CHECKOUT FOR COUROUTINE WWW class INFO TO STOP CRASHES http://answers.unity3d.com/questions/239591/unity-webplayer-and-www-object.html
/// </summary>
public class MusicManager : MonoBehaviour
{

    [HideInInspector]
    public bool trackNeedsChanged = false;
    [HideInInspector]
    public int newTrackNumber = 0; //Wont play this as its = currentTrackNumber. urlLsit[0] will be blank representing the 1st sonf that comes with the game

    private float introMusicTimer;
	int currentScene = 0; // Current scene/level number.
    int savedPlayerLevel = 0; // Saved player level.
	
    private float corruptionLevel; // Current level of curruption per level.
    private int corruptedObjects;  // Number of currently corrupted objects.
    private int totalObjects;      // Total object on this level (so we can calculate level of corruption).

    private static MusicManager _musicManager;
    public static MusicManager musicManager
    {
        get
        {
            if (_musicManager == null)
            {
                Debug.Log("Music manager prefab not assigned.");
            }
            return _musicManager;
        }
    }

    private void Awake()
    {
        if (_musicManager == null)
        {
            _musicManager = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        HandleMusic();
    }
	
    /**
     * Changes the music according to current level of corruption.
     */
    void CheckCorruption() {

        // In the first 2 levels using corruption music is confusing, since the player is just figuring out the game.
        // So don't use the corruption music in these cases:
        //     1. In level 1 (3 rocks going up)
        //     2. In level 2, while the player is still suspended and is reading the text.
        //        In EventManager.LevelThreeScriptedEvents() looks like it takes 9 seconds before the player is released.
        if (Application.loadedLevel == 1 || 
            (Application.loadedLevel == 2 && LevelManager.levelManager.levelPlayTime < 9)) {
            corruptionLevel = 0;
        }

        // Otherwise, calculate the current percentage of corruption.
        // (relationship of corrupted objects to the number of total objects.
        else {
            totalObjects = LevelManager.levelManager.interactableObjects.Count;
            corruptedObjects = LevelManager.levelManager.corruptedObjects.Count;
            corruptionLevel = Mathf.Round((float)corruptedObjects / (float)totalObjects * 100.0f);
        }

        // Send the corruption parameter to Fabric.
        //Debug.Log(corruptionLevel + "%");
        Fabric.EventManager.Instance.SetParameter("MainMusic", "Corruption", corruptionLevel);
    }
	
    /**
	 * When a new level is loaded, switch music.
	 */
    void OnLevelWasLoaded(int level)
	{
        HandleMusic();
    }

    /**
     * Meat and potatoes of the MusicManager.
     */
    private void HandleMusic() {

        // Which level are we on?
        currentScene = Application.loadedLevel;

        // Does the player have any saved level?
        savedPlayerLevel = PlayerPrefs.GetInt ("levelNumber");

        switch (currentScene)
        {
        case 0:
            Fabric.EventManager.Instance.PostEvent("Stop/MainMusic");
            if (savedPlayerLevel > 0) {
                // Do nothing. This screen should be silent.
            }
            else {
                // Play intro music.
                Fabric.EventManager.Instance.PostEvent ("IntroMusic");
            }
            
            break;
            
        case 1:
            // On the first level, play music.
            Fabric.EventManager.Instance.PostEvent("Stop/IntroMusic");
            Fabric.EventManager.Instance.PostEvent("MainMusic");
            break;
            
        default:
            break;
        }

        // Play music for the current level.
        Fabric.EventManager.Instance.PostEvent("MainMusic"); // @todo: This is temporary.
        Fabric.EventManager.Instance.SetParameter("MainMusic", "Scene", currentScene);
        
        // Switch back to the "Gameplay" mixer preset.
        // @todo: Try to use SwitchPreset here instead.
        //Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.RemovePreset, "Results", null);
        //Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.AddPreset, "Gameplay", null);
        Fabric.EventManager.Instance.PostEvent("Stop/Results");
        Fabric.GetDynamicMixer.Instance().SwitchPreset("Results", "Gameplay");
    }
	

    private void Update()
    {
        // Here we will calculate the level of corruption, and play appropriate music.
        CheckCorruption();

        // Intro Music is a separate event in Fabric, and we will call it every 30 seconds.
        if (currentScene == 0) {
            introMusicTimer += Time.deltaTime;
            if (introMusicTimer > 30) {
                introMusicTimer = 0;
                Fabric.EventManager.Instance.PostEvent ("IntroMusic");
            }
        }
    }

    public void SetEndMusic()
    {
        //
    }
}
