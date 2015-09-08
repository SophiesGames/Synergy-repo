using UnityEngine;
using System.Collections;

public class GreyBox : MonoBehaviour
{
    public GameObject PressJump;
    public GameObject PressN;
	public GameObject Failed;
	public GameObject Succeeded;
	public GameObject FailedShadow;
	public GameObject SucceededShadow;

    private GameObject text;
    public float delayBeforeAnimation = 1;

    private int petalToCorrupt = 0;
    private float startTimer = 0;
    private bool winGlow = false;

    private void FastStart()
    {
		//all faded out
		//fade in base don lives
//		this.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0);
//		this.transform.FindChild("Petal2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0);
//		this.transform.FindChild("Petal3").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0);
//		this.transform.FindChild("Petal4").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0);
		
		UpdatePicture();
        if (this.GetComponent<AnimateRGB>() != null) //if its a box with a square artound it ie not the last ending flower
        {
            //this.GetComponent<AnimateRGB>().SetRGBA(4, 0.3f);//0.58f   <put this number back instead of 0, to get green box
            //this.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1);
//            this.transform.FindChild("Petal2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1);
//            this.transform.FindChild("Petal3").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1);
//            this.transform.FindChild("Petal4").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1);
//            this.transform.FindChild("PlantHead").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1);
            //int percentComplete = (Application.loadedLevel * 5);//gets out of 100% by times by 5
            GUIText progressText = this.transform.FindChild("YourProgression").gameObject.GetComponent<GUIText>();
            progressText.text = Application.loadedLevel + "/22";
        }
    }

