using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] List<Enemy> enemyList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        enemyList = new List<Enemy>();
    }
    public int GetEnemyCount() => enemyList.Count;
    public void ClearEnemiesList() => enemyList.Clear();
    public void AddEnemy(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
