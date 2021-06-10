using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Sprite fullHeart, emptyHeart;
    public Image[] hearts;
    public int maxHearts = 3;
    public int currHearts;
    void Start()
    {
        currHearts = maxHearts;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(i >= maxHearts) hearts[i].enabled = false;
            else
            {
                hearts[i].enabled = true;
                if(i >= currHearts) hearts[i].sprite = emptyHeart;
                else hearts[i].sprite = fullHeart;
            }
        }
    }
}
