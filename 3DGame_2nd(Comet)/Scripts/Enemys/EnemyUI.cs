using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField]
    GameObject _Player;

    // Start is called before the first frame update
    void Start()
    {
        _Player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.forward = _Player.transform.forward;
    }
}
