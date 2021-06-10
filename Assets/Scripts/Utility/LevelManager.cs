using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject player, ui, cam, enemies;
    private static HeartManager uiHearts;
    private static FadeManager uiFade;
    private static PromptManager uiPrompt;
    private static PauseManager uiPause;

    public int maxHearts = 3;
    public float fadeWait = 0.5f;
    public float fadeOutDuration = 1;
    public float fadeInDuration = 2;
    public Vector3 cameraOffset;
    private static Vector3 respawnPoint;
    private static float respawnStart;
    private bool fading;
    private bool respawned;
    private bool started;
    private float respawnDuration;

    
    private static LevelManager self;
    
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<LevelManager>();

        // playerInput = player.GetComponent<PlayerInputController>();
        // playerMovement = player.GetComponent<PlayerCharacterController>();
        // playerCombat = player.GetComponent<PlayerCombatController>();

        // uiHearts = ui.GetComponent<HeartManager>();
        // uiHearts.maxHearts = maxHearts;
        // uiHearts.currHearts = maxHearts;
        // uiFade = ui.GetComponent<FadeManager>();
        // uiPrompt = ui.GetComponent<PromptManager>();
        // uiPause = ui.GetComponent<PauseManager>();

        respawnPoint = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.localScale.x);
        respawnStart = -1;
        respawned = false;

        respawnDuration = fadeWait + fadeOutDuration + fadeInDuration;
        fading = false;

        started = false;
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
    // }

    

    // public static LevelManager GetInstance()
    // {
    //     return self;
    // }

    // private void Respawn()
    // {
    //     respawned = true;
    //     Vector3 pos = new Vector3(respawnPoint.x, respawnPoint.y, 0);
    //     cam.transform.position = pos + cameraOffset;
    //     // playerMovement.Reset(pos, new Vector3(respawnPoint.z, 1, 1));
    //     // playerInput.SetFrozen(false);
    //     // playerCombat.FullHeal();
        
    //     // ActivateEnemies(enemies);
    //     uiFade.FadeIn(fadeInDuration);
    }

    // private void ActivateEnemies(GameObject obj)
    // {
    //     EnemyCombatController c = obj.GetComponent<EnemyCombatController>();
    //     if(c != null)
    //     {
    //         if(obj.activeSelf) c.Kill();
    //         obj.SetActive(true);
    //     }
    //     else
    //     {
    //         Transform[] ts = obj.GetComponentsInChildren<Transform>(true);
    //         foreach(Transform t in ts)
    //         {
    //             if(t.gameObject.name == obj.name) continue;
    //             ActivateEnemies(t.gameObject);
    //         }
    //     }
    // }

    public void StartRespawn()
    {
        respawnStart = Time.time;
    }

    public void SetRespawnPoint(Vector2 pos, float flip)
    {
        respawnPoint = new Vector3(pos.x, pos.y, flip);
    }

    public void UpdateLivesUI(int lives)
    {
        uiHearts.currHearts = lives;
    }

    public void ShowPrompt(string prompt, float time)
    {
        uiPrompt.ShowPrompt(prompt, time);
    }

    public void TransitionScene()
    {
        uiFade.FadeOut(3, true);
    }

    public void TogglePause()
    {
        uiPause.TogglePause();
    }

    public bool IsPaused()
    {
        return uiPause.IsPaused();
    }
}
