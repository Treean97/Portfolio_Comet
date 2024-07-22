using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Orb : MonoBehaviour
{
    [SerializeField]
    float OrbSpeed;

    [SerializeField]
    float OrbPower;

    [SerializeField]
    Vector3 _TargetPosition;

    public enum State
    {
        Stop,
        Move
    }

    public State _State;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_State == State.Move)
        {
            transform.Translate(_TargetPosition.normalized * OrbSpeed * Time.deltaTime);
        }
        
    }

    public void SetTargetPosition(Vector3 tTargetPosition)
    {
        _TargetPosition = tTargetPosition;
        _State = State.Move;
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("Player"))
    //    {
    //        collision.gameObject.GetComponent<Player>().Damaged(OrbPower);
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().Damaged(OrbPower);
            
        }

        Destroy(this.gameObject);
    }
}
