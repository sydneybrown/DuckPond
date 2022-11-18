using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCollision : MonoBehaviour
{
	GraphController gc;

    // Start is called before the first frame update
    void Start()
    {
    	gc = GameObject.Find("Water").GetComponent<GraphController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
    	if(!col.gameObject.name.Equals("Duck") && !col.gameObject.name.Contains("Swan") && !col.gameObject.name.Contains("Dot"))
    	{
    		gc.DisableNode(transform.position.x,transform.position.y);
    	}
        	
        	
    }
}
