using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletStatus", menuName = "Scriptable Object/BulletStatus", order = int.MaxValue - 2)]
public class BulletStatus : ScriptableObject
{
    [SerializeField]
    float BulletId = 0;
    public float GetBulletId { get { return BulletId; } }


    [SerializeField]
    float BulletPower = 0;
    public float GetBulletPower { get { return BulletPower; } }

}
