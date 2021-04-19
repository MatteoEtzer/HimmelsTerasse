using UnityEngine;
using System.Collections;

/*
 * Swipe Input script for Unity by @fonserbc, free to use wherever
 *
 * Attach to a GameObject, check the static booleans to check if a swipe has been detected this frame
 * Eg: if (SwipeInput.swipedRight) ...
 *
 * 
 */

public class SwipeInput : MonoBehaviour {

    public Transform TreeTarget;
    public Transform CloudTarget;
    public GameObject SearchButton;
    public GameObject ReturnButton;
    public GameObject SearchBar;
    public GameObject OSK;
    public bool SearchButtonClicked = false;
    public bool ReturnButtonClicked = false;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;



    //Overcloud Position Test
    public GameObject OverCloudObject;

    public float dragSpeed;
    private Vector3 dragOrigin;
    private Vector3 mouse;
    private Vector3 mouseLastFrame;
    private float dist;
    public float distBias;

    public Vector3 pos;


    void Start()
    {
        SearchBar.SetActive(false);
        ReturnButton.SetActive(false);
        OSK.SetActive(false);

        Debug.Log(this.gameObject.name);
    }

    void Update()
    {
        mouse = Input.mousePosition;
        dist = Vector3.Distance(mouse, mouseLastFrame);

        //dragging implementation
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        if (dist > distBias)
        {
            pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x, 0, pos.y);
            transform.Translate(move, Space.World);
        }

        mouseLastFrame = Input.mousePosition;

        //end of dragging


        //search fields

        if (SearchButtonClicked == true)
        {
            StartCoroutine("SearchAnimation");
            SearchButtonClicked = false;
        }

        if (ReturnButtonClicked == true)
        {
            StartCoroutine("ReturnAnimation");
            ReturnButtonClicked = false;
        }
        pos = this.transform.position;

    }


    public void Search()
    {
        TreeTarget.position = this.transform.position;
        SearchButtonClicked = true;
        SearchButton.SetActive(false);
        ReturnButton.SetActive(true);
        SearchBar.SetActive(true);
        OSK.SetActive(true);
    }

    IEnumerator SearchAnimation()
    {
        for (float ft = 10f; ft >= 0; ft -= 0.1f)
        {
            Vector3 targetPosition = CloudTarget.TransformPoint(new Vector3(0, 0, 0));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
    }

    IEnumerator ReturnAnimation()
    {
        for (float ft = 10f; ft >= 0; ft -= 0.1f)
        {
            Vector3 targetPosition = TreeTarget.TransformPoint(new Vector3(0, 0, 0));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            yield return null;
        }
    }

    public void ReturnToTree()
    {
        ReturnButtonClicked = true;
        Debug.Log("Return-Button clicked");
        ReturnButton.SetActive(false);
        SearchButton.SetActive(true);
        SearchBar.SetActive(false);
        OSK.SetActive(false);
    }


}
