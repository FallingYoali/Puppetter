using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScreen : MonoBehaviour
{
    public void PressPlay()
    {
        Debug.Log("aqui toy");
        SceneManager.LoadScene("prototipo");
    }
}
