using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearHandler : MonoBehaviour
{
    public Sprite[] tearSprites;

    private bool backPatched, frontPatched;
    private SpriteRenderer sprite;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();

        backPatched = false;
        frontPatched = false;

        sprite.sprite = tearSprites[0];
    }

    public void Close(bool front) {
        if(backPatched && frontPatched) return;

        if(front) frontPatched = true;
        else backPatched = true;

        if(frontPatched ^ backPatched) sprite.sprite = tearSprites[1];
        else if(backPatched && frontPatched) {
            sprite.sprite = tearSprites[2];
            LevelManager.GetInstance().TearClosed();
        }
    }

}
