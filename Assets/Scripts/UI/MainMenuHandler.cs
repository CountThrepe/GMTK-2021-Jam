using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public Animator animator;
    public GameObject fg;
    public float transDuration = 1f;

    private float transEnd;

    void Start()
    {
        transEnd = -1;
    }

    void Update()
    {
        if(transEnd != -1)
        {
            if(Time.time > transEnd) SceneManager.LoadScene(1);
        }
    }

    public void StartTransition()
    {
        // fg.SetActive(true);
        // animator.SetTrigger("Hush");
        transEnd = Time.time + transDuration;
    }
}
