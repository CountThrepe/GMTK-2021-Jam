using UnityEngine;
using UnityEngine.U2D;

public class YarnController : MonoBehaviour
{
    public float length = 200f;

    public Transform player;
    public Transform LastStitch;
    public GameObject backPlatformPrefab;
    public GameObject frontPlatformPrefab;
    public SpriteRenderer spool;
    public Sprite[] spoolSprites;
    public Animator animator;

    private SpriteShapeController spriteShape;
    private Spline yarnSpline;

    private GameObject platform = null;

    private bool behind;
    private float lockedLength = 0;

    void Start()
    {
        behind = true;

        spriteShape = GetComponent<SpriteShapeController>();
        yarnSpline = spriteShape.spline;
        yarnSpline.SetPosition(0, LastStitch.position);

        LastStitch.GetComponent<DistanceJoint2D>().distance = length;
        
        // Create back platform
        platform = Instantiate(backPlatformPrefab, GetPlatformCenter(), GetPlatformRotation());
        platform.transform.localScale = GetPlatformScale();
    }

    // Update is called once per frame
    void Update()
    {
        int nPoints = yarnSpline.GetPointCount();
        yarnSpline.SetPosition(nPoints - 1, player.position);

        if(!behind) {
            platform.transform.position = GetPlatformCenter();
            platform.transform.localRotation = GetPlatformRotation();
            platform.transform.localScale = GetPlatformScale();
        }

        UpdateSpool();
    }

    public void Stitch() {
        if(behind && lockedLength + Vector2.Distance(LastStitch.position, player.position) > length) return;
        
        //Tell the Animator we're starting a Stitch
        animator.SetTrigger("makeStitch");

        int index = yarnSpline.GetPointCount() - 1;

        yarnSpline.InsertPointAt(index, player.position + new Vector3(0, 0, 0.1f));
        yarnSpline.SetHeight(index, 0.1f);

        if(behind) {
            // Finish back platform;
            platform.transform.position = GetPlatformCenter();
            platform.transform.localRotation = GetPlatformRotation();
            platform.transform.localScale = GetPlatformScale();

            // Swap active platforms
            GameObject[] backPlatforms = GameObject.FindGameObjectsWithTag("BackPlatform");
            foreach(var bp in backPlatforms) {
                bp.GetComponent<BoxCollider2D>().enabled = false;
            }
            GameObject[] frontPlatforms = GameObject.FindGameObjectsWithTag("FrontPlatform");
            foreach(var fp in frontPlatforms) {
                fp.GetComponent<BoxCollider2D>().enabled = true;
            }

            // Create front platform
            platform = Instantiate(frontPlatformPrefab, GetPlatformCenter(), GetPlatformRotation());
            platform.transform.localScale = GetPlatformScale();
            //Tell Animator we're transitioning front->back
            animator.SetBool("isFrontSide", false);
        } else {
            // Finish front platform
            platform.transform.position = GetPlatformCenter();
            platform.transform.localRotation = GetPlatformRotation();
            platform.transform.localScale = GetPlatformScale();

            // Swap active platforms
            GameObject[] backPlatforms = GameObject.FindGameObjectsWithTag("BackPlatform");
            foreach(var bp in backPlatforms) {
                bp.GetComponent<BoxCollider2D>().enabled = true;
            }
            GameObject[] frontPlatforms = GameObject.FindGameObjectsWithTag("FrontPlatform");
            foreach(var fp in frontPlatforms) {
                fp.GetComponent<BoxCollider2D>().enabled = false;
            }

            // Create back platform
            platform = Instantiate(backPlatformPrefab, GetPlatformCenter(), GetPlatformRotation());
            platform.transform.localScale = GetPlatformScale();

            // Check for tears
            RaycastHit2D[] hits = Physics2D.RaycastAll(LastStitch.position, player.position, Vector2.Distance(LastStitch.position, player.position), LayerMask.GetMask("Tear"));
            foreach(RaycastHit2D hit in hits) {
                if(hit.collider != null) {
                    hit.collider.gameObject.GetComponent<TearHandler>().Close();
                }
            }

            //Tell Animator we're transitioning back->front
            animator.SetBool("isFrontSide", true);
        }

        lockedLength += Vector2.Distance(LastStitch.position, player.position);
        LastStitch.position = player.position;
        LastStitch.GetComponent<DistanceJoint2D>().distance = length - lockedLength;

        behind = !behind;
    }

    private Vector2 GetPlatformCenter() {
        return LastStitch.position + ((player.position - LastStitch.position) / 2);
    }

    private Quaternion GetPlatformRotation() {
        Quaternion q = new Quaternion();
        q.SetFromToRotation(new Vector2(1, 0), player.position - LastStitch.position);
        return q;
    }

    private Vector3 GetPlatformScale() {
        Vector3 scale = platform.transform.localScale;
        scale.x = Vector2.Distance(player.position, LastStitch.position) / 1.9f;
        return scale;
    }

    private void UpdateSpool() {
        float lengthFrac = (lockedLength + Vector2.Distance(LastStitch.position, player.position)) / (length + 10);
        int spoolIndex = (int) Mathf.Floor(lengthFrac * spoolSprites.Length);
        spool.sprite = spoolSprites[spoolIndex];
    }
}
