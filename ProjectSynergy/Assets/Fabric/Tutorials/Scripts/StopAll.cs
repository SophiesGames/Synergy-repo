using UnityEngine;
using System.Collections;

public class StopAll : MonoBehaviour {
	
	public bool stopAll = false;

    Fabric.Component component;// = Fabric.FabricManager.Instance.GetComponentByName("Audio_Group1") as Fabric.GroupComponent;

	// Use this for initialization
	void Start () {

        //Fabric.Component component = Fabric.FabricManager.Instance.GetComponentByName("Audio_Group1") as Fabric.Component;
	}
	
	// Update is called once per frame
	void Update () {
	
		//if(stopAll)
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
		{
            Fabric.Component component = Fabric.FabricManager.Instance.GetComponentByName("Audio_Group1") as Fabric.Component;
			//Fabric.Component component = Fabric.FabricManager.Instance.GetComponentByName("Audio_Group1") as Fabric.GroupComponent;
			
			component.Stop();//(true,false);
            //Fabric.EventManager.Instance.PostEvent("Group", Fabric.EventAction.StopSound, gameObject);

            Fabric.EventManager.Instance.PostEvent("A");//, gameObject);
			
			//stopAll = false;
		}

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Fabric.Component component = Fabric.FabricManager.Instance.GetComponentByName("Audio_Group1") as Fabric.Component;
            //Fabric.Component component = Fabric.FabricManager.Instance.GetComponentByName("Audio_Group1") as Fabric.GroupComponent;

            component.Stop();//true,false);
            //Fabric.EventManager.Instance.PostEvent("Group", Fabric.EventAction.StopSound, gameObject);

            Fabric.EventManager.Instance.PostEvent("B");//, gameObject);

            //stopAll = false;
        }

    }
}
