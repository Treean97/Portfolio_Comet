using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour
{
    [SerializeField]
    GameObject _Item;

    [SerializeField]
    GameObject _SelectItem;

    [SerializeField]
    ItemDictionary _ItemDictionary;

    [SerializeField]
    float _SelectItemSpawnChance;

    [SerializeField]
    AudioSource _AudioSource;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnItem(GameObject tItemSpawner)
    {
        _AudioSource.Play();

        Vector3 tSpawnPos = tItemSpawner.GetComponent<ItemSpawner>().GetItemSpawnPos().position;

        if(_SelectItemSpawnChance <= Random.Range(0f,100f))
        {
            GameObject tSelectItem = Instantiate<GameObject>(_SelectItem, tSpawnPos, Quaternion.identity);
        }
        else
        {
            GameObject tItem = Instantiate<GameObject>(_Item, tSpawnPos, Quaternion.identity);
            tItem.GetComponent<Items>()._ItemStatus = _ItemDictionary.GetTotalItemStatus[Random.Range(0, _ItemDictionary.GetTotalItemStatus.Length)];
        }
        
    }
}
