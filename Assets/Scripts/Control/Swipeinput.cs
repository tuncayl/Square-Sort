using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Swipeinput : MonoBehaviour
{
    private Touch touch;


    //Touch Control
    public static bool NextSwipe = false;


    [SerializeField] private const float dragLength = 70;

    //TOUCH POSİTİONS
    Vector2 startpos = Vector2.zero;
    Vector2 endpos = Vector2.zero;

    //Direction cal
    Vector3[] Directions = { Vector3.right, Vector3.back, Vector3.left, Vector3.forward };
    Vector3 currentdireciton = Vector3.zero;

    //Event
    public UnityEvent<Vector3> SwipeEvent { get { if (swipe == null) swipe = new UnityEvent<Vector3>(); return swipe; } }
    private UnityEvent<Vector3> swipe;

    //static variable
    public static short dir;
    public int CubecountProperty
    {
        get => cubeCount;
        set
        {
            cubeCount = value;
            if (value == 0)
            {
                int count = Gamemanager.Instance.CubesRigidbody.Count;
                Gamemanager.Instance.cameraRotate(Vector3.zero);
                Debug.Log(count);
                if (count == 0) Gamemanager.Instance.LevelClear();
                else if (SwipeController._SwipeCount == 0) Gamemanager.Instance.UImanager.MenuTranstation((int)Menu.defeat);
                else NextSwipe = true;
            }

        }
    }
    public static int cubeCount = 30;


    private void Update()
    {
        SwipeDetect();

    }
    private void SwipeDetect()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startpos = touch.position;
                endpos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                endpos = touch.position;
            }
            Vector2 dir = (endpos - startpos).normalized;
            if ((endpos - startpos).magnitude > dragLength)
            {
                if (!NextSwipe) return;
                //vector convert to angle
                float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
                //set default touch position
                startpos = endpos;
                //invoke Event
                swipe?.Invoke(AngleToDirection(angle));
            }

        }
    }
    Vector3 AngleToDirection(float angle)
    {
        Vector3 direction = Directions[3];
        dir = 3;
        //default swipe up
        if (angle > 60 && angle < 120)
        {
            direction = Directions[0];
            dir = 0;
            //right swipe
        }
        else if (angle > 150 || angle < -150)
        {
            direction = Directions[1];
            dir = 2;
            //down swipe
        }
        else if (angle < -60 && angle > -120)
        {
            direction = Directions[2];
            dir = 1;
            //left swipe
        }
        return direction;
    }

}
