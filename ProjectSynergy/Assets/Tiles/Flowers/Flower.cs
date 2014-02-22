using UnityEngine;
using System.Collections;

public class Flower : InteractableObject
{
	Grass grassTile = null;
	private void Start()
	{
		//find any grass tile that shares a coord with this and bind flowers life to that grass
		//Note: another way to have done this would hav ebeen to share flowers hitbox with grass
		Vector3 thisPos = this.transform.parent.transform.position;
//		var grassTile = LevelManager.levelManager.GrassTileList.Find (go => 
//		    go.gameObject.transform.position.x == thisPos.x && go.gameObject.transform.position.y == thisPos.y);

		foreach (var item in LevelManager.levelManager.GrassTileList) 
		{
			if (item.transform.position.x==14f 
			    && item.transform.position.y==0f)
			{
				int d =4;
			}

			if(item.transform.position.x == thisPos.x 
			   && item.transform.position.y == thisPos.y)
			{
				grassTile = item.GetComponent<Grass>();
				break;
			}

		}
		if (grassTile == null)
		{
			Debug.LogWarning("Flowers matching grass tile not found");
		}
	}

    private void Update()
    {
		if (grassTile != null && grassTile.isCorrupt && !isCorrupt) 
		{
			this.Corrupt();
		}

        if (gameObject.GetComponent<AnimateSprite>().FrameSetPlaying() == null && isCorrupt == false)//if no frameset is playing
        {
            gameObject.GetComponent<AnimateSprite>().PlayAnimation("Idle");
        }
    }
}

