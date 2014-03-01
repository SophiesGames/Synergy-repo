using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// At update, checks level the checks for and runs any code events that should be triggered
/// </summary>
public class EventManager : MonoBehaviour
{
		int eventIndex = 1;

        private static EventManager _EventManager;

		public static EventManager eventManager {
				get {
						if (_EventManager == null) {
								_EventManager = new GameObject ("EventManager").AddComponent<EventManager> ();
						}
						return _EventManager;
				}
		}

		private void Awake ()
		{
				switch (Application.loadedLevel) {
				case 0:
						{
								if (PlayerPrefs.GetInt ("levelNumber") == 0) {
										PlayerPrefs.SetInt ("lives", 4);			//set lives to 4
								}
								break;
						}

				//case 21:
				//    {

				//        break;
				//    }
				}
		}

		void Update ()
		{
				LevelEvents ();
		}

		private void LevelEvents ()
		{
				switch (Application.loadedLevel) {
				case 0:
						{
								if (PlayerPrefs.GetInt ("levelNumber") > 0) {
										NewOrContinue ();
								} else {
										LevelOneScriptedEvents ();
								}
								break;
						}

				case 1:
						{
								LevelTwoScriptedEvents ();
								break;
						}

				case 2:
						{
								//MusicManager.musicManager.StartTrackDownload(1);//get the new song ready for level 3
								LevelThreeScriptedEvents ();
								break;
						}

				default:
						break;
				}
		}

		private void NewOrContinue ()
		{
				GameObject.Find ("PressNReturnGame").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
				GameObject.Find ("PressSpace").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
				GameObject winFadeGO = GameObject.Find ("WinFade");
				if (winFadeGO != null)
						GameObject.Find ("WinFade").gameObject.SetActive (false);
				if (Input.GetButtonUp ("Jump")) {
						GameManagerC.gameManager.LoadLevelNumber (PlayerPrefs.GetInt ("levelNumber"));

						// And also start the music...
						//Debug.Log ("Continue...");
						Fabric.EventManager.Instance.PostEvent ("MainMusic");
						Fabric.EventManager.Instance.SetParameter ("MainMusic", "Scene", PlayerPrefs.GetInt ("levelNumber")); 
				}
				//n key for next level code in game manager
		}

