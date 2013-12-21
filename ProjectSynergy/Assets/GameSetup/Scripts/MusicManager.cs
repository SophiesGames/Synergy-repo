using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Change tracks and prepare music streams
/// //CHECKOUT FOR COUROUTINE WWW class INFO TO STOP CRASHES http://answers.unity3d.com/questions/239591/unity-webplayer-and-www-object.html
/// </summary>
public class MusicManager : MonoBehaviour
{
    //public List<string> urlList = new List<string>();
    //private AudioClip[] clipList;
    //public AudioClip SongOne;
    //public AudioClip SongTwo;
    //public AudioClip SongThree;
    //public AudioClip EndCrescendo;

    [HideInInspector]
    public bool trackNeedsChanged = false;
    [HideInInspector]
    public int newTrackNumber = 0;//Wont play this as its = currentTrackNumber. urlLsit[0] will be blank representing the 1st sonf that comes with the game

	
	int currentScene = 0; // Current scene/level number.
    //private int currentTrackNumber = 0;//not set yet
    //public WWW www;
	
	// @todo: We can probably get rid of this soon.
    private int trackNumber = 1;

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
                Debug.Log("error");
            }
            return _musicManager;
        }
    }

    private void Awake()
    {
        //clipList = new AudioClip[urlList.Count];
        if (_musicManager == null)
        {
            _musicManager = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
		// Play music!
		// Make sure we play the music for the current level.
		currentScene = Application.loadedLevel;
		Fabric.EventManager.Instance.PostEvent("MainMusic");
		Fabric.EventManager.Instance.SetParameter("MainMusic", "Scene", currentScene);
		
		// Use appropriate mixer setting.
		Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.RemovePreset, "Results", null);
		Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.AddPreset, "Gameplay", null);
		
		
		// AD: Testing Fabric timeline parameters.
		// @todo: Send proper values here.
        // @todo: Do we need to call this right away?
        //Fabric.EventManager.Instance.SetParameter("MainMusic", "Corruption", 0.5f);

        // Save the number of total objects on this level.
        //totalObjects = LevelManager.levelManager.interactableObjects.Count;

        // AD: Check level of corruption every 1 second.
        // AD: Maybe it's better to check on Update();
        //InvokeRepeating("CheckCorruption", 0, 1);
    }
	
    // Change the music according to current level of destruction.
    void CheckCorruption() {

        totalObjects = LevelManager.levelManager.interactableObjects.Count;
        corruptedObjects = LevelManager.levelManager.corruptedObjects.Count;
        corruptionLevel = Mathf.Round((float)corruptedObjects / (float)totalObjects * 100.0f);

        //Debug.Log(corruptionLevel + "%");
        Fabric.EventManager.Instance.SetParameter("MainMusic", "Corruption", corruptionLevel);
    }
	
    /**
	 * When a new level is loaded, switch music.
	 */
    void OnLevelWasLoaded(int level)
	{
        Fabric.EventManager.Instance.SetParameter("MainMusic", "Scene", level);
		
		// Switch back to the "Gameplay" mixer preset.
		Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.RemovePreset, "Results", null);
		Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.AddPreset, "Gameplay", null);
		
		switch (level)
		{
		case 0:
	        break;
			
		case 1:
			//Fabric.EventManager.Instance.PostEvent("Melody1");
			break;
		
	    case 2:
			//Fabric.EventManager.Instance.PostEvent("Melody1", Fabric.EventAction.AdvanceSequence);
			break;
			
	    case 3:
			break;
		}
    }
	

    private void Update()
    {
        // Here we will calculate the level of corruption, and play appropriate music.
        CheckCorruption();
    }

    public void SetEndMusic()
    {
        //audio.clip = EndCrescendo;
        //audio.Play();
    }

    //private void ChangeTrack()
    //{
    //    if (clipList[newTrackNumber] == null) //if the game has been started half way and nothing has told the new track to load
    //    {
    //        StartTrackDownload(newTrackNumber); //start loading that new track and play it as soon as possible.
    //    }
    //    //if the track nees to be changed, the current loop is at the end and the next song is done streaming
    //    if (currentTrackNumber != newTrackNumber && clipList[newTrackNumber].isReadyToPlay)
    //    {
    //        audio.clip = clipList[newTrackNumber];
    //        audio.Play();
    //        currentTrackNumber = newTrackNumber;
    //        StartNextTrackDownload();
    //    }
    //}

    //private void StartNextTrackDownload()
    //{
    //    int nextTrackNumber = currentTrackNumber + 1;
    //    StartTrackDownload(nextTrackNumber);
    //}

    //public void StartTrackDownload(int i)
    //{
    //    if (i < urlList.Count)
    //    {
    //        www = new WWW(urlList[i]);  //CHECKOUT FOR COUROUTINE WWW class INFO TO STOP CRASHES http://answers.unity3d.com/questions/239591/unity-webplayer-and-www-object.html
    //        clipList[i] = www.GetAudioClip(false, false);
    //    }
    //}
}
