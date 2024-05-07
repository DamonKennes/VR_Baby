using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;
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

    private Button startButton;
    private TextField userIdField;
    private DropdownField groupField;

    // storing data stuff
    private string userId;
    private float timer = 0f;
    private bool isRunning = false;

    protected enum Environment
    {
        FantasyForest,
        Cabin,
        House
    }

    protected enum Weapon 
    {
        Button,
        Gun,
        Knife
    }

    protected enum Character 
    {
        Goblin,
        Zombie,
        Eggy,
        Man,
        Woman
    }


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

        startButton = adminDocument.rootVisualElement.Q("StartButton") as Button;
        userIdField = adminDocument.rootVisualElement.Q("SubjectId") as TextField;
        groupField = adminDocument.rootVisualElement.Q("GroupField") as DropdownField;
        startButton.RegisterCallback<ClickEvent>((_) => {
            userId = userIdField.text;
            StartUserStudy(groupField.value);
        });


    }

    // keep track of timer
    void Update()
    {
        if(isRunning) {
            timer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            ContinueUserStudy();
        }
    }


    // starts the coroutine that will generate and load the next scene provided in subjectData
    public void GenerateAndLoadNextScene() 
    {
        // reset timer:
        timer = 0f;
        // start coroutine
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

        // start timer
        isRunning = true;

    }

    // start the user study
    public void StartUserStudy(string groupId) 
    {
        switch (groupId)
        {
            case "A1":
                scenesData = new List<int[]>
                {
                    new int[] { (int)Environment.FantasyForest, (int)Weapon.Gun, (int)Character.Man },
                    new int[] { (int)Environment.Cabin, (int)Weapon.Button, (int)Character.Goblin },
                    new int[] { (int)Environment.House, (int)Weapon.Knife, (int)Character.Eggy },
                    new int[] { (int)Environment.Cabin, (int)Weapon.Gun, (int)Character.Zombie },
                };
                break;
            case "A2":
                break;
            case "A3":
                break;
            case "B1":
                break;
            case "B2":
                break;
            case "B3":
                break;
            default:
                Debug.LogError("Unknown group identifier");
                break;
        }

        GenerateAndLoadNextScene();
    }

    // continue user study: load next scene after task is completed
    private void ContinueUserStudy() 
    {
        // if character has not beel killed:
        storeData(false);

        // update the list with our scene data (remove the previously loaded scene)
        scenesData.RemoveAt(0);

        // if no more scene data:
        if (scenesData.Count == 0)
        {
            Debug.Log("Finished!");
            // finished
            //LoadEndScreenScene();
            return;
        }
        // else load next scene:
        GenerateAndLoadNextScene();
    }

    // collect and store results
    public void FinishedTask() 
    {
        // stop timer
        isRunning = false;
        // store data
        storeData(true);
    }

    private void storeData(bool isMurderer) 
    {
        // store data
        string path = Application.persistentDataPath;

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, userId + ".txt"), true))
        {
            outputFile.WriteLine("Env: " + environments[scenesData[0][0]] + ", Weapon: " + ((Weapon)scenesData[0][1]).ToString() + ", Char: " + ((Character)scenesData[0][2]).ToString());
            if (isMurderer)
            {
                outputFile.WriteLine("Character has been killed");
            }
            else
            {
                outputFile.WriteLine("Character has been spared");
            }
            outputFile.WriteLine("Timer: " + timer);
            outputFile.WriteLine();
        }
        Debug.Log("Stored data in " + path);
    }

}
