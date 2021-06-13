using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int tears;
    public GameObject player; //, ui, cam, enemies;
    // private static FadeManager uiFade;
    // private static PromptManager uiPrompt;
    public PauseManager uiPause;
    
    // public float fadeWait = 0.5f;
    // public float fadeOutDuration = 1;
    // public float fadeInDuration = 2;
    // public Vector3 cameraOffset;
    // private static Vector3 respawnPoint;
    // private static float respawnStart;
    // private bool fading;
    // private bool respawned;
    // private bool started;
    // private float respawnDuration;

    private int closedTears;

    private PlayerMovement playerMovement;

    private static LevelManager self;
    
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<LevelManager>();

        closedTears = 0;
        tears = GameObject.FindGameObjectsWithTag("Tear").Length;

        // playerInput = player.GetComponent<PlayerInputController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        // playerCombat = player.GetComponent<PlayerCombatController>();

        // uiHearts = ui.GetComponent<HeartManager>();
        // uiHearts.maxHearts = maxHearts;
        // uiHearts.currHearts = maxHearts;
        // uiFade = ui.GetComponent<FadeManager>();
        // uiPrompt = ui.GetComponent<PromptManager>();
        // uiPause = ui.GetComponent<PauseManager>();

        // respawnPoint = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.localScale.x);
        // respawnStart = -1;
        // respawned = false;

        // respawnDuration = fadeWait + fadeOutDuration + fadeInDuration;
        // fading = false;

        // started = false;
    }

    // Update is called once per frame
    void Update()
    {
    //     if(!started)
    //     {
    //         uiFade.FadeIn(1);
    //         MusicManager.GetInstance().TurnUpBackground(1);
    //         uiPrompt.ShowPrompt("A to Move Left", 8);
    //         started = true;
    //     }

    //     if(respawnStart != -1)
    //     {
    //         if(Time.time < respawnStart + respawnDuration)
    //         {
    //             if(!fading && Time.time > respawnStart + fadeWait)
    //             {
    //                 fading = true;
    //                 uiFade.FadeOut(fadeOutDuration);
    //             }
    //             if(!respawned && Time.time > respawnStart + fadeWait + fadeOutDuration) Respawn();
    //         }
    //         else 
    //         {
    //             fading = false;
    //             respawned = false;
    //             respawnStart = -1;
    //         }
    //     }
    }

    public static LevelManager GetInstance()
    {
        return self;
    }

    public void TearClosed() {
        closedTears++;
        if(closedTears == tears) {
            FinishLevel();
        }
    }

    private void FinishLevel() {
        playerMovement.Celebrate();
        Invoke("NextScene", 2);
    }

    private void NextScene() {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount);
    }

    // public void ShowPrompt(string prompt, float time)
    // {
    //     uiPrompt.ShowPrompt(prompt, time);
    // }

    // public void TransitionScene()
    // {
    //     uiFade.FadeOut(3, true);
    // }

    public void TogglePause()
    {
        uiPause.TogglePause();
    }

    // public bool IsPaused()
    // {
    //     return uiPause.IsPaused();
    // }
}
