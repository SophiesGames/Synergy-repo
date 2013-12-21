using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nut : MonoBehaviour
{
    public GameObject explosionAnimation;
    private bool hasFallen = false;
    private Vector3 growthPosition = new Vector3(0, 0, 0);

    private void Update()
    {
        if (this.rigidbody.velocity.y < -1)
        {
            hasFallen = true;
        }

        // @todo: AD: Perhaps convert this to Fabric?
        if (this.rigidbody.velocity.x != 0 && audio.isPlaying == false)
        {
            audio.Play();
        }
        else if (this.rigidbody.velocity.x == 0)
        {
            audio.Stop();
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
        //Debug.Log("OnCollisionEnter");
        if (hasFallen == true)
        {
            //Debug.Log("hasFallen = true");
            if (collision.collider.tag == "Grass")//|| collision.collider.tag == "NutBreaker"
            {
                //Debug.Log("Grass");
                LevelManager.levelManager.healingFinished = false;
                NutLogic(collision);
            }
            else if (collision.collider.tag == "Rock")//|| collision.collider.tag == "NutBreaker"
            {
                //Debug.Log("Rock");
                hasFallen = false;
            }
        }
    }

    //if this code proves buggy, it might be that the list is being filled in a function that is lost when it retursn to the main function.
    private void NutLogic(Collision collision)
    {
        float contactArea;

        if (Mathf.Round(collision.contacts[0].point.x * 10f) / 10f != Mathf.Round(collision.contacts[1].point.x * 10f) / 10f)                                 //stupid statement but the corners the array corrsepond to changes randomly it seems. These numbers msut be rounded to 1 decimal place to ignore tiny differences.
        {
            contactArea = collision.contacts[0].point.x - collision.contacts[1].point.x;
        }
        else
        {
            contactArea = collision.contacts[0].point.x - collision.contacts[2].point.x;
        }
        contactArea = Mathf.Abs(contactArea);                 //avoid negative numbers
        if (contactArea >= 0.41f)                               //this check will only apply to 1 square. the one who should act as the centre point for nut functioality
        {
            List<Vector3> affectedSquarePositions = new List<Vector3>();                //Create a lsit of positions that should be checked for healing
            FindAffectedSquares(affectedSquarePositions, collision);
            HealAffectedSquares(affectedSquarePositions);
            NutDestroy();
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
