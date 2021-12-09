using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Swipes { None, Up, Down, Left, TopLeft, BottomLeft, Right, TopRight, BottomRight };

public class CharacterController : MonoBehaviour
{
    public float minSwipeLength = 200f;
    Vector2 currentSwipe;

    private Vector2 fingerStart;
    private Vector2 fingerEnd;

    public static Swipes direction;


   [SerializeField] bool canSwipe = true;
    [SerializeField] bool findMovePos = false;
    [SerializeField] Vector3 startPos, finalPos;
    [SerializeField] float percentMove;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] LayerMask ignorelayer;

    public bool rotate;
    public Vector3 targetRotation;
    Rigidbody rb;

    GameManager gm;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation.eulerAngles;
        gm = GameManager.instance;
    }
    void Update() 
    {
       
        SwipeDetection();
        if (!canSwipe && targetRotation==transform.rotation.eulerAngles)
        {

            if (!findMovePos)
            {
                findMovePos = true;
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100f, ignorelayer))
                {
                    //Debug.Log(hit.transform.name);
                    Vector3 direction = (transform.position - hit.transform.position).normalized;
                    //Debug.Log(direction);
                    //Debug.Log(hit.transform.position + direction); //new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z)));
                    finalPos = hit.transform.position + direction;
                    startPos = transform.position;
                    //Debug.Log(Vector3.Distance(finalPos, startPos));
                }
            }

            if(Vector3.Distance(finalPos, startPos) > 0.5f)
            {
                percentMove += Time.deltaTime * moveSpeed;

                transform.position = Vector3.MoveTowards(startPos, finalPos, percentMove);
                //if (percentMove > 1f)
                if(Vector3.Distance(transform.position,finalPos)<0.05f)
                {
                    percentMove = 0;
                    findMovePos = false;
                    canSwipe = true;
                    transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), Mathf.RoundToInt(transform.position.z));
                }
            }
            else
            {
                percentMove = 0;
                findMovePos = false;
                canSwipe = true;
            }
       
        }
    }
    public void SwipeDetection()
    {
        if (!gm.canMove)
        {
            return;
        }
        if (!canSwipe)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            fingerStart = Input.mousePosition;
            fingerEnd = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            fingerEnd = Input.mousePosition;

            currentSwipe = new Vector2(fingerEnd.x - fingerStart.x, fingerEnd.y - fingerStart.y);

            // Make sure it was a legit swipe, not a tap
            if (currentSwipe.magnitude < minSwipeLength)
            {
                direction = Swipes.None;
                return;
            }
            float angle = (Mathf.Atan2(currentSwipe.y, currentSwipe.x) / (Mathf.PI));
            //Debug.Log(angle);
            // Swipe up

            //if (angle > 0.375f && angle < 0.625f)
            //{
            //    direction = Swipes.Up;
            //    Debug.Log("Up");
            //    targetRotation.y = 0;
            //}
            //else if (angle < -0.375f && angle > -0.625f)
            //{
            //    direction = Swipes.Down;
            //    Debug.Log("Down");
            //    targetRotation.y = 180;

            //}
            //else if (angle < -0.875f || angle > 0.875f)
            //{
            //    direction = Swipes.Left;
            //    Debug.Log("Left");
            //    targetRotation.y = 90;
            //}
            //else if (angle > -0.125f && angle < 0.125f)
            //{
            //    direction = Swipes.Right;
            //    Debug.Log("Right");
            //    targetRotation.y = -90;
            //}
            //else
            if (angle > 0.125f && angle < 0.375f)
            {
                direction = Swipes.TopRight;
                //Debug.Log("top right");
                targetRotation.y = 90;

            }
            else if (angle > 0.625f && angle < 0.875f)
            {
                direction = Swipes.TopLeft;
                //Debug.Log("top left");
                targetRotation.y = 0;
            }
            else if (angle < -0.125f && angle > -0.375f)
            {
                direction = Swipes.BottomRight;
                //Debug.Log("bottom right");
                targetRotation.y = 180;
            }
            else if (angle < -0.625f && angle > -0.875f)
            {
                direction = Swipes.BottomLeft;
                //Debug.Log("bottom left");
                targetRotation.y = 270;
            }
            //Debug.Break();
            //transform.rotation = Quaternion.identity;
          
            transform.localRotation = Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z  );
            //rb.rotation.SetLookRotation (targetRotation;
            //transform.rotation.SetEulerAngles(targetRotation);

            canSwipe = false;

        }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    direction = Swipes.None;
        //}
    }
}
