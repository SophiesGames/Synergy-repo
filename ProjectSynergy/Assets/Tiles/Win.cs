using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Win : MonoBehaviour
{
	// @todo: Can remove these after transitioning to Fabric.
    public AudioClip winSound;
    public AudioClip failSound;
	
    public GameObject greyBox;
    public float rewardTimeSpan = 2.0f;
	
	private int petalsRemaining;

    private bool canJumpAdvance = false;

    private int numberOfColumns = 11;
    private float staggerTime;

    List<GameObject> Col0 = new List<GameObject>();
    List<GameObject> Col1 = new List<GameObject>();
    List<GameObject> Col2 = new List<GameObject>();
    List<GameObject> Col3 = new List<GameObject>();
    List<GameObject> Col4 = new List<GameObject>();
    List<GameObject> Col5 = new List<GameObject>();
    List<GameObject> Col6 = new List<GameObject>();
    List<GameObject> Col7 = new List<GameObject>();
    List<GameObject> Col8 = new List<GameObject>();
    List<GameObject> Col9 = new List<GameObject>();
    List<GameObject> Col10 = new List<GameObject>();
    List<GameObject> Col11 = new List<GameObject>();

    private bool isAnimationFinished = false;
    private int lastCol = 999;

    private void Update()
    {
        if (Input.GetButton("Jump") && canJumpAdvance && isAnimationFinished == true && petalsRemaining > 0)         //doesnt run every frame as aniamtionFinished only set to true once this win anim starts.
        {
			PlayerPrefs.SetInt("lives", petalsRemaining);				//locks in the lives just before changing level. 
            GameManagerC.gameManager.NextLevel();
            isAnimationFinished = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
		// Play "end of level" SFX.
		// @todo: Come up with a better SFX.
		Fabric.EventManager.Instance.PostEvent("EndLevel");
		
        if (Application.loadedLevel == 0 || Application.loadedLevel == 1)
        {
            GameManagerC.gameManager.NextLevel();
        }
    }

    private void OnTriggerStay(Collider collider)
    {
		// Change the Fabric mix for the results screen, but not in the tutorial levels.
        if (Application.loadedLevel > 1) {
            Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.RemovePreset, "Gameplay", null);
		    Fabric.EventManager.Instance.PostEvent("DynamicMixer", Fabric.EventAction.AddPreset, "Results", null);
        }

        if (LevelManager.levelManager.healingFinished == true                           //healing done
            && collider.gameObject.GetComponent<Player>().freezePlayer == false)        //code has not already played before
        {
            collider.gameObject.GetComponent<Player>().freezePlayer = true; //stop from moving
			
			FillLists();
            StartCoroutine("HighlightTiles");

            // Play Result Screen music, but make sure it doesn't play in tutorial levels.
            if (Application.loadedLevel > 1) {
                Fabric.EventManager.Instance.PostEvent("ResultScreen");
            }
        }
    }

    private void FillLists()
    {
        staggerTime = rewardTimeSpan / numberOfColumns;

        FindFirstCorruptColumn();

        foreach (GameObject tile in LevelManager.levelManager.interactableObjects)
        {
            int column = Mathf.CeilToInt(tile.transform.root.position.x / 2);//round to nearest whole number by typecast as int
            if (column < lastCol)       //on first round it wont be as lastCol is set to 100. 
            {
                switch (column)
                {
                    case 0:
                        {
                            Col0.Add(tile);
                        }
                        break;

                    case 1:
                        {
                            Col1.Add(tile);
                        }
                        break;
                    case 2:
                        {
                            Col2.Add(tile);
                        }
                        break;

                    case 3:
                        {
                            Col3.Add(tile);
                        }
                        break;
                    case 4:
                        {
                            Col4.Add(tile);
                        }
                        break;

                    case 5:
                        {
                            Col5.Add(tile);
                        }
                        break;
                    case 6:
                        {
                            Col6.Add(tile);
                        }
                        break;

                    case 7:
                        {
                            Col7.Add(tile);
                        }
                        break;
                    case 8:
                        {
                            Col8.Add(tile);
                        }
                        break;

                    case 9:
                        {
                            Col9.Add(tile);
                        }
                        break;
                    case 10:
                        {
                            Col10.Add(tile);
                        }
                        break;

                    case 11:
                        {
                            Col11.Add(tile);
                        }
                        break;

                    default:
                        {
                        }
                        break;
                }
            }
        }
    }

    private void FindFirstCorruptColumn()
    {
        if (LevelManager.levelManager.corruptedObjects.Count != 0)
        {
            float previousLastColPos = 22.0f;
            foreach (GameObject tileCorrupt in LevelManager.levelManager.corruptedObjects)
            {
                if (tileCorrupt.transform.root.position.x < previousLastColPos)
                {
                    previousLastColPos = tileCorrupt.transform.root.position.x;
                }
            }
            lastCol = Mathf.CeilToInt(previousLastColPos / 2);//compensate for the fact tiles are 2 long
        }
    }

    private IEnumerator HighlightTiles() //refactor to use timer isntead of subroutine
    {
        if (lastCol < 999)
        {
            numberOfColumns = lastCol;
        }

        for (int i = 0; i < numberOfColumns; i++)
        {
            switch (i)
            {
                case 0:
                    {
                        RewardGlow(Col0);
                    }
                    break;

                case 1:
                    {
                        RewardGlow(Col1);
                    }
                    break;
                case 2:
                    {
                        RewardGlow(Col2);
                    }
                    break;

                case 3:
                    {
                        RewardGlow(Col3);
                    }
                    break;
                case 4:
                    {
                        RewardGlow(Col4);
                    }
                    break;

                case 5:
                    {
                        RewardGlow(Col5);
                    }
                    break;
                case 6:
                    {
                        RewardGlow(Col6);
                    }
                    break;

                case 7:
                    {
                        RewardGlow(Col7);
                    }
                    break;
                case 8:
                    {
                        RewardGlow(Col8);
                    }
                    break;

                case 9:
                    {
                        RewardGlow(Col9);
                    }
                    break;
                case 10:
                    {
                        RewardGlow(Col10);
                    }
                    break;

                case 11:
                    {
                        RewardGlow(Col11);
                    }
                    break;

                default:
                    {
                    }
                    break;
            }
			
			yield return new WaitForSeconds(staggerTime);//all the interactable objects divide by teh time designated to play song.
           
			// you failed wait unti the end, if you won wait until i < 3
			if ((i == 3 && lastCol == 999) )//when its 3 and you have won 
			{
				petalsRemaining = PlayerPrefs.GetInt("lives");
				CallGreyBox();				// This coroutine will carry on playing in the background then skip over next stage as its done.
            }
        }

        if (lastCol < 999)         //there was active corrupt column so Player badness
        {
			// Play corrupted sound.
            Fabric.EventManager.Instance.PostEvent("Corrupt/Grass");
			
			//turn the corrupt ones black
            foreach (GameObject obj in LevelManager.levelManager.corruptedObjects)
            {
                Transform glow = obj.gameObject.transform.parent.FindChild("Glow");
                glow.position = new Vector3(glow.position.x, glow.position.y, -4);
                glow.GetComponent<AnimateSprite>().PlayAnimation("DarkCloud", 1);
            }
			//turn the ones glowing off
			foreach (GameObject obj in LevelManager.levelManager.nonCorruptedObjects)
            {
                Transform glow = obj.gameObject.transform.parent.FindChild("Glow");
                glow.GetComponent<AnimateSprite>().PlayAnimation("ReverseGlow", 1);
            }
			petalsRemaining = PlayerPrefs.GetInt("lives")-1;

			CallGreyBox();
        }

        // @todo: Play different result screen sounds based on petalsRemaining. Use the switch component.
        //string switchParameter = "Lives" + petalsRemaining.ToString();
        //Debug.Log(switchParameter);
        //Fabric.EventManager.Instance.PostEvent("ResultLives", Fabric.EventAction.SetSwitch, switchParameter);
        //Fabric.EventManager.Instance.PostEvent("ResultLives");

        yield return null;
    }
	
	private void CallGreyBox()
	{
		GameObject greyBoxInstance = (GameObject)Instantiate(greyBox);
        petalsRemaining = greyBoxInstance.GetComponent<GreyBox>().InitialiseBox(petalsRemaining);
		isAnimationFinished = true;
        canJumpAdvance = true;
	}

    private void RewardGlow(List<GameObject> currentList)
    {
        foreach (GameObject obj in currentList)
        {
            Transform glow = obj.gameObject.transform.parent.FindChild("Glow");
            glow.position = new Vector3(glow.position.x, glow.position.y, -4);
            glow.GetComponent<AnimateSprite>().PlayAnimation("Glow", 1);
        }
    }
}
