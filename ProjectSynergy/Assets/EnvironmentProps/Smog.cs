using UnityEngine;
using System.Collections;

public class Smog : MonoBehaviour 
{
    public float SmogCap = 0.8f;
    private float transparency;

    private void Update()
    {
        transparency = LevelManager.levelManager.corruptedObjects.Count * 0.08f;

        if (transparency > SmogCap)
        {
            transparency = SmogCap;
        }

        this.gameObject.GetComponent<AnimateRGB>().SetRGBA(4, transparency);
		
		// @todo: AD: Update music "Destruction level". Here?
    }
}
