using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : BaseBoardPiece
{
    [SerializeField] private int enemySpawnTime = 5;
    [SerializeField] private int maxBaseEnemies= 10;

    [SerializeField] private Transform enemySpawnPoint;

    [SerializeField] private Enemy enemyClass;
    private List<Enemy> enemies = new List<Enemy>();

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
       // StartCoroutine(SpawnEnemy());
    }

    //private IEnumerator SpawnEnemy()
    //{
    //    while (enemies.Count < maxBaseEnemies)
    //    {
    //        if (BaseManager.instance.PlayerBases.Count > 0)
    //        {
    //            Enemy enemy = Instantiate(enemyClass, enemySpawnPoint.position, enemySpawnPoint.rotation);
    //            enemies.Add(enemy);
    //            enemy.gameObject.name = ("Enemy: " + enemies.Count);
    //        }
    //        yield return new WaitForSeconds(enemySpawnTime);
    //    }
    //}
}
