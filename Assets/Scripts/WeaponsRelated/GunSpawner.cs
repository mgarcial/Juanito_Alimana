using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    [SerializeField] string[] weaponTags; 
    [SerializeField] float spawnInterval = 5f;
    public Transform[] spawnPoints;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnWeapon();
            timer = 0f;
        }
    }

    void SpawnWeapon()
    {
        int randomWeaponIndex = Random.Range(0, weaponTags.Length);
        string randomWeaponTag = weaponTags[randomWeaponIndex];

        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomSpawnPointIndex];

        PoolManager.instance.SpawnFromPool(randomWeaponTag, spawnPoint.position, spawnPoint.rotation);
    }
}
