using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool _Inst;

    [System.Serializable]
    public class Pool
    {
        public string Name;
        public GameObject Prefab;
        public int Size;
        public Transform Parent;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        _Inst = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab, pool.Parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.Name, objectPool);
        }
    }

    public GameObject GetObject(string tName)
    {
        if (!poolDictionary.ContainsKey(tName))
        {
            return null;
        }

        if (poolDictionary[tName].Count == 0)
        {
            // 풀이 비어있을 때 새로운 오브젝트 생성
            GameObject tNewObject = Instantiate(pools.Find(p => p.Name == tName).Prefab, pools.Find(p => p.Name == tName).Parent);
            tNewObject.SetActive(false);
            poolDictionary[tName].Enqueue(tNewObject);
        }

        GameObject tObjectToSpawn = poolDictionary[tName].Dequeue();
        tObjectToSpawn.SetActive(true);

        // poolDictionary[tag].Enqueue(objectToSpawn);

        return tObjectToSpawn;
    }

    public GameObject GetObject(string tName, Vector3 tPosition)
    {
        if (!poolDictionary.ContainsKey(tName))
        {
            return null;
        }

        if (poolDictionary[tName].Count == 0)
        {
            // 풀이 비어있을 때 새로운 오브젝트 생성
            GameObject newObj = Instantiate(pools.Find(p => p.Name == tName).Prefab, pools.Find(p => p.Name == tName).Parent);
            newObj.SetActive(false);
            poolDictionary[tName].Enqueue(newObj);
        }

        GameObject objectToSpawn = poolDictionary[tName].Dequeue();
        objectToSpawn.transform.position = tPosition;
        objectToSpawn.SetActive(true);

        // poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnObject(GameObject tObject)
    {
        tObject.SetActive(false);

        foreach (var tPool in pools)
        {
            if (tPool.Prefab == tObject)
            {
                poolDictionary[tPool.Name].Enqueue(tObject);
                break;
            }
        }
    }
}
