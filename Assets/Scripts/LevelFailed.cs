using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelFailed : MonoBehaviour
{
    public Button btnRetry;

    private void Start()
    {
        btnRetry.onClick.AddListener(() => RestartCallBack());
    }

    private void RestartCallBack()
    {
        SceneManager.LoadScene("Game");
    }
}
