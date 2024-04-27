using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ExperimentManager : MonoBehaviour
{
    // environments is a list of names of scenes
    public List<string> environments;
    // characters are gameobjects to be added to the environment scene
    public List<GameObject> characters;
    // weapons are gameobjects to be added to the environment scene
    public List<GameObject> weapons;

    private List<string> subjectData;

    // ui stuff
    private UIDocument adminDocument;
    private Button loadSceneButton;
    private DropdownField loadSceneField;


    private void Awake()
    {
        // mark this gameObject to not get destroyed when another scene loads
        DontDestroyOnLoad(this.gameObject);

        // get ui elements
        adminDocument = GetComponent<UIDocument>();
        
        loadSceneButton = adminDocument.rootVisualElement.Q("LoadSceneButton") as Button;
        loadSceneField = adminDocument.rootVisualElement.Q("LoadSceneField") as DropdownField;
        loadSceneButton.RegisterCallback<ClickEvent>((_) => {
            LoadScene(loadSceneField.text); 
        });
    }

    void Update()
    {
    }

    // loads a new scene by name
    public void LoadScene(string sceneName) 
    {
        // disable the ui
        adminDocument.enabled = false;
        // load scene
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    // generates a scene and loads it based on given sceneData
    // sceneData is a string of three characters, character 0 is the index of the environment, character 1 is the index of the character, character 2 is the index of the weapon
    public void GenerateAndLoadScene(string sceneData) 
    {
        if (sceneData.Length != 3) {
            Debug.LogError("Invalid scene data");
            return;
        }
        LoadScene(environments[Convert.ToInt32(sceneData[0])]);
        Instantiate(characters[Convert.ToInt32(sceneData[1])]);
        Instantiate(weapons[Convert.ToInt32(sceneData[2])]);
    }

    // start the user study
    public void StartUserStudy(string groupId) 
    {
        // example
        string groupA1 = "000+010+103+201";
        subjectData = groupA1.Split('+').ToList<string>();
        GenerateAndLoadScene(subjectData[0]);
        subjectData.RemoveAt(0);
    }

    // continue user study: load next scene after task is completed
    public void ContinueUserStudy() 
    {
        // collect and store results etc

        if(subjectData.Count == 0)
        {
            // finished
            //LoadEndScreenScene();
            //return;
        }
        // load next scene
        GenerateAndLoadScene(subjectData[0]);
        subjectData.RemoveAt(0);
    }

}
