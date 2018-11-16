using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Stack<Node> path;

    private Point GridPostition { get; set; }

    private Vector3 destination;

    private bool IsActive { get; set; }


    private void Update()
    {
        Move();
    }
    public void Spawn()
    {
        transform.position = LevelManager.Instance.BeginPortal.transform.position;

        SetPath(LevelManager.Instance.Path);
    }

    public void Move()
    {

     
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (transform.position == destination)
            {
                if (path != null & path.Count > 0)
                {
                    GridPostition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }
            }
        
    }

    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.path = newPath;

            GridPostition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }
}
