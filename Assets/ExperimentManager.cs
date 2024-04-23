using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentManager : MonoBehaviour
{
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;
    private int currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = 0;
        //myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/Environments");
        //scenePaths = myLoadedAssetBundle.GetAllScenePaths();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //currentScene %= scenePaths.Length - 1;
            //Debug.Log("scene2 loading: " + scenePaths[currentScene]);
            SceneManager.LoadScene(UnityEngine.Random.Range(0, 2), LoadSceneMode.Single);
           // currentScene++;
            //currentScene %= 2;





        }
    }


}
