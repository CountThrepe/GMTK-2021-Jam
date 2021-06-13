using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.Audio;

using System.Collections.Generic;

public class YarnController : MonoBehaviour
{
    public float length = 200f;
    public float tolerance = 0.1f;
    public float unstitchTolerance = 2;

    public Transform player;
    public Transform LastStitch;
    public GameObject backPlatformPrefab;
    public GameObject frontPlatformPrefab;
    public SpriteRenderer spool;
    public Sprite[] spoolSprites;
    public ThreadDisplay threadDisplay;

    public Animator animator;
    private AudioSource audioSrc;

    private SpriteShapeController spriteShape;
    private Spline yarnSpline;

    private GameObject platform = null;

    private bool behind;
    private float lockedLength = 0;
    private List<GameObject> platforms = new List<GameObject>();

    void Start()
    {
        behind = true;

        spriteShape = GetComponent<SpriteShapeController>();
        yarnSpline = spriteShape.spline;
        yarnSpline.SetPosition(0, LastStitch.position);

        LastStitch.GetComponent<DistanceJoint2D>().distance = length;

        threadDisplay.SetTotalThread(length);
        
        // Create back platform
        platform = Instantiate(backPlatformPrefab, GetPlatformCenter(), GetPlatformRotation());
        platform.transform.localScale = GetPlatformScale();

        //Load Audio
        audioSrc = GetComponent<AudioSource>();
        //stitchSound = audio.Effects.Load<AudioClip>("stitch");
    }

    // Update is called once per frame
    void Update()
    {
        int nPoints = yarnSpline.GetPointCount();
        yarnSpline.SetPosition(nPoints - 1, player.position);

        if(platform != null) {
            platform.transform.position = GetPlatformCenter();
            platform.transform.localRotation = GetPlatformRotation();
            platform.transform.localScale = GetPlatformScale();
        }

        // Update UI Stuff
        UpdateSpool();
        threadDisplay.SetCurrentThread(lockedLength + Vector2.Distance(LastStitch.position, player.position));
    }

    public void Stitch() {
        if(platform == null) return;
        bool newPlatform = lockedLength + Vector2.Distance(LastStitch.position, player.position) < length;
        
        //Tell the Animator we're starting a Stitch
        animator.SetTrigger("makeStitch");
        //Play Stitch Sound
        audioSrc.Play();

        int index = yarnSpline.GetPointCount() - 1;

        yarnSpline.InsertPointAt(index, player.position + new Vector3(0, 0, 0.1f));
        yarnSpline.SetHeight(index, 0.1f);

        // Finish current platform
        platform.transform.position = GetPlatformCenter();
        platform.transform.localRotation = GetPlatformRotation();
        platform.transform.localScale = GetPlatformScale();
        platforms.Add(platform);

        if(behind) {
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
            if(newPlatform) {
                platform = Instantiate(frontPlatformPrefab, GetPlatformCenter(), GetPlatformRotation());
                platform.transform.localScale = GetPlatformScale();
            } else platform = null;

            //Tell Animator we're transitioning front->back
            animator.SetBool("isFrontSide", false);
        } else {
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
            if(newPlatform) {
                platform = Instantiate(backPlatformPrefab, GetPlatformCenter(), GetPlatformRotation());
                platform.transform.localScale = GetPlatformScale();
            } else platform = null;

            //Tell Animator we're transitioning back->front
            animator.SetBool("isFrontSide", true);
        }

        // Check for tears
        RaycastHit2D[] hits = Physics2D.RaycastAll(LastStitch.position, player.position - LastStitch.position, Vector2.Distance(LastStitch.position, player.position), LayerMask.GetMask("Tear"));
        foreach(RaycastHit2D hit in hits) {
            if(Vector2.Distance(LastStitch.position, hit.point) > tolerance && Vector2.Distance(player.position, hit.point) > tolerance) {
                hit.collider.gameObject.GetComponent<TearHandler>().Close(!behind);
            }
        }

        lockedLength += Vector2.Distance(LastStitch.position, player.position);
        LastStitch.position = player.position;
        LastStitch.GetComponent<DistanceJoint2D>().distance = length - lockedLength;

        behind = !behind;
    }

    public void Unstitch() {
        if(Vector2.Distance(LastStitch.position, player.position) < unstitchTolerance) {
            if(platform!= null) Destroy(platform);

            platform = platforms[platforms.Count - 1];
            platforms.RemoveAt(platforms.Count - 1);

            yarnSpline.RemovePointAt(yarnSpline.GetPointCount() - 2);

            if(behind) {
                animator.SetBool("isFrontSide", false);

                // Swap active platforms
                GameObject[] backPlatforms = GameObject.FindGameObjectsWithTag("BackPlatform");
                foreach(var bp in backPlatforms) {
                    bp.GetComponent<BoxCollider2D>().enabled = false;
                }
                GameObject[] frontPlatforms = GameObject.FindGameObjectsWithTag("FrontPlatform");
                foreach(var fp in frontPlatforms) {
                    if(fp == platform) continue;
                    fp.GetComponent<BoxCollider2D>().enabled = true;
                }
            } else {
                animator.SetBool("isFrontSide", true);

                // Swap active platforms
                GameObject[] backPlatforms = GameObject.FindGameObjectsWithTag("BackPlatform");
                foreach(var bp in backPlatforms) {
                    if(bp == platform) continue;
                    bp.GetComponent<BoxCollider2D>().enabled = true;
                }
                GameObject[] frontPlatforms = GameObject.FindGameObjectsWithTag("FrontPlatform");
                foreach(var fp in frontPlatforms) {
                    fp.GetComponent<BoxCollider2D>().enabled = false;
                }
            }

            lockedLength -= 2 * Vector2.Distance(LastStitch.position, platform.transform.position);
            LastStitch.position = LastStitch.position - 2 * (LastStitch.position - platform.transform.position);
            LastStitch.GetComponent<DistanceJoint2D>().distance = length - lockedLength;

            behind = !behind;

            animator.SetTrigger("makeStitch");
        }
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
