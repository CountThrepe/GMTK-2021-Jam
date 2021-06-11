using UnityEngine;
using UnityEngine.U2D;

public class YarnController : MonoBehaviour
{
    public Transform player;
    public GameObject platformPrefab;

    private SpriteShapeController spriteShape;
    private Spline yarnSpline;

    private bool behind;
    private Vector2 lastStich;

    void Start()
    {
        behind = true;

        spriteShape = GetComponent<SpriteShapeController>();
        yarnSpline = spriteShape.spline;
    }

    // Update is called once per frame
    void Update()
    {
        int nPoints = yarnSpline.GetPointCount();
        yarnSpline.SetPosition(nPoints - 1, player.position);
    }

    public void Stitch() {
        int index = yarnSpline.GetPointCount() - 1;
        Vector3 offsetPosition = player.position + new Vector3(0, 0, 0.1f);

        yarnSpline.InsertPointAt(index, offsetPosition);
        yarnSpline.SetHeight(index, 0.1f);

        if(!behind) {
            Vector2 newStitch = offsetPosition;
            Vector2 center = lastStich + ((newStitch - lastStich) / 2);
            Quaternion quat = new Quaternion();
            quat.SetFromToRotation(new Vector2(1, 0), newStitch - lastStich);

            GameObject newPlatform = Instantiate(platformPrefab, center, quat);
            Vector3 scale = newPlatform.transform.localScale;
            scale.x *= Vector2.Distance(newStitch, lastStich);
            newPlatform.transform.localScale = scale;
        }

        lastStich = offsetPosition;
        behind = !behind;
    }
}
