using UnityEngine;
using System.Collections;

public class Flower : InteractableObject
{
    private void Update()
    {
        if (
            gameObject.GetComponent<AnimateSprite>().FrameSetPlaying() == null && isCorrupt == false)//if no frameset is playing
        {
            gameObject.GetComponent<AnimateSprite>().PlayAnimation("Idle");
        }
    }
}

