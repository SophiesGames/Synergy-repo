using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
using System.IO;

#if UNITY_EDITOR                        //This code is ingored outside of Unity but still run in Unity play mode
using UnityEditor;
#endif

/*To do
 * Add a preview button to each frameset
 * Linked to the above point - allow a texture to be added to the sprite script insepctor bseide where you add xml file.
 * Clean up the class and create sub classes
 * Add a boolean button to each frameSet where you can mark as true to say that when game runs it will start at this frameSet. Gets around problem of changing in the eidtor and applying changes to all GO sharing material.
 */

[ExecuteInEditMode]
public class AnimateSprite : MonoBehaviour
{
    //public bool refreshXML = false;
    public List<AnimationFrameSet> frameSetList = new List<AnimationFrameSet>();

    private string playingFrameSetName;
    private AnimationTimer animationTimer;
    private Material materialInstance;
    private AnimationFrameSet animationFrameSet;
    private AnimationFrameSet tempFrameSetIsPlaying;
    private bool returnSignal = false;
    private bool isLastFrame = false;
    private List<Rect> frames;

    [HideInInspector]
    public Vector2 atlasDimension = new Vector2();

    

#if !UNITY_EDITOR
    private void Awake()                                             //For outside of unity
    {
        materialInstance = this.renderer.material;
        ParseFrames();
        animationTimer = new AnimationTimer();
    }
    private void Update()
    {
        AutoCall();
    }
#endif
#if UNITY_EDITOR                                                    //This code is ingored outside of Unity but still run in Unity play mode
    public void Awake()                                             //For edit mode and play mode
    {
        if (EditorApplication.isPlaying)                            //seperate instances for individual manipulation
        {
            materialInstance = this.GetComponent<Renderer>().material;
        }
        else
        {
            materialInstance = this.GetComponent<Renderer>().sharedMaterial;              //keep them all the same for easy of use. does need to be stated.
        }
        ParseFrames();
        animationTimer = new AnimationTimer();
    }
    private void Update()
    {
        if (EditorApplication.isPlaying)
        {
            AutoCall();
        }
        else if (!EditorApplication.isPlaying)
        {
            ///This check is to see if the initilaisation info has been lost. This happens if the editor has called awake and initialised everything, then a change is made to the code,
            ///or presumably anything else that tells Unity to update, and focus is reotred to the Unity window. The update chucks the initiliasatin info but does not recall awake func.
            if (animationTimer == null)
            {
                Awake();
            }

            if (UpdateFrameSetInfo()) //this sets the new first frame
            {
                Awake();                                         //refreshes xml data
                SetFrameSet(animationFrameSet.frameSetName);
                animationFrameSet.applyFrameSet = false;        //set it back to false immediately
            }
        }
    }
    private bool UpdateFrameSetInfo()
    {
        for (int i = 0; i < frameSetList.Count; i++)
        {
            FrameSetGroupToggle(i);                                     //stops multiple groups been clicked in the editor mode at once. You can only ahve one as the active frameset

            if (frameSetList[i].applyFrameSet == true)                          //search for the desired frameset by checking name
            {
                animationFrameSet = frameSetList[i];                            //fill animation frameset with desired frameset  
                return true;
            }
        }
        return false;                                                        //No framsets need updating
    }
    private void FrameSetGroupToggle(int i)
    {
        ///Toggle to make sure only one of the framsets can be switched to isPlaying
        if (frameSetList[i].isPlaying)
        {
            if (tempFrameSetIsPlaying != frameSetList[i])
            {
                if (tempFrameSetIsPlaying != null)
                { tempFrameSetIsPlaying.isPlaying = false; }                //turn off old is playing
                tempFrameSetIsPlaying = frameSetList[i];                    //mark new one so it can be turned of whene needed
                //tempFrameSetIsPlaying.isPlaying = true                    //already set to true so not needing another line to do it.
            }
        }
    }
#endif

    private void Start()
    {
        InitialAutoPlay();
    }

    private void AutoCall()
    {
        if (playingFrameSetName != null)
        {
            //potential error. it is passing in animationFrameSet.reverseAnimation where animationFrameSet could be anything.
            PlayAnimation(playingFrameSetName, 1, returnSignal);                        //call it again
        }
		else if (animationTimer.animationTimeElapsed == 0)//if nothing else playing and timer been reset, cehck if anything needs to autoplay again.
		{
			InitialAutoPlay();//nothing playing so return to autoplay
		}
    }

