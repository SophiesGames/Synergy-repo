using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour
{
    CloudSystem cloudSystem;

    private void Start()
    {
        cloudSystem = this.gameObject.transform.parent.gameObject.GetComponent<CloudSystem>();

        Color color = GetComponent<Renderer>().material.color;
        color.a = cloudSystem.opacity;
        this.GetComponent<Renderer>().material.color = color;
    }

    private void Update()      
    {
        this.transform.Translate(Vector3.right * (Time.deltaTime * cloudSystem.cloudSpeed));//adjusts for lag or too fast fps
        
        if (this.transform.position.x > cloudSystem.maxWidth + 4)//4 just to  make sure its clear of view
        {
            cloudSystem.CloudSetUp(this.gameObject);
        }
    }
}
