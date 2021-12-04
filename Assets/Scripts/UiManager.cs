using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{


    public static UiManager Instance = null;
    public GameObject levelCompletePrefab;
    public GameObject levelFailedPrefab;


    public void Awake()
    {
        if (Instance == null) Instance = this;

    }


    public void LevelComplete()
    {
        Instantiate(levelCompletePrefab, this.transform);

    }

    public void LevelFailed()
    {
        Instantiate(levelFailedPrefab, this.transform);
    }



}
