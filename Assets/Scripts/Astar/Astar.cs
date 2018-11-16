using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Astar {

    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Point, Node>();

        foreach(TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile)); 
        }
    }

    public static Stack<Node> GetPath(Point start, Point goal)
    {

        if(nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();

        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> finalPath = new Stack<Node>();
     
        Node currentNode = nodes[start];

        openList.Add(currentNode);

        while(openList.Count > 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.x - x, currentNode.GridPosition.y - y);

                    if (LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].walkAble && neighbourPos != currentNode.GridPosition)
                    {

                        int gCost = 0;

                        //[14][10][14]
                        //[10][5][10]
                        //[14][10][14]
                        if (Mathf.Abs(x - y) == 1)
                        {
                            gCost = 10;
                        }
                        else
                        {
                            if (!ConnectedDiagonally(currentNode, nodes[neighbourPos]))
                            {
                                continue;
                            }
                            gCost = 14;
                        }
                        Node neighBour = nodes[neighbourPos];




                        if (openList.Contains(neighBour))
                        {
                            if (currentNode.G + gCost < neighBour.G)
                            {
                                neighBour.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }

                      
                        else if (!closedList.Contains(neighBour))
                        {
                            openList.Add(neighBour);
                            neighBour.CalcValues(currentNode, nodes[goal], gCost);
                        }                

                    }


                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0)
            {
                // Soorts the list by F value, and selects the first on the list
                currentNode = openList.OrderBy(n => n.F).First();
            }
            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                    break;
                
            }
        }

        return finalPath;    

        //*****Dit is alleen voor debuggen moet later verwijderd worden
        //GameObject.Find("AStarDebugger").GetComponent<AstarDebugger>().DebugPath(openList,closedList,finalPath);
    }

    private static bool ConnectedDiagonally(Node currentNode, Node neighbor)
    {
        Point direction = neighbor.GridPosition - currentNode.GridPosition;

        Point first = new Point(currentNode.GridPosition.x + direction.x, currentNode.GridPosition.y);

        Point second = new Point(currentNode.GridPosition.x, currentNode.GridPosition.y + direction.y);

        if (LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].walkAble)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].walkAble)
        {
            return false;
        }

        return true;

    }
	
}
