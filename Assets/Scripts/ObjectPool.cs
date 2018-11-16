using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ObjectPrefabs;

    public GameObject GetObject(string type)
    {
        for (int i = 0; i <ObjectPrefabs.Length; i++)
        {
            if (ObjectPrefabs[i].name == type)
            {
                GameObject newObject = Instantiate(ObjectPrefabs[i]);
                newObject.name = type;
                return newObject;
               
            }
        }
        return null;
    }

}