    private void ParseFrames()
    {
        frames = null;
        string XMLSheet = this.GetComponent<Renderer>().sharedMaterial.mainTexture.name;

        for (int i = 0; i < AnimationXMLData.animationXMLData.loadedXMLS.Count; i++)
        {
            if (AnimationXMLData.animationXMLData.loadedXMLS[i] == XMLSheet)//if the name is the same
            {
                frames = AnimationXMLData.animationXMLData.ListOfLists[i];//the position of the name is the same as the postion of the coreesponding list so make framelist = to this.
                atlasDimension = AnimationXMLData.animationXMLData.sheetDimensions[i];
                break;
            }
        }
        if (frames == null) //still not been assigned so there must not be an existing list.
        {
            AddXMLFrameList(XMLSheet);
        }
    }

    private void AddXMLFrameList(string XMLSheet)
    {
        AnimationXML animationXML = new AnimationXML();
        SmallXmlParser smallXML = new SmallXmlParser();
        TextAsset temp = (TextAsset)Resources.Load(XMLSheet);
        TextReader textReader = new StringReader(temp.text);
        smallXML.Parse(textReader, animationXML);

        AnimationXMLData.animationXMLData.loadedXMLS.Add(XMLSheet);     //add the name of the list.
        AnimationXMLData.animationXMLData.ListOfLists.Add(animationXML.frameDetails);//add the new list
        AnimationXMLData.animationXMLData.sheetDimensions.Add(animationXML.atlasDimension);//add the new list

        ParseFrames(); //try again now the new lsit ahs been created
    }

    private void InitialAutoPlay()
    {
        for (int i = 0; i < frameSetList.Count; i++)
        {
            if (frameSetList[i].isPlaying == true && frameSetList[i].isLooping)                                                   //search for the desired frameset by checking name
            {
                PlayAnimation(frameSetList[i].frameSetName, 1);
                break;
            }
        }
    }

    private void IdentifyFrameSet(string frameSetName)
    {
        for (int i = 0; i < frameSetList.Count; i++)
        {
            if (frameSetList[i].frameSetName == frameSetName)                                                   //search for the desired frameset by checking name
            {
                animationFrameSet = frameSetList[i];                                                            //fill animation frameset with desired frameset
                break;
            }
        }
        if (animationFrameSet == null)
        {
            Debug.LogError("animationFrameSet = null: No animationFrameSet set by the name '" + frameSetName + "' found on" + this.gameObject.name);
        }
    }

    private int FindIndex()
    {
        float index = (animationFrameSet.framesPerSecond * animationTimer.animationTimeElapsed);                                 //the currentFrame number will be the number of frames each second * how many seconds have gone by

        if (index > animationFrameSet.framesInSet)                                                              //if the index is past the total frames
        {
            index = LastFrameBehaviour((int)index);
        }
        else
        {
            index = index + animationFrameSet.startingFrame;                                                    //aniamtion as normal. shift index by the number it should be starting at    
        }
        return (int)index;
    }

    private int FindIndexInReverse()
    {
        float index = animationFrameSet.endingFrame - (animationFrameSet.framesPerSecond * animationTimer.animationTimeElapsed);                                  //the currentFrame number will be the number of frames each second * how many seconds have gone by

        if (index < animationFrameSet.startingFrame - 1)                                                                                                  //if the index is past the total frames
        {
            index = LastFrameBehaviourReverse((int)index);
        }
        index = (int)Math.Ceiling(index);
        return (int)index;
    }

    private int LastFrameBehaviour(int index)
    {
		animationTimer.ResetTimer(animationFrameSet);
		
        if (animationFrameSet.isLooping == true)                         //checks for looped animations and sets them back to the start
        {
            index = animationFrameSet.startingFrame;
        }
        else
        {
            if (animationFrameSet.endOnFrameOne == true)              //Otherwqise force them to stay at the last frame
            {
                index = animationFrameSet.startingFrame;
            }
            else
            {
                index = animationFrameSet.endingFrame;
            }
            playingFrameSetName = null;
            if (returnSignal == true)
            {
                isLastFrame = true;
            }
        }

        if (animationFrameSet.bounceAnimation == true)
        {
            animationFrameSet.reverseAnimation = true;
        }
        return index;
    }

