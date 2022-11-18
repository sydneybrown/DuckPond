using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
	public GameObject prefabDot;
	public float cellSize = 0.5f;
	Node[,] nodes;
	Renderer rend;
	bool firstUpdate = true;
	List<GameObject> dots;
	int numX;
	int numY;
	float width;
	float height;
	void Awake()
	{
		dots = new List<GameObject>();
		rend = GetComponent<Renderer>();
		Vector3 max = rend.bounds.max;
		Vector3 min = rend.bounds.min;
		width = max.x - min.x;
		height = max.y - min.y;
		numX = (int) (width / cellSize);
		numY = (int) (height / cellSize);
		nodes = new Node[numX,numY];
		for(int i = 0; i < numX; i++)
		{
			for(int j = 0; j < numY; j++)
			{
				float x = -width / 2 + cellSize * (i + 0.5f); 
				float y = -height / 2 + cellSize * (j + 0.5f);
				nodes[i,j] = new Node(x,y,i,j);
				GameObject tempDot = Instantiate(prefabDot, new Vector3(x,y,0f), Quaternion.identity);
				//tempDot.GetComponent<GraphCollision>().SetNode(nodes[i,j]);
				dots.Add(tempDot);


			}
		}
		
	}

    // Update is called once per frame
    void Update()
    {
    	if(firstUpdate)
    	{
    		int ct = 0;
		for(int i = 0; i < numX; i++)
		{
			for(int j = 0; j < numY; j++)
			{
				if(nodes[i,j].CollisionFree == false)
				{
					GameObject tmp = dots[ct];
					dots.Remove(tmp);
					Destroy(tmp);
				}
				else
					ct++;

			}
		}
    		
		firstUpdate = false;

    	}
        
    }

    public void DisableNode(float x, float y)
    {
    	nodes[PosToIndexX(x),PosToIndexY(y)].CollisionFree = false;
    }

    int PosToIndexX(float x)
    {
    	return (int) ((x + width/2)/cellSize - 0.5f);
    }

    int PosToIndexY(float y)
    {
    	return (int) ((y + height/2)/cellSize - 0.5f);
    }

    public Node[,] GetNodes()
    {
    	return nodes;
    }
}

public class Node
{
	private float xPos;
	private float yPos;
	private bool collisionFree;
	private int xInd;
	private int yInd;

	public Node(float x, float y, int i, int j)
	{
		xPos = x;
		yPos = y;
		xInd = i;
		yInd = j;
		collisionFree = true;
	}

	public float XPos
	{
		get
		{
			return xPos;
		}
	}

	public float YPos
	{
		get
		{
			return yPos;
		}
	}

	public int XInd
	{
		get
		{
			return xInd;
		}
	}

	public int YInd
	{
		get
		{
			return yInd;
		}
	}

	public bool CollisionFree
	{
		get
		{
			return collisionFree;
		}

		set
		{
			collisionFree = value;
		}
	}




}
