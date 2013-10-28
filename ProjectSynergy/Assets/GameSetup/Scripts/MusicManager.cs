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
    public AudioClip EndCrescendo;

    [HideInInspector]
    public bool trackNeedsChanged = false;
    [HideInInspector]
    public int newTrackNumber = 0;//Wont play this as its = currentTrackNumber. urlLsit[0] will be blank representing the 1st sonf that comes with the game

	
	int currentScene = 0; // Current scene/level number.
    //private int currentTrackNumber = 0;//not set yet
    //public WWW www;
    private int trackNumber = 1;

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
		currentScene = Application.loadedLevel;
		Fabric.EventManager.Instance.PostEvent("MainMusic");
		Fabric.EventManager.Instance.SetParameter("MainMusic", "Scene", currentScene);
		
		// AD: Testing Fabric timeline parameters.
		// @todo: Send proper values here.
		//Fabric.EventManager.Instance.SetParameter("MainMusic", "Destruction", 0.5f);
    }
	
	
    /**
	 * When a new level is loaded, switch music.
	 */
    void OnLevelWasLoaded(int level)
	{
        Fabric.EventManager.Instance.SetParameter("MainMusic", "Scene", level);
		
		switch (level)
		{
		case 0:
	        break;
			
		case 1:
			Fabric.EventManager.Instance.PostEvent("Melody1");
			break;
		
	    case 2:
			Fabric.EventManager.Instance.PostEvent("Melody1", Fabric.EventAction.AdvanceSequence);
			break;
			
	    case 3:
			break;
		}
    }
	

    private void Update()
    {
    }

    public void SetEndMusic()
    {
        audio.clip = EndCrescendo;
        audio.Play();
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
