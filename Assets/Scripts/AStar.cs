using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.PriorityQueue;

public class AStar
{
	Node[,] nodes;
     
    public AStar(Node[,] nodes)
    {
    	this.nodes = nodes;
    }

    public List<Node> Search(Node start, Node goal)
    {
    	List<Node> visited = new List<Node>();
    	PriorityQueue<AStarItem,float> queue = new PriorityQueue<AStarItem,float>(0);
    	AStarItem firstItem = new AStarItem(start);
    	queue.Insert(firstItem,EuclideanDistance(start,goal));
    	
    	while(queue.Top() != null)
    	{
    		AStarItem cur = queue.Pop();
    		if(IsGoalNode(cur.Item,goal))
    			return GetPath(cur);

    		List<Node> adjacentNodes = GetAdjacentNodes(cur.Item);
    		if(adjacentNodes.Count > 0)
    		{
    			foreach(Node n in adjacentNodes)
	    		{
	    			if(!visited.Contains(n))
	    			{
	    				AStarItem temp = new AStarItem(n,cur);
	    				queue.Insert(temp,EuclideanDistance(n,goal));
	    			}
	    		}
    		}

    		visited.Add(cur.Item);
    	}

    	return new List<Node>();
    }

    bool IsGoalNode(Node cur, Node goal)
    {
    	return cur.Equals(goal);

    }

    public float EuclideanDistance(Node a, Node b)
    {
    	return Mathf.Sqrt((b.YPos - a.YPos) * (b.YPos - a.YPos) + (b.XPos - a.XPos) * (b.XPos - a.XPos));
    }

    List<Node> GetAdjacentNodes(Node cur)
    {
    	List<Node> adjacentNodes = new List<Node>();
    	int xInd = cur.XInd;
    	int yInd = cur.YInd;
    	if(xInd - 1 >= 0)
    	{
    		if(nodes[xInd-1,yInd].CollisionFree)
    		{
    			adjacentNodes.Add(nodes[xInd-1,yInd]);
    		}
    	}
    	if(yInd + 1 < nodes.GetLength(1))
    	{
    		if(nodes[xInd,yInd+1].CollisionFree)
    		{
    			adjacentNodes.Add(nodes[xInd,yInd+1]);
    		}
    	}
    	if(xInd + 1 < nodes.GetLength(0))
    	{
    		if(nodes[xInd+1,yInd].CollisionFree)
    		{
    			adjacentNodes.Add(nodes[xInd+1,yInd]);
    		}
    	}
    	if(yInd - 1 >= 0)
    	{
    		if(nodes[xInd,yInd-1].CollisionFree)
    		{
    			adjacentNodes.Add(nodes[xInd,yInd-1]);
    		}
    	}

    	return adjacentNodes;
    }

    List<Node> GetPath(AStarItem cur)
    {
    	List<Node> temp = new List<Node>();
    	AStarItem current = cur;
    	
		temp.Add(current.Item);
    	while(current.Prev != null)
    	{
    		current = current.Prev;
    		temp.Add(current.Item);
    	}
    	
    	return temp;
    }

}

public class AStarItem
{
	Node item;
	AStarItem prev;

	public AStarItem(Node n)
	{
		item = n;
	}

	public AStarItem(Node n, AStarItem prev)
	{
		item = n;
		this.prev = prev;
	}

	public Node Item
	{
		get{
			return item;
		}

	}

	public AStarItem Prev
	{
		get{
			return prev;
		}
	}

}
