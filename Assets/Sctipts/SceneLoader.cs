using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameData _data;

    private void Awake()
    {
        _data.Load();

        if(SceneManager.GetActiveScene().buildIndex != _data.LastLevelIndex)
        {
            SceneManager.LoadScene(_data.LastLevelIndex);
        }
    }
}