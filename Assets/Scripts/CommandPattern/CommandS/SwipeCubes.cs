using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;




public class SwipeCubes : ICommand
{
    Vector3 direction;
    float _SwipeForce;
    public SwipeCubes(Vector3 dir, float force)
    {
        direction = dir;
        _SwipeForce = force;
    }

    public void Execute()
    {
        
        Swipeinput.cubeCount = Gamemanager.Instance.CubesRigidbody.Count;

        WallsSetCollider();
        RigidbodyConstraints newconts = Mathf.Abs(direction.x) > 0.1f ? RigidbodyConstraints.FreezePositionZ : RigidbodyConstraints.FreezePositionX;

        foreach (Rigidbody i in Gamemanager.Instance.CubesRigidbody)
        {
            i.constraints = newconts | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
            i.isKinematic = false;
            i.GetComponent<Cube>().CreateNode(direction * -1);
        }
        foreach (Rigidbody i in Gamemanager.Instance.CubesRigidbody)
        {
            i.AddForce((direction) * _SwipeForce, ForceMode.VelocityChange);
        }
        Gamemanager.Instance.cameraRotate(direction);
    }
    private void WallsSetCollider()
    {
        foreach (BoxCollider i in Gamemanager.Instance.Walls) i.enabled = false;
        for (int i = 0; i < 4; i++)
        {
            Gamemanager.Instance.Walls[Swipeinput.dir].enabled = true;
        }
    }
    public void Undo()
    {
        Debug.Log("wowow");
    }
}
