using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearHandler : MonoBehaviour
{
    private bool open = true;

    public void Close() {
        if(open) {
            Debug.Log("Closing!");
            open = false;
            LevelManager.GetInstance().TearClosed();
        }
    }

}
