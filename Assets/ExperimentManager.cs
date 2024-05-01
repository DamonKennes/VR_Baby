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

    private List<int[]> scenesData;

    // ui stuff
    private UIDocument adminDocument;
    private Button loadSceneButton;
    private Vector3IntField loadSceneField;




    private void Awake()
    {
        // mark this gameObject to not get destroyed when another scene loads
        DontDestroyOnLoad(this.gameObject);

        // get ui elements
        adminDocument = GetComponent<UIDocument>();
        
        loadSceneButton = adminDocument.rootVisualElement.Q("LoadSceneButton") as Button;
        loadSceneField = adminDocument.rootVisualElement.Q("LoadSceneField") as Vector3IntField;
        loadSceneButton.RegisterCallback<ClickEvent>((_) => {
            scenesData = new List<int[]>
            {
                new int[] { loadSceneField.value.x, loadSceneField.value.y, loadSceneField.value.z }
            };
            GenerateAndLoadNextScene();
        });
    }

    void Update()
    {
    }


    // starts the coroutine that will generate and load the next scene provided in subjectData
    public void GenerateAndLoadNextScene() 
    {
        StartCoroutine(StartGeneratingAndLoadingScene());
    }

    // will generate and load the scene
    private IEnumerator StartGeneratingAndLoadingScene()
    {
        // disable the ui
        adminDocument.enabled = false;

        // load scene async
        AsyncOperation asyncSceneLoader = SceneManager.LoadSceneAsync(environments[scenesData[0][0]], LoadSceneMode.Single);
        Debug.Log("Loading " + environments[scenesData[0][0]]);

        // wait for the scene to be loaded
        while (!asyncSceneLoader.isDone)
        {
            yield return null;
        }
        Debug.Log("Scene succesfully loaded");

        // now the scene is loaded:
        // activate the correct weapon
        GameObject.Find("Weapons").transform.GetChild(scenesData[0][1]).gameObject.SetActive(true);
        // activate the correct character
        GameObject.Find("Characters").transform.GetChild(scenesData[0][2]).gameObject.SetActive(true);

        // update the list with our scene data
        scenesData.RemoveAt(0);
    }

    // start the user study
    public void StartUserStudy(string groupId) 
    {
        if (groupId == "A1") {
            scenesData = new List<int[]>
            {
                new int[] {1, 2, 2},
                new int[] {0, 2, 1},
                new int[] {1, 2, 0},
                new int[] {1, 0, 1},
            };
        }
        // example
        scenesData = new List<int[]>
        {
            new int[] {1, 2, 2},
            new int[] {0, 2, 1},
            new int[] {1, 2, 0},
            new int[] {1, 0, 1},
        };

        GenerateAndLoadNextScene();
    }

    // continue user study: load next scene after task is completed
    public void ContinueUserStudy() 
    {
        // collect and store results etc

        if(scenesData.Count == 0)
        {
            // finished
            //LoadEndScreenScene();
            //return;
        }
        // load next scene
        GenerateAndLoadNextScene();
    }

}
