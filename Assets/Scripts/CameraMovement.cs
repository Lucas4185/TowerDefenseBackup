using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {


    [SerializeField]
    private float cameraSpeed = 0;
    private float xMax;
    private float yMin;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

        GetInput();

    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * cameraSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * cameraSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * cameraSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }

       transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, xMax), Mathf.Clamp(transform.position.y, yMin,0),-10);
    }

    public void SetLimits(Vector3 maxTiles)
    {
        Vector3 wp = Camera.main.ViewportToWorldPoint(new Vector3(1, 0));

        xMax = maxTiles.x - wp.x;
        yMin = maxTiles.y - wp.y;

    }
}
