using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Slider = UnityEngine.UI.Slider;

public class GameManager : MonoBehaviour
{
    private PlayerController player;

    [Header("UI Menus")]
    [SerializeField] private GameObject[] menus;

    [Header("TextBoxes")]
    [SerializeField] private TextMeshProUGUI clipText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private TextMeshProUGUI pickupObjText;
    [SerializeField] private TextMeshProUGUI pausedText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    //[SerializeField] private TextMeshProUGUI resourcesTotalText;


    [Header("Sliders")]
    [SerializeField] private Slider healthBar;


    public bool menuOpen = false;
    public bool paused = false;
    public bool gameOver = false;

    public int enemyCount = 0;
    public int startEnemyCount = 0;
    public int enemiesKilled = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        healthBar = GameObject.Find("healthBar").GetComponent<Slider>();

        clipText = GameObject.Find("clipText").GetComponent<TextMeshProUGUI>();
        ammoText = GameObject.Find("ammoText").GetComponent<TextMeshProUGUI>();


        weaponText = GameObject.Find("weaponName").GetComponent<TextMeshProUGUI>();
        pickupObjText = GameObject.Find("PickupObjText").GetComponent<TextMeshProUGUI>();

        pausedText = GameObject.Find("pausedText").GetComponent<TextMeshProUGUI>();

        gameOverText = GameObject.Find("gameOverText").GetComponent<TextMeshProUGUI>();


        startEnemyCount = GameObject.FindGameObjectsWithTag("MeleeEnemy").Length + GameObject.FindGameObjectsWithTag("RangedEnemy").Length;

        enemyCount = startEnemyCount;

        //Clears all the menus off the screen incase some are active.
        for (var i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            healthBar.value = player.health;

            if (player.currentMeleeWeapon)
            {

                clipText.text = " ";
                weaponText.text = player.currentMeleeWeapon.weaponName;
            }
            if (player.currentRangedWeapon)
            {
                clipText.text = "Clip: " + player.currentRangedWeapon.clip + "/" + player.currentRangedWeapon.clipSize;
                ammoText.text = "Ammo: " + player.currentRangedWeapon.ammo + "/" + player.currentRangedWeapon.maxAmmo;

                weaponText.text = player.currentRangedWeapon.weaponName;
            }
            else
            {
                weaponText.text = " ";
                clipText.text = " ";
                ammoText.text = " ";
            }

            if (player.pickupObj != null)
            {
                pickupObjText.text = player.pickupObj.name;
            }
            else
                pickupObjText.text = " ";
        }
        if (paused)
        {
            pausedText.text = "Paused";
        }
        else
            pausedText.text = " ";

        if (gameOver)
        {
            gameOverText.text = "GAME OVER";
        }
        else
            gameOverText.text = " ";

        if (player.health <= 0)
        {
            gameOver = true;
            Time.timeScale = 0;
        }
        enemiesKilled = startEnemyCount - enemyCount;
    }


    public void Pause()
    {
        if (!gameOver)
        {
            paused = !paused;

            if (paused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void LoadLevel(int levelID)
    {
        if (levelID > SceneManager.sceneCountInBuildSettings)
            Debug.Log("Level ID is too high: " + levelID);
        else
            SceneManager.LoadScene(levelID);
    }

    public void LoadNextLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        LoadLevel(0);
    }

    public void OpenMenu(int menuOpening)
    {
        menuOpen = !menuOpen;

        menus[menuOpening].SetActive(menuOpen);
    }

    public void Quit()
    {
        Application.Quit();
    }
}