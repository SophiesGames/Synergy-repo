using UnityEngine;
using System.Collections;

public class Flower : InteractableObject
{
	//-1 as tiles placed at x = 14 actualy start at 13 and end at 15 and mid point 14
	 
	Grass grassTile = null;
	private void Start()
	{
		//find any grass tile that shares a coord with this and bind flowers life to that grass
		//Note: another way to have done this would hav ebeen to share flowers hitbox with grass

		Vector3 thisPos = this.transform.parent.transform.position;
		int startTilePosMod = -1;
		var grassTile  = SetGrassTile (thisPos, startTilePosMod);

		if (grassTile == null)
		{
			//try the one ahead. Only do this if null at end thoguh as it might belong to tile infront
			startTilePosMod = 1;
			grassTile = SetGrassTile(thisPos, startTilePosMod);
			
			if(grassTile == null)
			{
				Debug.LogWarning("Matching grass tile not found for flower at:" + thisPos);
			}
		}

	}


	private Grass SetGrassTile(Vector3 thisPos, int startTilePosMod)
	{

		foreach (var item in LevelManager.levelManager.GrassTileList) 
		{
			if((item.transform.position.x == thisPos.x 
			    && item.transform.position.y == thisPos.y)
			   ||
			   (item.transform.position.x + startTilePosMod) == thisPos.x 
			   && item.transform.position.y == thisPos.y
			   )
			{
				grassTile = item.transform.FindChild("GrassLogic").GetComponent<Grass>();
				return grassTile;
			}
		}
		return null;

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

