using UnityEngine;
using System.Collections;

public class LastLevel : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
		
		this.gameObject.GetComponent<GreyBox> ().InitialiseBox (PlayerPrefs.GetInt ("lives"));
        GameObject.Find("Synergy").gameObject.GetComponent<AnimateSprite>().PlayAnimation("Synergy", 1);
        GameObject.Find("Glow").gameObject.GetComponent<AnimateSprite>().PlayAnimation("Glow", 1);
        MusicManager.musicManager.SetEndMusic();

    }
}
