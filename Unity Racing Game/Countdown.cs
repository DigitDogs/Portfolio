using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{

    public GameObject CountDown;
    public AudioSource GetReady;
    public AudioSource GoAudio;
    public GameObject LapTimer;
    public GameObject LapTimer2;
    public GameObject CarControls;

    void Start()
    {
        StartCoroutine(CountStart());
    }


    IEnumerator CountStart()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(0.5f);
        CountDown.GetComponent<Text>().text = "3";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "2";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        CountDown.SetActive(false);
        CountDown.GetComponent<Text>().text = "1";
        GetReady.Play();
        CountDown.SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        CountDown.SetActive(false);
        GoAudio.Play();

        if(LapTimer != null)
            LapTimer.SetActive(true);

        if (LapTimer2 != null)
            LapTimer2.SetActive(true);

        Time.timeScale = 1f;
    }


}