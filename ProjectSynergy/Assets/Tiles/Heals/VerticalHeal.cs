using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VerticalHeal : MonoBehaviour
{
    public float healDelay = 0.1f;
    private List<InteractableObject> negativeAxisToHealList;
    private List<InteractableObject> positiveAxisToHealList;

    private void Awake()
    {
        if (this.transform.parent.gameObject.name != "VerticalHeal")
        {
            HealDown();                     //it must be a clone thats jsut been created so the awake should heal
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        HealDown();
    }

    public void HealDown()
    {
        gameObject.GetComponent<AnimateSprite>().PlayAnimation("VerticalHeal", 1);
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

        for (int i = 1; i < LevelManager.levelManager.sceneHeightInUnits; i++) //starts at 1 so as not to include itself. will go way beyond whats needed in one direction but no harm done! Just does a lot of needless checks....
        {
            for (int j = LevelManager.levelManager.groupOfObjects.Count - 1; j >= 0; j--)
            {
                InteractableObject interactableObject = LevelManager.levelManager.groupOfObjects[j].GetComponent<InteractableObject>();
                if (interactableObject.transform.root.position.y <= this.transform.root.position.y - i + 1 &&
                    interactableObject.transform.root.position.y >= this.transform.root.position.y - i)
                {
                    negativeAxisToHealList.Add(interactableObject);
                    LevelManager.levelManager.groupOfObjects.Remove(LevelManager.levelManager.groupOfObjects[j]);    //it has been added and should not get looked at again for the next position
                }
                else if (interactableObject.transform.root.position.y >= this.transform.root.position.y + i - 1 &&
                            interactableObject.transform.root.position.y <= this.transform.root.position.y + i)
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
            if (interactableObject.transform.root.position.x == this.transform.position.x)//making a list of this means it doesnt have to be run every frame..
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
