using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nut : MonoBehaviour
{
    public GameObject explosionAnimation;
    private bool hasFallen = false;
    private Vector3 growthPosition = new Vector3(0, 0, 0);
	private bool dragging = false;

	private Collision FirstGrassCollision;
	private float FirstContactArea = 0f;
	private bool startTimer = false;
	private int timer = 0;

	private void OnCollisionStay(Collision collision)
	{
		if (hasFallen == true && this.GetComponent<Rigidbody>().velocity.y == 0 && collision.collider.tag == "Grass")
		{
			Debug.Log("Collider is Grass");
			LevelManager.levelManager.healingFinished = false;
			NutActivateExplosion(collision);
		}
	}

    private void Update()
    {
		//if 100 frames gone by since hitting then it menas its only hit 1 grass tile and not exploded. rectify this!
		if (startTimer) 
		{
			timer ++;
			if (timer > 5)
			{
				Collision chosenCollider = FirstGrassCollision;
				List<Vector3> affectedSquarePositions = new List<Vector3>();                //Create a lsit of positions that should be checked for healing
				FindAffectedSquares(affectedSquarePositions, chosenCollider);
				HealAffectedSquares(affectedSquarePositions);
				NutDestroy();
			}
		}



        if (this.GetComponent<Rigidbody>().velocity.y < -1)
        {
            hasFallen = true;
        }

		// Play acorn dragging sound.
        if (this.GetComponent<Rigidbody>().velocity.x != 0 && dragging == false && this.GetComponent<Rigidbody>().velocity.y > -1)
        {
            //audio.Play();
			Fabric.EventManager.Instance.PostEvent("AcornDrag");
			dragging = true;
		}
        else if (this.GetComponent<Rigidbody>().velocity.x == 0 || hasFallen == true)
        {
            //audio.Stop();
			Fabric.EventManager.Instance.PostEvent("AcornDrag",  Fabric.EventAction.StopSound);
			dragging = false;
        }
        //if (this.rigidbody.velocity.x < 0 && this.transform.localScale.x > 0)
        //{
        //    this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);//times it by direction to get - or positive and apply that to scale.
        //}
        //else if (this.rigidbody.velocity.x > 0 && this.transform.localScale.x < 0)
        //{
        //    this.transform.localScale = new Vector3(-1 * this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        //}

    }
    //if fall through air then enable next function
    private void OnCollisionEnter(Collision collision)
    {
        if (hasFallen == true)
        {
            Debug.Log("hasFallen == true");
            if (collision.collider.tag == "Grass")//|| collision.collider.tag == "NutBreaker"
            {
                Debug.Log("Collider is Grass");
                LevelManager.levelManager.healingFinished = false;
                NutActivateExplosion(collision);
            }
            else if (collision.collider.tag == "Rock")//|| collision.collider.tag == "NutBreaker"
            {
                Debug.Log("Collider is Rock");
                hasFallen = false;
            }
        }
    }

    ///i was getting numbers for the first colsion as 10.9 and then the second as 11. Two different points meant none could work.
	/// solved by checking if its exploded yet with a timer.
    private void NutActivateExplosion(Collision collision)
    {
        float contactArea = 0.6f;

		//NEW clarification comment: This checks to see if [0] and [1] are == in x as that means they are vertically aligned on y axis
		//if so i use the 3rd point [2] instead so i ahve points from different sides of square(i dont check for top or bttom corner) so i dont care if its top row or bootom or diagonal
        //if (Mathf.Round(collision.contacts[0].point.x * 10f) / 10f != Mathf.Round(collision.contacts[1].point.x * 10f) / 10f)                                 //stupid statement but the corners the array corrsepond to changes randomly it seems. These numbers msut be rounded to 1 decimal place to ignore tiny differences.
        //{

        //    contactArea = collision.contacts[0].point.x - collision.contacts[1].point.x;
        //}
        //else
        //{
        //    contactArea = collision.contacts[0].point.x - collision.contacts[2].point.x;
        //}

        contactArea = Mathf.Abs(contactArea);                 //avoid negative numbers
		if (contactArea >= 0.5f)                               //this check will only apply to 1 square. the one who should act as the centre point for nut functioality
        {
			Debug.Log("found a contact area:"+ contactArea +" >= 0.5f");
            List<Vector3> affectedSquarePositions = new List<Vector3>();                //Create a lsit of positions that should be checked for healing
            FindAffectedSquares(affectedSquarePositions, collision);
            HealAffectedSquares(affectedSquarePositions);
            NutDestroy();
        }
		else
		{
			startTimer = true;

			if (FirstGrassCollision != null )
			{
				Collision chosenCollider = FirstContactArea > contactArea ? FirstGrassCollision : collision;
				List<Vector3> affectedSquarePositions = new List<Vector3>();                //Create a lsit of positions that should be checked for healing
				FindAffectedSquares(affectedSquarePositions, chosenCollider);
				HealAffectedSquares(affectedSquarePositions);
				NutDestroy();
			}
			else
			{
				FirstGrassCollision = collision;
				FirstContactArea = contactArea;
			}
			Debug.Log("contactArea:" + contactArea + "");
		}
    }

    private void FindAffectedSquares(List<Vector3> affectedSquarePositions, Collision collision)
    {
        growthPosition = collision.collider.transform.position;                                 //set original
        affectedSquarePositions.Add(growthPosition + new Vector3(0, 0, 0));                     //add original
        affectedSquarePositions.Add(growthPosition + new Vector3(0, collision.collider.transform.lossyScale.y, 0));  //add one up square for things that stick up like mushrooms.
        affectedSquarePositions.Add(growthPosition + new Vector3(collision.collider.transform.lossyScale.x, 0, 0));  //add right
        affectedSquarePositions.Add(growthPosition + new Vector3(-collision.collider.transform.lossyScale.x, 0, 0)); //add left
        affectedSquarePositions.Add(growthPosition + new Vector3(collision.collider.transform.lossyScale.x, collision.collider.transform.lossyScale.y, 0));   //add right and up one
        affectedSquarePositions.Add(growthPosition + new Vector3(-collision.collider.transform.lossyScale.x, collision.collider.transform.lossyScale.y, 0));  //add left and up one

        //for (int i = 0; i < affectedSquarePositions.Count; i++)
        //{
        //    Debug.Log(affectedSquarePositions[i]);
        //}
        //heal squares if needed
    }

    private void HealAffectedSquares(List<Vector3> affectedSquarePositions)
    {
        //Check corruptable objects list against objects that should be healed. If they match then heal them.
        for (int j = 0; j < affectedSquarePositions.Count; j++)
        {
            //heal
            for (int i = LevelManager.levelManager.corruptedObjects.Count - 1; i >= 0; i--)
            {
                if (affectedSquarePositions[j] == LevelManager.levelManager.corruptedObjects[i].gameObject.GetComponent<InteractableObject>().transform.root.position)
                {
                    LevelManager.levelManager.corruptedObjects[i].gameObject.GetComponent<InteractableObject>().Rejuvenate(i);
                }
            }
            //void rejuvenate
            for (int i = LevelManager.levelManager.nonCorruptedObjects.Count - 1; i >= 0; i--)
            {
                if (affectedSquarePositions[j] == LevelManager.levelManager.nonCorruptedObjects[i].gameObject.GetComponent<InteractableObject>().transform.root.position)
                {
                    LevelManager.levelManager.nonCorruptedObjects[i].gameObject.GetComponent<InteractableObject>().voidRejuvenate();
                }
            }
        }
    }

    private void NutDestroy()
    {
        //Debug.Log("NutDestroy");
        GameObject expAnim;
        expAnim = (GameObject)Instantiate(explosionAnimation, transform.position, transform.rotation);
        expAnim.gameObject.GetComponent<AnimateSprite>().PlayAnimation("NutExplosion", 1, true);

        // AD: Call Fabric HealingPlant sound.
        Fabric.EventManager.Instance.PostEvent("AcornExplode");

        Destroy(this.gameObject);
    }
}
