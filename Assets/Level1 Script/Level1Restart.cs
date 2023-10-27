using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Level1Restart : MonoBehaviour
{
    private Button restartButton;

    private void Start()
    {
        restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(RestartLevel);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}