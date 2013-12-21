using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorizontalHeal : MonoBehaviour
{
    public float healDelay = 0.1f;
    private List<InteractableObject> negativeAxisToHealList;
    private List<InteractableObject> positiveAxisToHealList;

    private void Awake()
    {
        if (this.transform.parent.gameObject.name != "HorizontalHeal" && this.transform.parent.gameObject.name != "HorizontalHealFade")
        {
            HealHorizontally();                     //it must be a clone thats jsut been created so the awake should heal
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        HealHorizontally();
    }

    public void HealHorizontally()
    {
        this.gameObject.GetComponent<AnimateSprite>().PlayAnimation("HorizontalHeal", 1);
        CreateHealList();
        LevelManager.levelManager.healingFinished = false;
        StartCoroutine("Heal");

        // AD: Call Fabric HealingPlant sound.
        Fabric.EventManager.Instance.PostEvent("HealingPlant");
    }

    private void CreateHealList()
    {
        negativeAxisToHealList = new List<InteractableObject>();
        positiveAxisToHealList = new List<InteractableObject>();

        CreateGroupList();                  //creates a group list that is saved to levelmanger. n this case the group is fille dwith things on y axis for horizontal heal

        for (int i = 1; i < LevelManager.levelManager.sceneWidthInUnits + 1; i++) //starts at 1 so as to compnesate for the -1 range check. will go way beyond whats needed in one direction but no harm done! Just does a lot of needless checks....
        { 
            for (int j = LevelManager.levelManager.groupOfObjects.Count - 1; j >= 0; j--)
            {
                InteractableObject interactableObject = LevelManager.levelManager.groupOfObjects[j].gameObject.GetComponent<InteractableObject>();
                //check it falls into a range of the furthest out being checked right now and bigger than the last furthers out check. This allows for tiles to have decimal places and still get counted in heal list.
                if (interactableObject.transform.root.position.x <= this.transform.position.x - i + 1 && 
                    interactableObject.transform.root.position.x >= this.transform.position.x - i)
                {
                    negativeAxisToHealList.Add(interactableObject);
                    LevelManager.levelManager.groupOfObjects.Remove(LevelManager.levelManager.groupOfObjects[j]);    //it has been added and should not get looked at again for the next position
                }
                else if (interactableObject.transform.root.position.x >= this.transform.root.position.x + i - 1 &&
                    interactableObject.transform.root.position.x <= this.transform.root.position.x + i)
                {
                    positiveAxisToHealList.Add(interactableObject);
                    LevelManager.levelManager.groupOfObjects.Remove(LevelManager.levelManager.groupOfObjects[j]);
                }
            }
        }
        LevelManager.levelManager.groupOfObjects.Clear();         //empty for next use
    }

    private void CreateGroupList()
    {
        foreach (GameObject Object in LevelManager.levelManager.interactableObjects)
        {
            GameObject interactableObject = (GameObject)Object;
            if (interactableObject.transform.root.position.y == this.transform.position.y)//making a list of this means it doesnt have to be run every frame..
            {
                LevelManager.levelManager.groupOfObjects.Add(interactableObject);
            }
        }
    }

    private IEnumerator Heal()
    {
        while (negativeAxisToHealList.Count != 0 || positiveAxisToHealList.Count != 0)
        {
            //negative
            if (negativeAxisToHealList.Count != 0)
            {
                if (negativeAxisToHealList[0].isCorrupt == false)
                {
                    negativeAxisToHealList[0].SendMessage("voidRejuvenate");                      //fake heal the oens that dont need it	
                }
                else
                {
                    HealCorruptedObjectsOnly(negativeAxisToHealList[0].transform.root.position);
                }
                negativeAxisToHealList.RemoveAt(0);
            }

            //positive
            if (positiveAxisToHealList.Count != 0)
            {
                if (positiveAxisToHealList[0].isCorrupt == false)
                {
                    positiveAxisToHealList[0].SendMessage("voidRejuvenate");                      //fake heal the oens that dont need it	
                }
                else
                {
                    HealCorruptedObjectsOnly(positiveAxisToHealList[0].transform.root.position);
                }
                positiveAxisToHealList.RemoveAt(0);
            }
            yield return new WaitForSeconds(healDelay);
        }
        negativeAxisToHealList.Clear();//make sure
        positiveAxisToHealList.Clear();
        LevelManager.levelManager.healingFinished = true;
        yield return null;
    }

    private void HealCorruptedObjectsOnly(Vector3 objPos)
    {
        for (int i = LevelManager.levelManager.corruptedObjects.Count - 1; i >= 0; i--)
        {
            if (objPos == LevelManager.levelManager.corruptedObjects[i].gameObject.GetComponent<InteractableObject>().transform.root.position)
            {
                LevelManager.levelManager.corruptedObjects[i].BroadcastMessage("Rejuvenate", i);
            }
        }
    }
}