    private int LastFrameBehaviourReverse(int index)
    {
		animationTimer.ResetTimer(animationFrameSet);
		
        if (animationFrameSet.isLooping == true)                         //checks for looped animations and sets them back to the start
        {
            index = animationFrameSet.endingFrame;
        }
        else
        {
            if (animationFrameSet.endOnFrameOne == true)              //Otherwqise force them to stay at the last frame
            {
                index = animationFrameSet.endingFrame;
            }
            else
            {
                index = animationFrameSet.startingFrame;
            }
            playingFrameSetName = null;
            if (returnSignal == true)
            {
                this.gameObject.SendMessage("AnimationFinished", animationFrameSet.frameSetName);
            }
        }
        if (animationFrameSet.bounceAnimation == true)
        {
            animationFrameSet.reverseAnimation = false;
        }
        return index;
    }

    private void UpdateViewRect(int index)
    {
        index = index - 1;                                                                          //return to 0 base 

        Vector2 size = new Vector2(frames[index].width / atlasDimension.x, frames[index].height / atlasDimension.y);
        Vector2 offset = new Vector2(frames[index].x / atlasDimension.x, ((1 - size.y) - (frames[index].y / atlasDimension.y)));//(1 - size.y) - < that step taken as 0y is at teh bottom not the top.

        //Apply calculations and changes for currentFrame
        materialInstance.mainTextureScale = size;                                                       //Scale it up   (sharedMaterial means all objects using that amterial will be affected over normal 'material property'
        materialInstance.mainTextureOffset = offset;

        if (returnSignal == true && isLastFrame == true)                                                //this goes here to amke sure anima finsihed incase a new aniamtion is called
        {
            this.gameObject.SendMessage("AnimationFinished", animationFrameSet.frameSetName);
            isLastFrame = false;
        }
    }

    //Accessible methods

    public void SetFrameSet(string frameSetName)
    {
        if (animationFrameSet == null || animationFrameSet.frameSetName != frameSetName)   //take out the or part?//check animationFrameSet needs to be assigned a new frameset
        {
            IdentifyFrameSet(frameSetName);
        }
        int index = animationFrameSet.startingFrame;
        SetFrameSet(index);
    }

    public void SetFrameSet(int index)
    {
        UpdateViewRect(index);
    }

    public void PlayAnimation(string frameSetName)
    {
        int index;

        //Debug.Log("animationFrameSet " + animationFrameSet);
        if (animationFrameSet == null || animationFrameSet.frameSetName != frameSetName)            //check animationFrameSet needs to be assigned a new frameset
        {
            IdentifyFrameSet(frameSetName);
        }

        if (playingFrameSetName != animationFrameSet.frameSetName)                                  //if its a new aniamtion being played by the GO, then stop the autoPlay so it wont come on after without being recalled.
        {
            playingFrameSetName = null;
        }

        if (animationTimer == null)                                  //if its a new aniamtion being played by the GO, then stop the autoPlay so it wont come on after without being recalled.
        {
            Debug.LogError("animationTimer is null for " + this.name + "when calling" + animationFrameSet.frameSetName + ". Make sure you are not calling PlayAnimation() before it has time to intialise");
        }
        //Debug.Log("animationFrameSet.frameSetName " + animationFrameSet.frameSetName);
        //Debug.Log("animationTimer " + animationTimer);

        animationTimer.UpdateElapsedTime(animationFrameSet);

        if (animationFrameSet.reverseAnimation)
        {
            index = FindIndexInReverse();                                                           //reverse code
        }
        else { index = FindIndex(); }

        UpdateViewRect(index);
    }

    public void PlayAnimation(string frameSetName, int playCycles, bool _returnSignal = false)
    {
        returnSignal = _returnSignal;
        playingFrameSetName = frameSetName;
        PlayAnimation(frameSetName);                                            //plays aniamtion normally and gets the index to put into autoupdate func.
    }

    public string FrameSetPlaying()
    {
        return playingFrameSetName;
    }
}