    private void Update()
    {
        if (startTimer == 0)
        {
            startTimer = Time.fixedTime;
        }
        float currentTime = Time.fixedTime;

        if (currentTime > startTimer + delayBeforeAnimation) //if current time is greater than startTimer +1 second.
        {
            if (petalToCorrupt > 0)
            {
                if (petalToCorrupt == 4)  //if its the last one
                {
                    //this.transform.FindChild("PlantHead").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 0.0f, 0.1f);
                    this.transform.FindChild("TooCorrupt").gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1.0f);
                }

                string petal = "Petal" + petalToCorrupt;
				string nextPetal = "Petal" + (petalToCorrupt + 1);
                this.transform.FindChild("FlowerGlow").GetComponent<AnimateSprite>().PlayAnimation("DarkCloud", 1);

				//aniamte version
                //this.gameObject.transform.FindChild(petal).gameObject.GetComponent<AnimateSprite>().PlayAnimation("PetalDie", 1);

				//fade version
				this.gameObject.transform.FindChild(nextPetal).gameObject.SetActive(true);
				this.transform.FindChild(nextPetal).gameObject.GetComponent<AnimateRGB>().SetRGBA(4, 1);
				this.gameObject.transform.FindChild(petal).gameObject.GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);

                petalToCorrupt = 0; //reset it
                startTimer = 0;
            }
            else if (winGlow)    //win aniamtion delay??
            {
                this.transform.FindChild("FlowerGlow").gameObject.GetComponent<AnimateSprite>().PlayAnimation("Glow", 1);
                winGlow = false;
            }
        }
    }

    public int InitialiseBox(int petalsLeft)
    {
        FastStart();

        if (petalsLeft == PlayerPrefs.GetInt("lives"))			//checks that there has not been a change
        {
            text = (GameObject)Instantiate(PressJump);
            text.transform.parent = this.transform;
            text.transform.localPosition = new Vector3(-0.6759f, -0.4786608f, -4f);


			if (Succeeded != null)
			{
				//Failed;Succeded
				text = (GameObject)Instantiate(Succeeded);
				text.transform.parent = this.transform;
				text.transform.localPosition = new Vector3(-0.6791f, -0.4551999f, -4f);
				text = (GameObject)Instantiate(SucceededShadow);
				text.transform.parent = this.transform;
				text.transform.localPosition = new Vector3(-0.6790002f, -0.4549999f, -4f);
			}



            winGlow = true;
            if (petalsLeft < 4)     //regrow petals
            {
                petalsLeft = petalsLeft + 1;
                PlayerPrefs.SetInt("lives", petalsLeft);
                string Petal;
                switch (PlayerPrefs.GetInt("lives"))
                {
                    case 2:
                        Petal = "Petal" + 3;
                        //this.gameObject.transform.FindChild(Petal).gameObject.GetComponent<AnimateSprite>().PlayAnimation("PetalRegrow", 1);
					this.gameObject.transform.FindChild(Petal).gameObject.SetActive(true);	
					this.gameObject.transform.FindChild(Petal).gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
                        break;

                    case 3:
                        Petal = "Petal" + 2;
                        //this.gameObject.transform.FindChild(Petal).gameObject.GetComponent<AnimateSprite>().PlayAnimation("PetalRegrow", 1);
					this.gameObject.transform.FindChild(Petal).gameObject.SetActive(true);	
					this.gameObject.transform.FindChild(Petal).gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);    
					break;

                    case 4:
                        Petal = "Petal" + 1;
                        //this.gameObject.transform.FindChild(Petal).gameObject.GetComponent<AnimateSprite>().PlayAnimation("PetalRegrow", 1);
					this.gameObject.transform.FindChild(Petal).gameObject.SetActive(true);
					this.gameObject.transform.FindChild(Petal).gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);    
					break;
                }
            }
        }
        else if (petalsLeft == 0)
        {
            text = (GameObject)Instantiate(PressN);
            text.transform.parent = this.transform;
            text.transform.localPosition = new Vector3(-0.68f, -0.4786608f, -4f);
            petalToCorrupt = 4;
            //special ending
        }
        else
        {
            text = (GameObject)Instantiate(PressJump);
            text.transform.parent = this.transform;
            text.transform.localPosition = new Vector3(-0.6759f, -0.4786608f, -4f);

			if (Failed != null)
			{
				text = (GameObject)Instantiate(Failed);
				text.transform.parent = this.transform;
				text.transform.localPosition = new Vector3(-0.6791f, -0.4551999f, -4f);
				text = (GameObject)Instantiate(FailedShadow);
				text.transform.parent = this.transform;
				text.transform.localPosition = new Vector3(-0.6790002f, -0.4549999f, -4f);
			}

            switch (petalsLeft)
            {
                case 1:
                    {
                        petalToCorrupt = 3;
                        //this.gameObject.transform.FindChild("Petal3").gameObject.GetComponent<AnimateSprite>().PlayAnimation("PetalDie", 1);
                    }
                    break;

                case 2:
                    {
                        petalToCorrupt = 2;
                        //this.gameObject.transform.FindChild("Petal2").gameObject.GetComponent<AnimateSprite>().PlayAnimation("PetalDie", 1);
                    }
                    break;

                case 3:
                    {
                        petalToCorrupt = 1;
                        //this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateSprite>().PlayAnimation("PetalDie", 1);
                    }
                    break;
            }
        }
        return petalsLeft;
    }

    private void UpdatePicture()
    {
        switch (PlayerPrefs.GetInt("lives"))
        {

            case 1:
                {
//                    this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1,0.5)
//                    this.gameObject.transform.FindChild("Petal2").gameObject.GetComponent<AnimateSprite>().SetFrameSet(93);
//                    this.gameObject.transform.FindChild("Petal3").gameObject.GetComponent<AnimateSprite>().SetFrameSet(93);

//					this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);
//					this.gameObject.transform.FindChild("Petal2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);
//					this.gameObject.transform.FindChild("Petal3").GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);

			this.gameObject.transform.FindChild("Petal4").GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
					
				}
			break;

            case 2:
                {
//                    this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateSprite>().SetFrameSet(93);
//                    this.gameObject.transform.FindChild("Petal2").gameObject.GetComponent<AnimateSprite>().SetFrameSet(93);
//					this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);
//					this.gameObject.transform.FindChild("Petal2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);

			this.gameObject.transform.FindChild("Petal3").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
			//this.gameObject.transform.FindChild("Petal4").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
		}
			break;

            case 3:
                {
                    //this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateSprite>().SetFrameSet(93);
					//this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);

			this.gameObject.transform.FindChild("Petal2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
			//this.gameObject.transform.FindChild("Petal3").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
			//this.gameObject.transform.FindChild("Petal4").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
                }
                break;
			//new
		case 4:
		{
			//this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateSprite>().SetFrameSet(93);
			//this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,0f,0.5f);
			this.gameObject.transform.FindChild("Petal1").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,0.5f);
			//this.gameObject.transform.FindChild("Petal2").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,4f);
			//this.gameObject.transform.FindChild("Petal3").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,4f);
			//this.gameObject.transform.FindChild("Petal4").gameObject.GetComponent<AnimateRGB>().SetRGBA(4,1f,4f);
		}
			break;
		}
	}
	
}
