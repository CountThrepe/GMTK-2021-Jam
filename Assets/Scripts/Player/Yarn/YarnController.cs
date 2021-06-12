using UnityEngine;
using UnityEngine.U2D;

public class YarnController : MonoBehaviour
{
    public float length = 200f;

    public Transform player;
    public Transform LastStitch;
    public GameObject platformPrefab;
    public SpriteRenderer spool;
    public Sprite[] spoolSprites;

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
        int index = yarnSpline.GetPointCount() - 1;

        yarnSpline.InsertPointAt(index, player.position + new Vector3(0, 0, 0.1f));
        yarnSpline.SetHeight(index, 0.1f);

        if(behind) {
            platform = Instantiate(platformPrefab, GetPlatformCenter(), GetPlatformRotation());
            platform.transform.localScale = GetPlatformScale();
        } else {
            platform.transform.position = GetPlatformCenter();
            platform.transform.localRotation = GetPlatformRotation();
            platform.transform.localScale = GetPlatformScale();
            platform.GetComponent<Collider2D>().enabled = true;
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
        float lengthFrac = (lockedLength + Vector2.Distance(LastStitch.position, player.position)) / length;
        int spoolIndex = (int) Mathf.Floor(lengthFrac * spoolSprites.Length);
        spool.sprite = spoolSprites[spoolIndex];
    }
}
