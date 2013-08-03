using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
    public bool startCorrupt = false;
    [HideInInspector]
    public bool isCorrupt = false;
    [HideInInspector]
    public Vector3 worldPosition;
	[HideInInspector]
	public string objectType = ""; // AD: What kind of object is this?
	
    public AudioClip corruptSound;
    public AudioClip rejuvenateSound;

    private void Start()
    {
		// AD: Assign object type. Grab it from class name, 
		// but we could also override the function in child classes.
		objectType = this.GetType().Name;
		
        if (startCorrupt)
        {
            gameObject.GetComponent<AnimateSprite>().SetFrameSet("idleCorrupt");
            LevelManager.levelManager.nonCorruptedObjects.Remove(gameObject);//take from one lsit
            LevelManager.levelManager.corruptedObjects.Add(gameObject);//and add to the other
            isCorrupt = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            Corrupt();
        }
    }

    private void AnimationFinished(string frameSetName)
    {
        //LevelManager.levelManager.
    }

    public void Corrupt()
    {
        if (isCorrupt == false)
        {
            if (corruptSound != null)
            {
				// AD: Don't use basic Unity audio.
                //audio.PlayOneShot(corruptSound);
				
				// AD: Instead, call Fabric "Corrupt" event with current class name as parameter.
				Fabric.EventManager.Instance.PostEvent("Corrupt", Fabric.EventAction.SetSwitch, objectType);
				Fabric.EventManager.Instance.PostEvent("Corrupt");
            }
            gameObject.GetComponent<AnimateSprite>().PlayAnimation("Corrupt", 1);
            LevelManager.levelManager.nonCorruptedObjects.Remove(gameObject);//take from one lsit
            LevelManager.levelManager.corruptedObjects.Add(gameObject);//and add to the other
            isCorrupt = true;
        }
    }

    public void Rejuvenate(int corruptedObjectIndex)
    {
        if (corruptSound != null)
        {
			// @todo: AD - call Fabric rejuvenate event.
            //audio.PlayOneShot(rejuvenateSound);
			
			// @todo:
			// AD: Instead, call Fabric "Rejuvenate" event with current class name as parameter.
			Fabric.EventManager.Instance.PostEvent("Corrupt", Fabric.EventAction.SetSwitch, "Warning");
			Fabric.EventManager.Instance.PostEvent("Corrupt");
        }
        AnimateSprite animateSprite = gameObject.GetComponent<AnimateSprite>();
        animateSprite.PlayAnimation("Rejuvenate", 1);
        isCorrupt = false;

        GameObject healedGameObject = LevelManager.levelManager.corruptedObjects[corruptedObjectIndex];//find object
        LevelManager.levelManager.nonCorruptedObjects.Add(healedGameObject);//add to heal list
        LevelManager.levelManager.corruptedObjects.RemoveAt(corruptedObjectIndex);//remove from corrupt list

    }

    public void voidRejuvenate()
    {
        audio.PlayOneShot(rejuvenateSound);
        AnimateSprite animateSprite = gameObject.GetComponent<AnimateSprite>();
        animateSprite.PlayAnimation("voidRejuvenate", 1);
    }

    public virtual void TriggerObject(Player player)
    {
        //overide with functionality
    }
    public virtual void TriggerObject()
    {
    }
}


//public virtual void PlayerCollision(Player player)
//{
//    if (isCorrupt == false)
//    {
//        if (canCorrupt == true)
//        {
//            Corrupt();
//        }
//        TriggerObject(player);                                              //how to decide which function to call?
//        TriggerObject();
//    }
//}