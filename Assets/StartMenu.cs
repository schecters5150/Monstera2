using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public InputMaster _inputMaster;
    private string selectedScene;
    private int index;
    private int sceneCount;
    public GameObject textMeshObject;

    private void Awake()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        _inputMaster = new InputMaster();
    }
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        selectedScene = SceneUtility.GetScenePathByBuildIndex(index);
        textMeshObject.GetComponent<TMPro.TextMeshProUGUI>().text = selectedScene;

        if (_inputMaster.Platformer.Jump.triggered)
        {

            SceneManager.LoadScene(selectedScene);
        }

        if (_inputMaster.Platformer.Hover.triggered) Application.Quit();

        if (_inputMaster.Platformer.Dodge.triggered) index++;
        if (index == sceneCount) index = 0;

    }

    private void OnEnable()
    {
        _inputMaster.Enable();
    }
    private void OnDisable()
    {
        _inputMaster.Disable();
    }
}
