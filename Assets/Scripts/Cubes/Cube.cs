using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Cube : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public BoxCollider _Collider;

    // [SerializeField] bool overlap = false;
    public Swipeinput swipe;
    [SerializeField] LayerMask layer;

    [SerializeField] Cube node;
    [SerializeField] GameObject particle;

    Vector3 direction;

    private void Awake()
    {
        swipe = Gamemanager.Instance.Swipe._Swipeinput;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (rb.isKinematic) return;
        swipe.CubecountProperty -= 1;
        rb.isKinematic = true;
        if (other.gameObject.layer >= 12) SetPosition(other.transform.position);
        else SetPosition(other.transform.position, 0);
        if (node == null) return;
        node.Transfer(transform.position);

    }
    private void SetPosition(Vector3 pos, float value = 0.05f, float interval = 0.52f)
    {

        if (Mathf.Abs(direction.x) > 0)
        {
            pos = new Vector3(pos.x + (value * direction.x), 0.7382904f, transform.position.z);

        }
        else
        {
            pos = new Vector3(transform.position.x, 0.7382904f, pos.z + (value * direction.z));
        }

        transform.position = pos + (direction * interval);
    }
    private void OnTriggerEnter(Collider other)
    {

        Gamemanager.Instance.CubesRigidbody.Remove(this.rb);
        swipe.CubecountProperty -= 1;
        ////////////////////////////////////
        particle.transform.SetParent(null);
        particle.SetActive(true);
        gameObject.SetActive(false);
        /////////////////////////////////
        Destroy(this.gameObject, 0.5f);
    }


    public void CreateNode(Vector3 dir)
    {
        Collider[] hitcollider = Physics.OverlapBox(transform.position + (dir * 0.52f),
       new Vector3((transform.localScale.x / 4f), transform.localScale.y / 4f,
       transform.localScale.z / 4f), Quaternion.identity, layer);
        node = hitcollider.Length > 0.1 ? hitcollider[0].GetComponent<Cube>() : null;
        direction = dir;

    }
    public void Transfer(Vector3 pos)
    {
        swipe.CubecountProperty -= 1;
        rb.isKinematic = true;
        transform.position = pos + (direction * 0.515f);
        if (node != null) node.Transfer(transform.position);
    }
}
