using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovementController : MonoBehaviour
{
	Transform tr;
	public float speed = 3f;
    bool facingRight = true;
    SceneController scene;
    int score = 0;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        tr = gameObject.transform;
        scene = GameObject.Find("Main Camera").GetComponent<SceneController>();
        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Node> bread = scene.GetThatBread();
        Node temp = CheckForCollision(bread);
        if(temp != null)
        {
            score++;
            scene.RemoveBread(temp);
            scoreText.text = score + "";
        }
        float xComponent = 0f;
        float yComponent = 0f;

    	if(Input.GetKey(KeyCode.A))
    	{
    		xComponent -= Time.deltaTime;
            if(facingRight)
            {
                tr.Rotate(0f,180f,0f);
                facingRight = false;
            }
    	}
    	if(Input.GetKey(KeyCode.W))
    	{
    		yComponent += Time.deltaTime;
    	}
    	if(Input.GetKey(KeyCode.D))
    	{
    		xComponent += Time.deltaTime;
            if(!facingRight)
            {
                tr.Rotate(0f,180f,0f);
                facingRight = true;
            }
    	}
    	if(Input.GetKey(KeyCode.S))
    	{
    		yComponent -= Time.deltaTime;
            
    	}

        Vector3 change = new Vector3(xComponent,yComponent,0f);
        change.Normalize();
        change = change * Time.deltaTime * speed;
        tr.position += change;
        
    }

    Node CheckForCollision(List<Node> nodes)
    {
        foreach(Node n in nodes)
        {
            if(WithinRadius(n,0.5f))
                return n;
        }
        return null;
    }

    bool WithinRadius(Node target, float radius)
    {
        float xComponent = (target.XPos - transform.position.x) * (target.XPos - transform.position.x);
        float yComponent = (target.YPos - transform.position.y) * (target.YPos - transform.position.y);
        return Mathf.Sqrt(xComponent + yComponent) <= radius;
    }
}
