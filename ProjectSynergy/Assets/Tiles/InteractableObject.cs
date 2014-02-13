using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractableObject : MonoBehaviour
{
	public bool canStartCorrupt = false;
    public bool startsCorrupt = false;
    [HideInInspector]
    public bool isCorrupt = false;
    [HideInInspector]
    public Vector3 worldPosition;
	[HideInInspector]
	public string objectType = ""; // AD: What kind of object is this.
	
    public AudioClip corruptSound;
    public AudioClip rejuvenateSound;

	private void Awake()
	{
		//make sure levelmanager already exists. SImple calling something on it should be enough to force ti to get made here
		if (LevelManager.levelManager.nonCorruptedObjects != null) {
				}
		}

    private void Start()
    {
		// AD: Assign object type. Grab it from class name, 
		// but we could also override the function in child classes.
		objectType = this.GetType().Name;

		if (this.GetType() == typeof(Flower))
		{
			int d = 44;
		}
		if (startsCorrupt) 
		{
			StartsCorrupt();
				}

    }

	public void StartsCorrupt()
	{
			gameObject.GetComponent<AnimateSprite>().SetFrameSet("idleCorrupt");
			LevelManager.levelManager.nonCorruptedObjects.Remove(gameObject);//take from one lsit
			LevelManager.levelManager.corruptedObjects.Add(gameObject);//and add to the other
			isCorrupt = true;
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
            // AD: Call "Corrupt" event with current class name as parameter.
            //Debug.Log("Corrupt/" + objectType);
            Fabric.EventManager.Instance.PostEvent("Corrupt/" + objectType);

            gameObject.GetComponent<AnimateSprite>().PlayAnimation("Corrupt", 1);
            LevelManager.levelManager.nonCorruptedObjects.Remove(gameObject);//take from one lsit
            LevelManager.levelManager.corruptedObjects.Add(gameObject);//and add to the other
            isCorrupt = true;
        }
    }

    public void Rejuvenate(int corruptedObjectIndex)
    {
        // AD: Call "Heal" event with current class name as parameter.
        //Debug.Log("Heal/" + objectType);
        Fabric.EventManager.Instance.PostEvent("Heal/" + objectType);

        AnimateSprite animateSprite = gameObject.GetComponent<AnimateSprite>();
        animateSprite.PlayAnimation("Rejuvenate", 1);
        isCorrupt = false;

        GameObject healedGameObject = LevelManager.levelManager.corruptedObjects[corruptedObjectIndex];//find object
        LevelManager.levelManager.nonCorruptedObjects.Add(healedGameObject);//add to heal list
        LevelManager.levelManager.corruptedObjects.RemoveAt(corruptedObjectIndex);//remove from corrupt list

    }

    public void voidRejuvenate()
    {
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