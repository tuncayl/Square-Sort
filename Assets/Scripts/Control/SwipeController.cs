using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class SwipeController : MonoBehaviour
{

    CommandStack _commandStack = new CommandStack();

    public Swipeinput _Swipeinput;

    [SerializeField] float _SwipeForce;
    [SerializeField] TextMeshProUGUI SwipeCountText;

    public static int _SwipeCount = default;

    public int _SwipeCountProperty
    {
        get => _SwipeCount;
        set
        {
            _SwipeCount = value;
            if (_SwipeCount == 0) Debug.Log("Count 0000000000000");
            SwipeCountText.text = _SwipeCount.ToString();
        }
    }

    public static Vector3 oldvector = Vector3.zero;












    private void DirectionCallback(Vector3 dir)
    {

        if (oldvector == dir) return;
        if (_SwipeCountProperty == 0) return;
        else _SwipeCountProperty--;
        Swipeinput.NextSwipe = false;
        _commandStack.ExecuteCommand(new SwipeCubes(dir, _SwipeForce));
        oldvector = dir;

    }
    private void OnEnable()
    {
        _Swipeinput.enabled = true;
        _Swipeinput.SwipeEvent.AddListener(DirectionCallback);
        Swipeinput.NextSwipe = true;
    }
    private void OnDisable()
    {
        _Swipeinput.SwipeEvent.RemoveListener(DirectionCallback);
        _Swipeinput.enabled = false;
        Swipeinput.NextSwipe = false;
    }
}
