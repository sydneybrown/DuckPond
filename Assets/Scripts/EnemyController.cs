using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    float time = 0f;
	Vector3 direction;
	public float speed = 2f;
    List<Node> path;
    Node pos;
    AStar astar;
    GraphController graph;
    Node[,] nodes;
    List<Node> targets;
    Vector3 currentDirection;
    Node currentTarget;
    Node currentGoal;
    SceneController scene;
    int score = 0;
    bool facingRight = false;
    float timeSinceLastAttempt = 0f;
    // Start is called before the first frame update
    void Start()
    {
        graph = GameObject.Find("Water").GetComponent<GraphController>();
        scene = GameObject.Find("Main Camera").GetComponent<SceneController>();
        //direction = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0f);
        //direction.Normalize();
        targets = new List<Node>();

            nodes = graph.GetNodes();
            astar = new AStar(nodes);
            int randX = 0;
            int randY = 0;
            do
            {
                randX = Random.Range(0,nodes.GetLength(0));
                randY = Random.Range(0,nodes.GetLength(1));
            }
            while(!nodes[randX,randY].CollisionFree);
            SetPosition(nodes[randX,randY]);

    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 change = direction * Time.deltaTime * speed;
        transform.position += change;*/
        time += Time.deltaTime;
        UpdateTargets();
        Node closestTarget = GetClosestTarget();
        if(path == null && (targets.Count < 1 || currentGoal == null))
        {
            int randX = 0;
            int randY = 0;
            do
            {
                randX = Random.Range(0,nodes.GetLength(0));
                randY = Random.Range(0,nodes.GetLength(1));
            }
            while(!nodes[randX,randY].CollisionFree);
            currentGoal = nodes[randX,randY];
            UpdateRotation(pos.XPos < currentGoal.XPos);
            path = astar.Search(pos,currentGoal);
            transform.position = new Vector3(pos.XPos,pos.YPos,0f);
        }
        else if(closestTarget != null && currentGoal != null && (!targets.Contains(currentGoal) || !currentGoal.Equals(closestTarget) || path == null))
        {

            currentGoal = closestTarget;
             UpdateRotation(pos.XPos < currentGoal.XPos);
            path = astar.Search(pos,currentGoal);
            transform.position = new Vector3(pos.XPos,pos.YPos,0f);
        }

        if(currentTarget != null && !pos.Equals(currentTarget))
        {
            if(WithinRadius(currentTarget,0.1f))
            {
                transform.position = new Vector3(currentTarget.XPos,currentTarget.YPos,0f);
                pos = currentTarget;
            }
            else
            {
                transform.position += direction * Time.deltaTime * speed;
            }
        }
        else if(path != null)
        {

            currentTarget = path[path.Count-1];
            path.RemoveAt(path.Count-1);
            direction = GetDirection(pos,currentTarget);
            time = 0f;
            if(path.Count < 1)
            {
                path = null;
                if(currentGoal!=null)
                {
                    scene.RemoveBread(currentGoal);
                    score++;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        timeSinceLastAttempt += Time.deltaTime;
        if(col.gameObject.name.Contains("Swan") || col.gameObject.name.Contains("Obstacle") || col.gameObject.name.Contains("Ground")
            && timeSinceLastAttempt > 1f)
        {
            int randX = 0;
            int randY = 0;
            do
            {
                randX = Random.Range(-4,4) + pos.XInd;
                randY = Random.Range(-4,4) + pos.YInd;
            }
            while(randX < 0 || randX >= nodes.GetLength(0) || randY < 0 || randY >= nodes.GetLength(1) || !nodes[randX,randY].CollisionFree );
            currentTarget= nodes[randX,randY];
            currentGoal = null;
            path = new List<Node>();
            path.Add(currentTarget);
            
            direction = GetDirection(currentTarget);
            timeSinceLastAttempt = 0f;
            
        }

    }


    public void SetPosition(Node pos)
    {
        transform.position = new Vector3(pos.XPos,pos.YPos,0f);
        this.pos = pos;
    }

    public void SetPath(List<Node> path)
    {
        this.path = path;
    }

    public Node GetPosition()
    {
        return pos;
    }

    public void AddTarget(Node t)
    {
        targets.Add(t);
    }

    Node GetClosestTarget()
    {
        float minDistance = 1000f;
        Node closestFood = null;
        if(targets == null)
            return closestFood;
        foreach(Node t in targets)
        {
            float curDistance = astar.EuclideanDistance(t,pos);
            if(curDistance < minDistance)
            {
                minDistance = curDistance;
                closestFood = t;

            }
        }
        return closestFood;
    }

     Node GetRandomTarget()
    {
        if(targets == null)
            return null;
        
        return targets[Random.Range(0,targets.Count)];
    }

    bool WithinRadius(Node target, float radius)
    {
        Vector3 nextPosition = transform.position + direction * speed * Time.deltaTime;
        float xComponent = (target.XPos - nextPosition.x) * (target.XPos - nextPosition.x);
        float yComponent = (target.YPos - nextPosition.y) * (target.YPos - nextPosition.y);
        return Mathf.Sqrt(xComponent + yComponent) <= radius;

    }

    Vector3 GetDirection(Node from, Node to)
    {
        return (new Vector3(to.XPos - from.XPos, to.YPos - from.YPos, 0f)).normalized;
    }

    Vector3 GetDirection(Node to)
    {
        return (new Vector3(to.XPos - transform.position.x, to.YPos - transform.position.y, 0f)).normalized;
    }

    void UpdateRotation(bool isRight)
    {
        if(isRight && !facingRight)
        {
            transform.Rotate(0f,180f,0f);
            facingRight = true;
        }
        else if(!isRight && facingRight)
        {
            transform.Rotate(0f,180f,0f);
            facingRight = false;
        }
    }

    bool UpdateTargets()
    {
        targets = scene.GetThatBread();
        return targets == null || targets.Count > 0;
    }

}
