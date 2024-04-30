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

    private List<int[]> subjectData;

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
            subjectData = new List<int[]>
            {
                new int[] { 0, 0, 0 }
            };
            GenerateAndLoadScene();
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
    public void GenerateAndLoadScene() 
    {
        LoadScene(environments[subjectData[0][0]]);
        Invoke("LoadWeaponAndCharacter", 3f);
    }

    private void LoadWeaponAndCharacter()
    {

        GameObject.Find("Weapons").transform.GetChild(subjectData[0][1]).gameObject.SetActive(true);
        GameObject.Find("Characters").transform.GetChild(subjectData[0][2]).gameObject.SetActive(true);

        subjectData.RemoveAt(0);
    }

    // start the user study
    public void StartUserStudy(string groupId) 
    {
        if (groupId == "A1") {
            subjectData = new List<int[]>
            {
                new int[] {1, 2, 2},
                new int[] {0, 2, 1},
                new int[] {1, 2, 0},
                new int[] {1, 0, 1},
            };
        }
        // example
        subjectData = new List<int[]>
        {
            new int[] {1, 2, 2},
            new int[] {0, 2, 1},
            new int[] {1, 2, 0},
            new int[] {1, 0, 1},
        };

        GenerateAndLoadScene();
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
        GenerateAndLoadScene();
    }

}
