using UnityEngine;
using System.Collections;

public class PollenatePlant : MonoBehaviour
{
    public AudioClip PollenExplodeSFX;
    public AudioClip RootGrowSFX;

    private bool playerVisited = false;
    //private bool follow = false;
    Player player;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            player = collider.gameObject.GetComponent<Player>();
            AnimateSprite animation = gameObject.GetComponent<AnimateSprite>();

            if (playerVisited == false)
            {
                if (player.pollenatedPlantName == "")                                   //if it has a no name, it has not visited another plant
                {
                    //play sound
					//audio.PlayOneShot(PollenExplodeSFX);
					Fabric.EventManager.Instance.PostEvent("Pollen/Grab"); 

                    animation.PlayAnimation("PollenExplode", 1);
                    player.pollenatedPlantName = gameObject.transform.parent.name;                                      //set the nametag so player knows whose pollen is attached
                    //make player covered in seed
                    playerVisited = true;           //required as well as the anme as teh name will be set to "" after both ahve been used but the plant cant be used ever again.
                	player.transform.Find("PollenParticles").GetComponent<ParticleSystem>().Play();
					player.transform.Find("PlayerParticleSystem").GetComponent<ParticleSystem>().Stop();
					this.transform.parent.Find("PollenHeadParticles").GetComponent<ParticleSystem>().Stop();
				}

                else if (player.pollenatedPlantName == gameObject.transform.parent.name)                 //if it has a name, check its the same plant type by checking anme, then do stuff
                {
					player.transform.Find("PollenParticles").GetComponent<ParticleSystem>().Stop();
					player.transform.Find("PlayerParticleSystem").GetComponent<ParticleSystem>().Play();
                    
					//play sound
					//audio.PlayOneShot(RootGrowSFX);
					Fabric.EventManager.Instance.PostEvent("Pollen/Drop");

					LevelManager.levelManager.healingFinished = false;
                    animation.PlayAnimation("SuckPollen", 1, true);
                    player.pollenatedPlantName = "";
                    //make player NOT covered in seed
                    playerVisited = true;
                }
                //else no reaction. Happens when player pollenated and touches other pollen plant of wrong type
            }
        }
    }

    private void AnimationFinished(string frameSetName)
    {
        if (frameSetName == "SuckPollen")
        {
            gameObject.GetComponent<AnimateSprite>().PlayAnimation("ShrinkPollen", 1);
            Transform stem = this.transform.parent.FindChild("Stem");
            stem.gameObject.GetComponent<AnimateSprite>().PlayAnimation("Birth", 1, true);
        }
    }

    //private void Update()
    //{
    //    if (transform.parent == null)
    //    {
    //        Debug.Log("player" + player);

    //        float tempX = transform.position.x;
    //        if (transform.position.x > player.transform.position.x + 0.1)//makes a box 0.1 to the right and in next stement 0.1 to left. In totla a box of .2 for the 0.1 movement increments to bump into.
    //        {
    //            tempX = tempX - 0.1f;
    //            transform.position = new Vector3(tempX, transform.position.y, transform.position.z);
    //        }
    //        else if (transform.position.x < player.transform.position.x -0.1)
    //        {
    //            tempX = tempX + 0.1f;
    //            transform.position = new Vector3(tempX, transform.position.y, transform.position.z);
    //        }
    //        else
    //        {
    //            transform.parent = player.transform;
    //            transform.localPosition = new Vector3(0, 0, 0);
    //        }
    //    }
    //}


}