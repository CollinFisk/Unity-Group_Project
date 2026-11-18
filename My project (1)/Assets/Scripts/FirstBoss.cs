using UnityEngine;

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
    [SerializeField] private GameObject[] Tvs;

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
        if(!TvAttackEnabled && !summoningEnemies && !TossingJunk)
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
                TvAttackStart();
            }
        }
        if (chooseAttack == 2)
        {
            if (!summoningEnemies)
            {
                SummonEnemiesStart();
            }
        }
        if (chooseAttack == 3)
        {
            if (!TossingJunk)
            {
                JunkAttackStart();
            }
        }
    }

    #region Attacks
    private void TvAttackStart()
    {
        TvAttackEnabled = true;
        Invoke(nameof(TvAttack), attackCooldown);
    }    
    private void TvAttack()
    {
        Invoke(nameof(TvAttackStop), TvEnabledTime);
    }    
    private void TvAttackStop()
    {
        TvAttackEnabled = false;
    }  
    

    private void SummonEnemiesStart()
    {
        summoningEnemies = true;
        Invoke(nameof(SummonEnemies), attackCooldown);
    }
    private void SummonEnemies()
    {
        Invoke(nameof(SummonEnemiesStop), summonTime);
    }
    private void SummonEnemiesStop()
    {
        summoningEnemies = false;
    }


    private void JunkAttackStart()
    {
        TossingJunk = true;
        Invoke(nameof(JunkAttack), attackCooldown);
    }
    private void JunkAttack()
    {
        Invoke(nameof(JunkAttackStop), tossingTime);
    }
    private void JunkAttackStop()
    {
        TossingJunk = false;
    }
    #endregion
}
