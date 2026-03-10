using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObjectPooler _pooler;

    [Header("Values")]
    [SerializeField] private Vector2 _area;
    [SerializeField] private float dummyDuration;
    [SerializeField] private float targetDuration;

    [Header("Dummys")]
    [SerializeField] private string dummyPoolerTag;

    [Header("Targets")]
    [SerializeField] private string targetPoolerTag;
    [SerializeField] private List<GameObject> targetsPositionList;

    [ContextMenu("Start spawn")]
    public void StartSpawn()
    {
        InvokeRepeating(nameof(SetDummys), 0, dummyDuration);
        InvokeRepeating(nameof(SetTragets), 0, targetDuration);
    }

    void SetDummys()
    {
        var pos = SetRandomPos(_area);

        _pooler.SpawnDummyOrTargetFromPool(dummyPoolerTag, pos, new Quaternion(0, -180, 0, 0));
        _pooler.SpawnMaxDummyOrTargetAmountFromPool(dummyPoolerTag, 3, pos, new Quaternion(0, -180, 0, 0));

        var smooke = _pooler.SpawnParticlesFromPool("Puff_Smoke", pos + new Vector3(-0.1f, -0.2f, 0.0f), Quaternion.identity, true) as GameObject;
        smooke.GetComponent<AudioSource>().Play();
    }

    void SetTragets()
    {
        var randomPos = Random.Range(0, targetsPositionList.Count);

        _pooler.SpawnFromPool(targetPoolerTag, targetsPositionList[randomPos].transform.position, targetsPositionList[randomPos].transform.rotation);

        var smooke = _pooler.SpawnParticlesFromPool("Puff_Smoke", targetsPositionList[randomPos].transform.position + new Vector3(-0.3f, 0.2f, 0.0f), Quaternion.identity, true) as GameObject;
        smooke.GetComponent<AudioSource>().Play();
    }

    Vector3 SetRandomPos(Vector2 area)
    {
        var randomPosX = Random.Range(transform.position.x - area.x, transform.position.x + area.x);
        var randomPosZ = Random.Range(transform.position.z -  area.y, transform.position.z + area.y);

        Vector3 randomPos = new (randomPosX, 0.1f, randomPosZ);

        return randomPos;
    }

    [ContextMenu("Stop spawn")]
    public void StopSpawn()
    {
        CancelInvoke(nameof(SetDummys));
        CancelInvoke(nameof(SetTragets));

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