		private void LevelOneScriptedEvents ()
        {
            switch (eventIndex) {
				case 1:
						{
								GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezePlayer = true;

								if (LevelManager.levelManager.levelPlayTime > 1) {


										if (PlayerPrefs.GetInt ("levelNumber") > 0) {
											// Do nothing. This screen should be silent.
										}
										else {
											// Play intro music.
											Fabric.EventManager.Instance.PostEvent ("IntroMusic");
										}
										Fabric.EventManager.Instance.PostEvent("Stop/MainMusic");
										Fabric.EventManager.Instance.PostEvent("Stop/Results");




                                        GameObject.Find ("Advancement").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}

				case 2:
				{
					GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezePlayer = true;
					
					if (LevelManager.levelManager.levelPlayTime > 2) {
						GameObject.Find ("Advancement").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
						eventIndex++;
					}
					break;
				}

				case 3:
						{
								if (LevelManager.levelManager.levelPlayTime > 4.5) {
										GameObject.Find ("Progression").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}

				case 4:
						{
								if (LevelManager.levelManager.levelPlayTime > 7) {
										GameObject.Find ("ToAchieve").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);

										//play sound made on victory
										//play sparkle aniamtion on win
										eventIndex++;
								}
								break;
						}

				case 5:
						{
								if (LevelManager.levelManager.levelPlayTime > 9.5) {
										GameObject.Find ("TheEssence").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}

				case 6:
				{
					if (LevelManager.levelManager.levelPlayTime > 12.5) {
						GameObject.Find ("PlayerFade").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
						GameObject.Find ("PlayerFade").transform.Find("PlayerParticleSystem").GetComponent<ParticleSystem>().Play();
						GameObject.Find ("PlayerFade").transform.Find("JumpParticles").GetComponent<ParticleSystem>().Play();
						//play sound for player
						//play human animation
						eventIndex++;
					}
					break;
				}

				case 7:
						{
								if (LevelManager.levelManager.levelPlayTime > 15) {
										GameObject.Find ("YearnsFor").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
				                        GameObject.Find ("WinFade").transform.Find("GoalParticles").GetComponent<ParticleSystem>().Play();
										GameObject.Find ("WinLogic").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										//play sound for player
										//play human animation
										eventIndex++;
								}
								break;
						}

				case 8:
						{
								if (LevelManager.levelManager.levelPlayTime > 18) {

										GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezePlayer = false;
										GameObject.Find ("MovementInstruc").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("MovementInstruc2").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("RockBaseFade").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);

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

		private void LevelTwoScriptedEvents ()
		{
				switch (eventIndex) {
				case 1:
						{
								GameObject.Find ("PlayerFade").transform.Find("PlayerParticleSystem").GetComponent<ParticleSystem>().Play();
								GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezePlayer = true;
								GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezeJump = true;
								GameObject.Find ("TheEnviron").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);

								GameObject.Find ("DirtBaseSingleFade1").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
								GameObject.Find ("DirtBaseSingleFade2").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
								GameObject.Find ("DirtBaseSingleFade3").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
								GameObject.Find ("DirtBaseSingleFade4").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);

								foreach (GameObject obj in LevelManager.levelManager.interactableObjects) {
										obj.gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
								}
								eventIndex++;
								break;
						}

				case 2:
						{
								if (LevelManager.levelManager.levelPlayTime > 2) {
										GameObject.Find ("PlayerFade").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezePlayer = false;
										GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezeJump = false;
										GameObject.Find ("HoldingUp").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("JumpInstruc").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}

				case 3:
						{
								if (LevelManager.levelManager.corruptedObjects.Count >= 1) {
										GameObject.Find ("SometimesTo").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}
				case 4:
						{
								if (LevelManager.levelManager.levelPlayTime > 4) {
										GameObject.Find ("RunAnd").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
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

		private void LevelThreeScriptedEvents ()
		{
				switch (eventIndex) {
				case 1:
						{
								GameObject.Find ("PlayerFade").transform.Find("PlayerParticleSystem").GetComponent<ParticleSystem>().Play();
								GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezePlayer = true;
								GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezeJump = true;
								GameObject.Find ("EarthHas").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
					
								eventIndex++;
								break;
						}
				case 2:
						{
								if (LevelManager.levelManager.levelPlayTime > 2) {//change to after has healed
										GameObject.Find ("HorizontalHealLogic").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("Stem").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}

				case 3:
						{
								if (LevelManager.levelManager.levelPlayTime > 3) {//change to after has healed
				GameObject.Find ("BackgroundFade").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("HorizontalHealLogic").gameObject.GetComponent<HorizontalHeal> ().HealHorizontally ();
										eventIndex++;
								}
								break;
						}

				case 4:
						{
								if (LevelManager.levelManager.levelPlayTime > 4) {//change to after ones before
										GameObject.Find ("WithPerception").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}

				case 5:
						{
								if (LevelManager.levelManager.levelPlayTime > 6) {//change to after ones before
										GameObject.Find ("CanWe").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										eventIndex++;
								}
								break;
						}

				case 6:
						{
								if (LevelManager.levelManager.levelPlayTime > 9) {//change to after ones before

										GameObject.Find ("PlayerFade").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezePlayer = false;
										GameObject.Find ("PlayerFade").gameObject.GetComponent<Player> ().freezeJump = false;
										GameObject.Find ("IndustryContainer").gameObject.SetActiveRecursively (true);
										GameObject.Find ("EarthHas").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 0.0f);
										GameObject.Find ("WithPerception").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 0.0f);
										GameObject.Find ("CanWe").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 0.0f);
										
										eventIndex++;
								}
								break;
						}

				case 7:
						{
								if (LevelManager.levelManager.levelPlayTime > 10) {//change to after ones before
										GameObject.Find ("Synergy").gameObject.GetComponent<AnimateRGB> ().SetRGBA (4, 1.0f);
										GameObject.Find ("Synergy").gameObject.GetComponent<AnimateSprite> ().PlayAnimation ("Synergy", 1);
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
