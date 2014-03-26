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

    private float levelTimer;
	int currentScene = 0; // Current scene/level number.
    int savedPlayerLevel = 0; // Saved player level.
	
    private float corruptionLevel; // Current level of curruption per level.
    private int corruptedObjects;  // Number of currently corrupted objects.
    private int totalObjects;      // Total object on this level (so we can calculate level of corruption).
	private bool playedFlourish = false;

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

		levelTimer = 0;

		// @temp: Never fail, hahaha! 
		//if (PlayerPrefs.GetInt ("lives") == 1) {
		//	PlayerPrefs.SetInt("lives", 4);
		//}
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
			//Debug.Log("Case 0");
//            if (savedPlayerLevel > 0) {
//                // Do nothing. This screen should be silent.
//            }
//            else {
//                // Play intro music.
//                Fabric.EventManager.Instance.PostEvent ("IntroMusic");
//            }
//			Fabric.EventManager.Instance.PostEvent("Stop/MainMusic");
//			Fabric.EventManager.Instance.PostEvent("Stop/Results");
            break;
            
        case 1:
			//Debug.Log("Case 1");
            // On the first level, play music.
            Fabric.EventManager.Instance.PostEvent("Stop/IntroMusic");
            Fabric.EventManager.Instance.PostEvent("MainMusic");
            break;
            
        default:
            break;
        }

        // Play music for the current level.
		// Do not restart music (or change mixer preset) between the first 2 levels, so it sounds smoother.
        //Debug.Log ("Current scene: " + currentScene);
		if (currentScene > 2) {

			//Fabric.EventManager.Instance.PostEvent("MusicAdvance", Fabric.EventAction.ResetSequence);
			Fabric.EventManager.Instance.PostEvent("MainMusic"); 
			//Fabric.EventManager.Instance.PostEvent("MainMusic",  Fabric.EventAction.UnpauseSound);
			//Fabric.EventManager.Instance.PostEvent("MusicAdvance",  Fabric.EventAction.AdvanceSequence);

			//if (currentScene%2 == 0) {
			//	Debug.Log("Even level!");
			//	Fabric.EventManager.Instance.PostEvent("MusicAdvance",  Fabric.EventAction.AdvanceSequence);
			//}

			// Switch back to the "Gameplay" mixer preset.
			// @todo: Try to use SwitchPreset here instead.
			//Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.RemovePreset, "Results", null);
			//Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.AddPreset, "Gameplay", null);
			//Fabric.GetDynamicMixer.Instance().SwitchPreset("Results", "Gameplay");
			Fabric.EventManager.Instance.PostEvent("Stop/Results");
		}
        Fabric.EventManager.Instance.SetParameter("MainMusic", "Scene", currentScene);
    }
	

    private void Update()
    {
        // Here we will calculate the level of corruption, and play appropriate music.
        CheckCorruption();

        // Intro Music is a separate event in Fabric, and we will call it every 30 seconds.
        if (currentScene == 0) {
			levelTimer += Time.deltaTime;
			if (levelTimer > 30) {
				levelTimer = 0;
                Fabric.EventManager.Instance.PostEvent ("IntroMusic");
            }
        }

		// Play sound flourish on the word Synergy.
		if (currentScene == 2) {
			//levelTimer += Time.deltaTime;
			if (LevelManager.levelManager.levelPlayTime > 9.7 && playedFlourish == false) {
				Fabric.EventManager.Instance.PostEvent ("Flourish");
				playedFlourish = true;
			}
		}
    }

    public void SetEndMusic()
    {
        //
    }
}
