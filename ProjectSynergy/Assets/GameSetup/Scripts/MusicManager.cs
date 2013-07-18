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
    public AudioClip SongOne;
    public AudioClip SongTwo;
    public AudioClip SongThree;
    public AudioClip EndCrescendo;

    [HideInInspector]
    public bool trackNeedsChanged = false;
    [HideInInspector]
    public int newTrackNumber = 0;//Wont play this as its = currentTrackNumber. urlLsit[0] will be blank representing the 1st sonf that comes with the game
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
        //audio.loop = true;
		
		// AD: Commenting out current audio.
        //audio.Play();   //plays the default music to start with.
		
		// AD: Using Fabric instead.
		// @todo: Rename event to something self-explanatory.
		Fabric.EventManager.Instance.PostEvent("Simple");
		
		// AD: Testing Fabric timeline parameters.
		// @todo: Send proper values here.
		Fabric.EventManager.Instance.SetParameter("Simple", "Destruction", 0.5f);
    }

    private void Update()
    {
        if (audio.isPlaying == false)
        {
            trackNumber++;
            if (trackNumber > 3)
            {
                trackNumber = 1;
            }

            if (trackNumber == 1)
            {
                audio.clip = SongOne;
            }
            else if (trackNumber == 2)
            {
                audio.clip = SongTwo;
            }
            else
            {
                audio.clip = SongThree;
            }
			
			// AD: Commenting out current audio.
            //audio.Play();
			
			// AD: Using Fabric instead.
			// @todo: Eventually, we will change out tracks here.
			//Fabric.EventManager.Instance.PostEvent("TestEvent1");
        }

        //ChangeTrack();                //the old system with streaming uses event manager and each level to set a newTrackNumber
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
