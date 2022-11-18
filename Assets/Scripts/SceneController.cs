using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
	GraphController graph;
	public GameObject prefabSwan;
	public GameObject prefabBread;
	AStar astar;
	Node[,] nodes;
	bool firstUpdate = true;
	float time = 0f;
	List<GameObject> swans;
	List<GameObject> bread;
	List<BreadTarget> breadTargets;
	List<Node> targets;
	public int numSwans = 2;
    // Start is called before the first frame update
    void Start()
    {
    	graph = GameObject.Find("Water").GetComponent<GraphController>();
    	swans = new List<GameObject>();
    	bread = new List<GameObject>();
    	breadTargets = new List<BreadTarget>();
    	targets = new List<Node>();

    }

    // Update is called once per frame
    void Update()
    {
    	
    	if(firstUpdate)
    	{
    		for(int i = 0; i < numSwans; i++)
    		{
    			swans.Add(Instantiate(prefabSwan));
    		}
    		firstUpdate = false;
    		nodes = graph.GetNodes();
    		AddBread();
    	}

    	time += Time.deltaTime;
    	if(time > Random.Range(5,9))
    	{
	    	AddBread();
	    	time = 0f;
    	}
    }

    void AddBread()
    {
    	int randX = 0;
        int randY = 0;
        do
        {
            randX = Random.Range(0,nodes.GetLength(0));
            randY = Random.Range(0,nodes.GetLength(1));

        }
        while(!nodes[randX,randY].CollisionFree);
    	GameObject b = Instantiate(prefabBread, new Vector3(nodes[randX,randY].XPos,nodes[randX,randY].YPos,0f), Quaternion.identity);
    	breadTargets.Add(new BreadTarget(nodes[randX,randY],b));
    	targets.Add(nodes[randX,randY]);
    }

    public List<Node> GetThatBread()
    {
    	return targets;
    }

    public void RemoveBread(Node t)
    {
    	targets.Remove(t);
    	BreadTarget bt = GetBreadFromTarget(t);
    	if(bt != null)
    	{
    		breadTargets.Remove(bt);
    		Destroy(bt.Bread);
    	}   	
    }

    BreadTarget GetBreadFromTarget(Node t)
    {
    	foreach(BreadTarget bt in breadTargets)
    	{
    		if(t.Equals(bt.Target))
    			return bt;
    	}
    	return null;
    }
}

public class BreadTarget
{
	Node target;
	GameObject bread;

	public BreadTarget(Node t, GameObject b)
	{
		bread = b;
		target = t;
	}

	public Node Target
	{
		get
		{
			return target;
		}
	}
	public GameObject Bread
	{
		get
		{
			return bread;
		}
	}

}
