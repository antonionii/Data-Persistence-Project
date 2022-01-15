using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using TMPro;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{

    public GameObject inputField;
    public TextMeshProUGUI PlayerNameObj;
    public TMP_InputField PlayerNameInput;
    public string currentPlayer;
    public static MenuManager Instance;

    public static int highScore;
    public static Text highPlayer;


    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public void setName()
    {


        PlayerNameObj.text = PlayerNameInput.text;
        currentPlayer = PlayerNameInput.text;
        Debug.Log("currentPlayer is " + currentPlayer);


    }
    // string text = inputField.GetComponent<TMP_InputField>().text;
    // Start is called before the first frame update

    public void Update()
    {
        setName();
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);

    }
    public void Exit()
    {
        // MainManager.Instance.SaveColor();

#if UNITY_EDITOR

        EditorApplication.ExitPlaymode();

#else 
        Application.Quit();
#endif

    }

}

