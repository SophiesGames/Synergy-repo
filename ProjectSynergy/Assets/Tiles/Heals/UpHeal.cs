using UnityEngine;
using System.Collections;

public class UpHeal : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        HealUp();
    }

    public void HealUp()
    {
        gameObject.GetComponent<AnimateSprite>().PlayAnimation("UpHeal", 1);
        object[] objectArray = InteractableObject.FindSceneObjectsOfType(typeof(InteractableObject));
        foreach (object Object in objectArray)
        {
            InteractableObject interactableObject = (InteractableObject)Object;

            if (interactableObject.isCorrupt == false && this.transform.position.x == interactableObject.transform.position.x
                &&
                interactableObject.transform.position.y > this.transform.position.y)
            {
                interactableObject.SendMessage("voidRejuvenate");                      //fake heal the oens that dont need it
            }
        }
        HealCorruptedObjectsOnly();                                 //heal the corrupted

        // AD: Call Fabric HealingPlant sound.
        Fabric.EventManager.Instance.PostEvent("HealingPlant");
    }

    private void HealCorruptedObjectsOnly()
    {
        for (int i = LevelManager.levelManager.corruptedObjects.Count - 1; i >= 0; i--)
        {
            if (
                LevelManager.levelManager.corruptedObjects[i].transform.position.x == this.transform.position.x
                &&
                LevelManager.levelManager.corruptedObjects[i].transform.position.y > this.transform.position.y
                )
            {
                LevelManager.levelManager.corruptedObjects[i].SendMessage("Rejuvenate", i);
            }
        }
    }
}
