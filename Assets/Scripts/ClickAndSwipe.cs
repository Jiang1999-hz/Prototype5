using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer),typeof(BoxCollider))]//this code will ensure that a TrailRenderer and BoxCollider are on the GameObject the script is attached to
public class ClickAndSwipe : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider col;

    private bool swiping = false;
    // To initialize our variables as they are private
    void Awake()
    {
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        col = GetComponent<BoxCollider>();
        trail.enabled = false;
        col.enabled = false;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            //player pressing the mousebutton
            if (Input.GetMouseButtonDown(0))
            {
                swiping = true;
                UpdateComponents();
            }
            //player release the mousebutton
            else if (Input.GetMouseButtonUp(0))
            {
                swiping = false;
                UpdateComponents();
            }
            //while player pressing and swiping
            if (swiping)
            {
                UpdateMousePosition();
            }
        }
    }

    //set up the GameObject to move with the mouse position
    void UpdateMousePosition()
    {
        //ScreenToWorld will conver the screen position of the mouse to a world position, z = 10.0f because the cam here has the z position -10.0f
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    void UpdateComponents()
    {
        //set the enabled state to whatever the swiping boolean is to ensure while swiping the trail and col exits
        trail.enabled = swiping;
        col.enabled = swiping;
    }

    //when the mouse collide with something check if its a target
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Target>())
        {
            //destroy the target
            collision.gameObject.GetComponent<Target>().DestroyTarget();
        }
    }
}
