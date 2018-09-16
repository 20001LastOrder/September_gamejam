using UnityEngine;
using UnityEditor;

public class EnvironmentManager : EditorWindow {

    public int levelLength;
    public int levelHeight;
    public float stepHeightMax;
    public float stepHeightMin;
    public float gapDistance;
    public Vector3 startPoint;
    public GameObject floor;
   

    [MenuItem("Window/Environment")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<EnvironmentManager>("Environment");
    }

    private void OnGUI()
    {
        GUILayout.Label("Floor Prefab");
        floor = (GameObject)EditorGUILayout.ObjectField(floor, typeof(GameObject), false);
        
        levelLength = EditorGUILayout.IntField("Level Length", levelLength);
        levelHeight = EditorGUILayout.IntField("Level Height", levelHeight);

        GUILayout.BeginHorizontal();
        stepHeightMax = EditorGUILayout.FloatField("Max Step", stepHeightMax);
        stepHeightMin = EditorGUILayout.FloatField("Min Step", stepHeightMin);
        GUILayout.EndHorizontal();

        gapDistance = EditorGUILayout.FloatField("Gap Distance", gapDistance);

        startPoint = EditorGUILayout.Vector3Field("Start Location", startPoint);

        if (GUILayout.Button("Initialize"))
        {
            Initialize();
        }

        if (GUILayout.Button("Generate Level"))
        {
            
            GenerateLevel();
        }
    }

    private void Initialize()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GeneratedItem");

        foreach (GameObject obj in gameObjects)
            DestroyImmediate(obj, false);
    }

    // generate a platformer level based on the params
    private void GenerateLevel()
    {
        GameObject gameObject = null;
        GameObject previousObj;
        int objectLength = 10;
        Vector3 spawnPoint = startPoint;
        int record = 0;
        int nextStep = 0;
        float height = 0;

        while (record <= levelLength && floor != null) {
            previousObj = gameObject;
            gameObject = (GameObject)PrefabUtility.InstantiatePrefab(floor);
            bool reachLimit = false;
            // level generation rule
            // choose to go up or down
            if (reachLimit == false)
            {
                nextStep = Random.Range(0, 8);
            }

            // reset the reach limit flag
            reachLimit = false;

            if (nextStep == 0 || nextStep == 1)
            {
                // go up
                float randomHeight = Random.Range(stepHeightMin, stepHeightMax);
                height += randomHeight;

                if (height >= levelHeight)
                {
                    nextStep = 2;
                    reachLimit = true;
                }

                spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y + randomHeight, spawnPoint.z + objectLength);
                SetParam(gameObject, spawnPoint);
            }

            else if (nextStep == 2)
            {
                // go down
                float randomHeight = Random.Range(stepHeightMin, stepHeightMax);
                height -= randomHeight;
                spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y - randomHeight, spawnPoint.z + objectLength);
                SetParam(gameObject, spawnPoint);
            }

            else if (nextStep == 3 || nextStep == 4)
            {
                // make gap horizontally
                gameObject = (GameObject)PrefabUtility.InstantiatePrefab(floor);
                // two gaps
                for (int i = 0; i < 3; i++)
                {
                    spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z + gapDistance);
                    SetParam(gameObject, spawnPoint);
                }
            }

            //else if (nextStep == 4)
            //{
            //    // make gap with incline
            //    gameObject = (GameObject)PrefabUtility.InstantiatePrefab(floor);
            //    // two gaps
            //    for (int i = 0; i < 4; i++)
            //    {
            //        float randomHeight = Random.Range(stepHeightMin, stepHeightMax);
            //        height += randomHeight;

            //        // make sure it doesnt go beyond the height limit
            //        if (height >= levelHeight)
            //        {
            //            nextStep = 2;
            //            reachLimit = true;
            //            break;
            //        }

            //        spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y + randomHeight, spawnPoint.z + gapDistance / 2);
            //        SetParam(gameObject, spawnPoint);
            //    }
            //}

            else
            {
                spawnPoint = new Vector3(spawnPoint.x, spawnPoint.y, spawnPoint.z + objectLength);
                SetParam(gameObject, spawnPoint);
            }

            // go the next tile with distance of 10
            Debug.Log(height);
            record += objectLength;
        }
    }

    private void SetParam(GameObject gameObject, Vector3 spawnPoint)
    {
        gameObject.tag = "GeneratedItem";
        gameObject.transform.position = spawnPoint;
    }
}
