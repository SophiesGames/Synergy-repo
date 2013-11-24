using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// At update, checks level the checks for and runs any code events that should be triggered
/// </summary>
public class EventManager : MonoBehaviour
{
    int eventIndex = 1;
	bool startedMusicFlag = false;

    private static EventManager _EventManager;
    public static EventManager eventManager
    {
        get
        {
            if (_EventManager == null)
            {
                _EventManager = new GameObject("EventManager").AddComponent<EventManager>();
            }
            return _EventManager;
        }
    }

    private void Awake()
    {
        switch (Application.loadedLevel)
        {
            case 0:
                {
                    if (PlayerPrefs.GetInt("levelNumber") == 0)
                    {
                        PlayerPrefs.SetInt("lives", 4);			//set lives to 4
                    }
                    break;
                }

            //case 21:
            //    {

            //        break;
            //    }
        }
    }

    void Update()
    {
        LevelEvents();
    }

    private void LevelEvents()
    {
		// @todo: We can probably get rid of this soon.
        switch (Application.loadedLevel)
        {
            case 0:
                {
					if(startedMusicFlag == false
						&& GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezePlayer == false
						&& (Input.GetButtonDown("Jump") || Input.GetButtonDown("Horizontal"))
					)
					{
						Fabric.EventManager.Instance.SetParameter("MainMusic", "Scene", 1);
						startedMusicFlag = true;
			    	}
			
                    if (PlayerPrefs.GetInt("levelNumber") > 0)
                    {
                        NewOrContinue();
                    }
                    else
                    {
                        LevelOneScriptedEvents();
                    }
                    break;
                }

            case 1:
                {
                    LevelTwoScriptedEvents();
                    break;
                }

            case 2:
                {
                    //MusicManager.musicManager.StartTrackDownload(1);//get the new song ready for level 3
                    LevelThreeScriptedEvents();
                    break;
                }

            case 3:
                {
                    MusicManager.musicManager.newTrackNumber = 1;//will start playing this song and load next song.
                    break;
                }
            case 4:
                {
                    MusicManager.musicManager.newTrackNumber = 1;//if the player starts fro thei level amke sure its on correct song.
                    break;
                }

            case 5:
                {
                    MusicManager.musicManager.newTrackNumber = 2;
                    break;
                }

            case 6:
                {
                    MusicManager.musicManager.newTrackNumber = 2;
                    break;
                }

            case 7:
                {
                    MusicManager.musicManager.newTrackNumber = 3;
                    break;
                }

            case 8:
                {
                    MusicManager.musicManager.newTrackNumber = 3;
                    break;
                }

            case 9:
                {
                    MusicManager.musicManager.newTrackNumber = 4;
                    break;
                }

            case 10:
                {
                    MusicManager.musicManager.newTrackNumber = 4;
                    break;
                }

            case 11:
                {
                    MusicManager.musicManager.newTrackNumber = 5;
                    break;
                }

            case 12:
                {
                    MusicManager.musicManager.newTrackNumber = 5;
                    break;
                }

            case 13:
                {
                    MusicManager.musicManager.newTrackNumber = 6;
                    break;
                }
            case 14:
                {
                    MusicManager.musicManager.newTrackNumber = 6;
                    break;
                }

            case 15:
                {
                    MusicManager.musicManager.newTrackNumber = 7;
                    break;
                }

            case 16:
                {
                    MusicManager.musicManager.newTrackNumber = 7;
                    break;
                }

            case 17:
                {
                    MusicManager.musicManager.newTrackNumber = 8;
                    break;
                }

            case 18:
                {
                    MusicManager.musicManager.newTrackNumber = 8;
                    break;
                }

            case 19:
                {
                    MusicManager.musicManager.newTrackNumber = 9;
                    break;
                }

            case 20:
                {
                    MusicManager.musicManager.newTrackNumber = 10;
                    break;
                }
        }
    }

    private void NewOrContinue()
    {
        GameObject.Find("PressNReturnGame").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
        GameObject.Find("PressSpace").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
		GameObject winFadeGO = GameObject.Find("WinFade");
		if (winFadeGO!= null) GameObject.Find("WinFade").gameObject.SetActive(false);
        if (Input.GetButtonUp("Jump"))
        {
            GameManagerC.gameManager.LoadLevelNumber(PlayerPrefs.GetInt("levelNumber"));
        }
        //n key for next level code in game manager
    }

