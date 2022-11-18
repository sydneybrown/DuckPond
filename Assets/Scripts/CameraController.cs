using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	/**
    	https://stackoverflow.com/questions/65816546/unity-camera-follows-player-script#:~:text=Making%20the%20camera%20following%20the,to%20be%20from%20the%20player.
    	*/
    	transform.position = new Vector3(player.transform.position.x,player.transform.position.y,transform.position.z);
        
    }
}
