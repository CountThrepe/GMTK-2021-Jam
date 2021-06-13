using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThreadDisplay : MonoBehaviour
{
    public TMP_Text text;
    
    private int totalThread;
    private int currentThread;

    void Update() {
        string displayStr = currentThread.ToString();
        while(displayStr.Length < 3) {
            displayStr = " " + displayStr;
        }

        displayStr += " / " + totalThread.ToString();
        text.text = displayStr;
    }
    
    public void SetTotalThread(float val) {
        totalThread = (int) val;
    }

    public void SetCurrentThread(float val) {
        currentThread = (int) (val < totalThread ? val : totalThread);
        
    }
}
