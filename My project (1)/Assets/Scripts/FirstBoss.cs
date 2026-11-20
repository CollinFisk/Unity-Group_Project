using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class FirstBoss : MonoBehaviour
{
    //TV HEAD BOSS

    [Header("Stats")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private float attackCooldown;

    [Space]
    [Header("Attacks")]

    [Space]
    [SerializeField] private bool TvAttackEnabled = false;
    [SerializeField] private float TvEnabledTime;
    [SerializeField] private int SpawnedLasersCount;
    [SerializeField] private GameObject[] Tvs;
    [SerializeField] private GameObject laser;

    [Space]
    [SerializeField] private bool summoningEnemies = false;
    [SerializeField] private float summonTime;
    [SerializeField] private int summonCount;
    [SerializeField] private int summonedEnemiesTotal;
    [SerializeField] private GameObject summonedEnemy;

    [Space]
    [SerializeField] private bool TossingJunk = false;
    [SerializeField] private float tossingTime;
    [SerializeField] private int tossJunkCount;
    [SerializeField] private GameObject junkGameObject;
    [SerializeField] private GameObject junkTarget;

    [Space]
    [Header("Meta")]
    [SerializeField] private bool canAttack;
    private bool secondPhase;
    public bool isSecondPhase;
    public bool bossDefeated;

    [Header("Phase 2 Values")]
    [SerializeField] private float TvEnabledTimePhase2;
    [SerializeField] private int summonCountPhase2;
    [SerializeField] private float tossingTimePhase2;
    [SerializeField] private float tossJunkCountPhase2;

    void Update()
    {
        if (health <= 0)
        {
            Destroy(transform.gameObject);
        }
        //Check if Can Attack, Check What Phase, Choose Attack, Attack

        //Check if Can Attack
        if (!TvAttackEnabled && !summoningEnemies && !TossingJunk)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }

        //Check What Phase
        if (health * 4 <= maxHealth * 2)
        {
            secondPhase = true;
        }

        if (secondPhase && !isSecondPhase && canAttack)
        {
            SecondPhase();
        }

        //Choose Attack
        if (canAttack)
        {
            ChooseAttack();
        }

    }
    private void SecondPhase()
    {
        isSecondPhase = true;
    }
    private void ChooseAttack()
    {
        int chooseAttack = Random.Range(1, 4);

        if (chooseAttack == 1)
        {
            if (!TvAttackEnabled)
            {
                StartCoroutine(nameof(TvAttack));
            }
        }
        if (chooseAttack == 2)
        {
            if (!summoningEnemies)
            {
                StartCoroutine(nameof(SummonEnemies));
            }
        }
        if (chooseAttack == 3)
        {
            if (!TossingJunk)
            {
                StartCoroutine(nameof(JunkAttack));
            }
        }
    }

    #region Attacks

    IEnumerator TvAttack()
    {
        TvAttackEnabled = true;
        yield return new WaitForSeconds(attackCooldown);
        GameObject[] spawnedLasers = new GameObject[SpawnedLasersCount];

        float[] spawnPositions = { -1.4f, 0f, 1.4f };
        Vector3 spawnpos;
        spawnpos.x = transform.position.x + spawnPositions[Random.Range(0, spawnPositions.Length)];
        spawnpos.y = -3.6f;
        spawnpos.z = 0f;

        for (int i = 0; i < SpawnedLasersCount; i++)
        {
            spawnedLasers[i] = GameObject.Instantiate(laser, spawnpos, transform.rotation);
        }
        yield return new WaitForSeconds(TvEnabledTime);
        for (int i = 0; i < SpawnedLasersCount; i++)
        {
            if (spawnedLasers[i] != null)
            {
                Destroy(spawnedLasers[i]);
            }
        }
        TvAttackEnabled = false;
    }

    IEnumerator SummonEnemies()
    {
        summoningEnemies = true;
        yield return new WaitForSeconds(attackCooldown);
        GameObject.Instantiate(summonedEnemy, transform.position, transform.rotation);
        yield return new WaitForSeconds(summonTime);
        summoningEnemies = false;
    }

    IEnumerator JunkAttack()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        summoningEnemies = true;
        float choosePositionX = player.position.x + Random.Range(-2f, 2f);
        float choosePositionY = player.position.y + Random.Range(-2f, 2f);
        Vector3 chosenPosition;
        chosenPosition.x = choosePositionX;
        chosenPosition.y = choosePositionY;
        chosenPosition.z = 0;

        yield return new WaitForSeconds(attackCooldown);
        GameObject.Instantiate(junkTarget, player.position, transform.rotation);
        yield return new WaitForSeconds(attackCooldown);
        GameObject.Instantiate(junkGameObject, junkTarget.transform.position, transform.rotation);
        yield return new WaitForSeconds(summonTime);
        summoningEnemies = false;
    }
    #endregion
}