    private void LevelOneScriptedEvents()
    {
        switch (eventIndex)
        {
            case 1:
                {
			
					//todo: starting music trigger goes here so it doesnt play on the continue screen
                    GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezePlayer = true;
                    GameObject.Find("Advancement").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                    eventIndex++;
                    break;
                }

            case 2:
                {
                    if (LevelManager.levelManager.levelPlayTime > 1)
                    {
                        GameObject.Find("Progression").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex++;
                    }
                    break;
                }

            case 3:
                {
                    if (LevelManager.levelManager.levelPlayTime > 3)
                    {
                        GameObject.Find("ToAchieve").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);

                        //play sound made on victory
                        //play sparkle aniamtion on win
                        eventIndex++;
                    }
                    break;
                }

            case 4:
                {
                    if (LevelManager.levelManager.levelPlayTime > 5)
                    {
                        GameObject.Find("TheEssence").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("PlayerFade").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        //play sound for player
                        //play human animation
                        eventIndex++;
                    }
                    break;
                }

            case 5:
                {
                    if (LevelManager.levelManager.levelPlayTime > 7)
                    {
                        GameObject.Find("YearnsFor").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("WinLogic").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        //play sound for player
                        //play human animation
                        eventIndex++;
                    }
                    break;
                }

            case 6:
                {
                    if (LevelManager.levelManager.levelPlayTime > 9)
                    {

                        GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezePlayer = false;
                        GameObject.Find("MovementInstruc").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("MovementInstruc2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("RockBaseFade").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);

                        //                        GameObject[] rockTiles = GameObject.FindGameObjectsWithTag("Rock");
                        //                        foreach (GameObject rockObj in rockTiles)
                        //                        {
                        //                            rockObj.gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        //                        }
                        eventIndex = 0; //reset it
                    }
                    break;
                }

            default:
                {
                    break;
                }
        }

    }

    private void LevelTwoScriptedEvents()
    {
        switch (eventIndex)
        {
            case 1:
                {
                    GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezePlayer = true;
                    GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezeJump = true;
                    GameObject.Find("TheEnviron").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);

                    GameObject.Find("DirtBaseSingleFade1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                    GameObject.Find("DirtBaseSingleFade2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                    GameObject.Find("DirtBaseSingleFade3").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                    GameObject.Find("DirtBaseSingleFade4").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);

                    foreach (GameObject obj in LevelManager.levelManager.interactableObjects)
                    {
                        obj.gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                    }
                    eventIndex++;
                    break;
                }

            case 2:
                {
                    if (LevelManager.levelManager.levelPlayTime > 2)
                    {
                        GameObject.Find("PlayerFade").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezePlayer = false;
                        GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezeJump = false;
                        GameObject.Find("HoldingUp").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("JumpInstruc").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex++;
                    }
                    break;
                }

            case 3:
                {
                    if (LevelManager.levelManager.corruptedObjects.Count >= 1)
                    {
                        GameObject.Find("SometimesTo").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex++;
                    }
                    break;
                }
            case 4:
                {
                    if (LevelManager.levelManager.levelPlayTime > 4)
                    {
                        GameObject.Find("RunAnd").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex = 0;
                    }
                    break;
                }

            default:
                {
                    break;
                }
        }

    }

    private void LevelThreeScriptedEvents()
    {
        switch (eventIndex)
        {
            case 1:
                {
                    GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezePlayer = true;
                    GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezeJump = true;
                    GameObject.Find("EarthHas").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
					
                    eventIndex++;
                    break;
                }
            case 2:
                {
                    if (LevelManager.levelManager.levelPlayTime > 2)//change to after has healed
                    {
                        GameObject.Find("HorizontalHealLogic").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("Stem").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex++;
                    }
                    break;
                }

            case 3:
                {
                    if (LevelManager.levelManager.levelPlayTime > 3)//change to after has healed
                    {
                        GameObject.Find("HorizontalHealLogic").gameObject.GetComponent<HorizontalHeal>().HealHorizontally();
                        eventIndex++;
                    }
                    break;
                }

            case 4:
                {
                    if (LevelManager.levelManager.levelPlayTime > 4)//change to after ones before
                    {
                        GameObject.Find("WithPerception").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex++;
                    }
                    break;
                }

            case 5:
                {
                    if (LevelManager.levelManager.levelPlayTime > 6)//change to after ones before
                    {
                        GameObject.Find("CanWe").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex++;
                    }
                    break;
                }

            case 6:
                {
                    if (LevelManager.levelManager.levelPlayTime > 9)//change to after ones before
                    {

                        GameObject.Find("PlayerFade").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezePlayer = false;
                        GameObject.Find("PlayerFade").gameObject.GetComponent<Player>().freezeJump = false;
                        GameObject.Find("IndustryContainer").gameObject.SetActiveRecursively(true);
                        GameObject.Find("EarthHas").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0.0f);
                        GameObject.Find("WithPerception").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0.0f);
                        GameObject.Find("CanWe").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0.0f);
                        GameObject.Find("BackgroundFade").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        eventIndex++;
                    }
                    break;
                }

            case 7:
                {
                    if (LevelManager.levelManager.levelPlayTime > 10)//change to after ones before
                    {
                        GameObject.Find("Synergy").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                        GameObject.Find("Synergy").gameObject.GetComponent<AnimateSprite>().PlayAnimation("Synergy", 1);
                        eventIndex = 0;
                    }
                    break;
                }

            default:
                {
                    break;
                }
        }

    }
}
