using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Unity.VisualScripting;

public class ObjectPooler : MonoBehaviour
{
    public List<Pool> pools;
    public Dictionary <string, Queue<GameObject>> poolDictionary;

    private Queue<GameObject> objectPool;
    private int amountIndex = 0;

    void Start()
    {
        poolDictionary = new Dictionary <string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj;

                if (pool.haveParent)
                    obj = Instantiate(pool.prefab, pool.parent);

                else
                    obj = Instantiate(pool.prefab);

                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't excist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnParticlesFromPool(string tag, Vector3 position, Quaternion rotation, bool playOnSpawn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't excist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        ParticleSystem particles = objectToSpawn.GetComponentInChildren(typeof(ParticleSystem)) as ParticleSystem;

        if (playOnSpawn)
            particles.Play();

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnDummyOrTargetFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't excist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.GetComponent<HealthManager>().Invoke("ResetCurrentHealth", 0);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnMaxDummyOrTargetAmountFromPool(string tag, int MaxAmount, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't excist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.GetComponent<HealthManager>().Invoke("ResetCurrentHealth", 0);

        poolDictionary[tag].Enqueue(objectToSpawn);

        amountIndex++;

        if (amountIndex > 0 && amountIndex <= MaxAmount)
            return objectToSpawn;
        else
        {
            amountIndex = 0;
            return null;
        }
    }
}

[System.Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
    [Space(10)]

    public bool haveParent;
    [ConditionalField(nameof(haveParent), false, true)] public Transform parent;
}
