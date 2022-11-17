using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public enum matColor
{
    white = 0,
    green = 1,
    purple = 2,
    red = 3,
    yellow = 4
}
#if UNITY_EDITOR

public class LevelCreate : EditorWindow
{
    private Texture2D logo = null;


    private int x = 0, y = 0;

    private short level = 0;

    //private int upSize = 5;

    int MeshX = 0, Meshy = 0;

    static int MoveCount = default;

    static GameObject currentMesh = null;

    private Toggle tog;

    private GameObject prefabCube;

    private GameObject SelectedCube;

    List<cubedata> cubedatas = new List<cubedata>();

    RaycastHit hit;

    GameObject cubeSelect
    {
        get => prefabCube;
        set
        {
            if (cubeSelect != value)
            {
                DestroyImmediate(SelectedCube);
                prefabCube = value;
                SpawnCube(value);
            }
        }
    }
    int Toolbarindex = 0;
    int CubesToolbarindex
    {
        get => Toolbarindex;
        set
        {
            if (Toolbarindex != value)
            {
                Toolbarindex = value;
                cubeSelect = (GameObject)AssetDatabase.LoadAssetAtPath(cubesPath[value], typeof(GameObject));
                if (SceneView.sceneViews.Count > 0)
                {
                    SceneView sceneView = (SceneView)SceneView.sceneViews[0];
                    sceneView.Focus();
                }
            }

        }

    }
    string[] cubesToolBar = { "Green", "Purple", "Red", "Yellow" };
    public static string[] cubesPath ={
        "Assets/Resources/CubePrefabs/1GreenCube.prefab",
        "Assets/Resources/CubePrefabs/2PurpleCube.prefab",
        "Assets/Resources/CubePrefabs/3RedCube.prefab",
        "Assets/Resources/CubePrefabs/4YellowCube.prefab"
    };

    InterAction left, right, up, down;

    static string BasePath = "Assets/Prefabs/Base.prefab";

    [MenuItem("LevelGeneration/CreateLevel")]
    static void Init()
    {
        var window = GetWindow<LevelCreate>();
        window.titleContent = new GUIContent("LEVELCREATE");
        window.Show();

    }

    void OnGUI()
    {
        //logo
        GUILayout.Label(logo);
        //logo
        //SIZE SETTING 
        GUILayout.Label("SİZE SETTING");
        y = EditorGUILayout.IntField("Y-SIZE", y);
        x = EditorGUILayout.IntField("X-SIZE", x);
        if (GUILayout.Button("Create Base Mesh")) CreateBase();
        //SIZE SETTING

        if (currentMesh != null)
        {

            GUILayout.BeginVertical();
            GUI.contentColor = Color.red;
            GUILayout.Label("LEFT", EditorStyles.boldLabel);
            GUI.contentColor = Color.white;
            left.Icolor = (matColor)EditorGUILayout.EnumPopup("COLOR: ", left.Icolor);

            GUI.contentColor = Color.red;
            GUILayout.Label("RIGHT", EditorStyles.boldLabel);
            GUI.contentColor = Color.white;
            right.Icolor = (matColor)EditorGUILayout.EnumPopup("COLOR: ", right.Icolor);

            GUI.contentColor = Color.red;
            GUILayout.Label("UP", EditorStyles.boldLabel);
            GUI.contentColor = Color.white;
            up.Icolor = (matColor)EditorGUILayout.EnumPopup("COLOR: ", up.Icolor);


            GUI.contentColor = Color.red;
            GUILayout.Label("DOWN", EditorStyles.boldLabel);
            GUI.contentColor = Color.white;
            down.Icolor = (matColor)EditorGUILayout.EnumPopup("COLOR: ", down.Icolor);
            GUILayout.EndVertical();

            GUI.contentColor = Color.red;
            GUILayout.Label("PREFAB SELECT", EditorStyles.boldLabel);
            GUI.contentColor = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("SELECT CUBE", EditorStyles.boldLabel);
            CubesToolbarindex = GUILayout.Toolbar(CubesToolbarindex, cubesToolBar);
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical();
            level = (short)EditorGUILayout.IntField("LEVEL", level);
            LevelSelectButtons();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("MOVE LEFT", EditorStyles.boldLabel);
            MoveCount = EditorGUILayout.IntField("MoveLeft", MoveCount);
            LevelSelectButtons();
            GUILayout.EndVertical();
        }
    }

    void LevelSelectButtons()
    {
        if (GUILayout.Button("LOAD LEVEL")) LoadLevel(level);
        if (GUILayout.Button("SAVE LEVEL")) SaveLevel();
        if (GUILayout.Button("DELETE LEVEL")) DeleteLevel();
    }
    void SaveLevel()
    {
        LevelData asset = ScriptableObject.CreateInstance<LevelData>();
        MonoScript sourceScriptAsset = MonoScript.FromScriptableObject(asset);
        string pathfolder = AssetDatabase.GetAssetPath(sourceScriptAsset);
        pathfolder = pathfolder.Substring(0, pathfolder.LastIndexOf("L"));
        AssetDatabase.CreateAsset(asset, "Assets/Resources/LevelData/" + level + ".asset");
        foreach (cubedata item in cubedatas)
        {
            asset.savedObjects.Add(new CubeVirutalData
            {
                color = item.color,
                localpositon = item.localpositon
            });
        }

        asset.level = level;
        asset.x = x;
        asset.y = y;
        asset.SwipeCount = MoveCount;
        asset.cornersColor = new int[] { (int)left.Icolor, (int)right.Icolor, (int)up.Icolor, (int)down.Icolor };
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(asset);
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    public void LoadLevel(short Level)
    {
        string[] data = AssetDatabase.FindAssets("t:LevelData", new string[] { "Assets/Resources/LevelData/" });
        string find = "Assets/Resources/LevelData/" + Level + ".asset";
        if (data.Length > 0)
        {
            foreach (string path in data)
            {
                var sopath = AssetDatabase.GUIDToAssetPath(path);
                if (find == sopath)
                {
                    resetxy();
                    DestroyImmediate(currentMesh);
                    cubedatas.Clear();
                    LevelData loadedlevel = AssetDatabase.LoadAssetAtPath<LevelData>(sopath);
                    x = loadedlevel.x;
                    y = loadedlevel.y;
                    MoveCount = loadedlevel.SwipeCount;
                    foreach (CubeVirutalData i in loadedlevel.savedObjects)
                    {
                        cubedatas.Add(new cubedata
                        {
                            localpositon = i.localpositon,
                            color = i.color
                        });
                    }
                    SpawMesh();
                    currentMesh.transform.GetChild(0).localScale = new Vector3(x, 1, y);
                    SetMeshsize();
                    SetChildScale();
                    SetColor(loadedlevel.cornersColor);
                    break;
                }

            }


        }
    }
    void SetColor(int[] colors)
    {
        left.Icolor = (matColor)colors[0];
        right.Icolor = (matColor)colors[1];
        up.Icolor = (matColor)colors[2];
        down.Icolor = (matColor)colors[3];

    }
    void DeleteLevel()
    {
        string[] data = AssetDatabase.FindAssets("t:LevelData", new string[] { "Assets/Resources/LevelData/" });
        string findme1 = "Assets/Resources/LevelData/" + level + ".asset";
        if (data.Length > 0)
        {
            foreach (string path in data)
            {
                var sopath = AssetDatabase.GUIDToAssetPath(path);
                if (findme1 == sopath)
                {
                    AssetDatabase.DeleteAsset(sopath);
                    break;
                }
            }
        }
    }
    private void OnSceneGUI(SceneView scene)
    {
        if (currentMesh == null) return;
        if (SelectedCube == null) return;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            int layer = hit.transform.gameObject.layer;
            if (layer == 7) SelectedCube.transform.position = hit.transform.position;
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == KeyCode.LeftControl)
                {
                    if (layer == 0)
                    {
                        cubedata cube_data = cubedatas.Find(x => x.localpositon == hit.transform.localPosition);
                        DestroyImmediate(hit.transform.gameObject);
                        cube_data.parent.gameObject.layer = 7;
                        cube_data.parent.GetComponent<MeshRenderer>().enabled = true;
                        cubedatas.Remove(cube_data);
                        return;
                    }
                }
                else if (Event.current.keyCode == KeyCode.LeftShift)
                {
                    if (layer == 7)
                    {
                        GameObject holdercube = Instantiate(SelectedCube, currentMesh.transform.GetChild(14).transform);
                        holdercube.gameObject.layer = 0;
                        hit.transform.gameObject.GetComponent<MeshRenderer>().enabled = false;
                        hit.transform.gameObject.layer = 2;
                        cubedatas.Add(new cubedata
                        {
                            cube = holdercube,
                            localpositon = holdercube.transform.localPosition,
                            parent = hit.transform,
                            color = (matColor)int.Parse(holdercube.gameObject.name[0].ToString())
                        });
                    }
                }
            }
        }

    }

    void SpawnCube(GameObject gameObject)
    {
        SelectedCube = (GameObject)PrefabUtility.InstantiatePrefab(gameObject);
        SelectedCube.layer = 2;
    }
    private void OnEnable()
    {
        logo = (Texture2D)Resources.Load("logo", typeof(Texture2D));
        prefabCube = (GameObject)AssetDatabase.LoadAssetAtPath(BasePath, typeof(GameObject));
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;

    }
    private void OnDisable()
    {
        if (currentMesh != null) DestroyImmediate(currentMesh);
        if (SelectedCube != null) DestroyImmediate(SelectedCube);
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void CreateBase()
    {
        //Check AnotherMeshSize
        if (MeshX == x && Meshy == y) return;
        //Spawn Mesh
        cubedatas.Clear();

        if (currentMesh == null) SpawMesh();
        //SETSIZE
        currentMesh.transform.GetChild(0).localScale = new Vector3(x, 1, y);
        SetMeshsize();
        SetChildScale();
    }

    void resetxy()
    {
        MeshX = 0;
        Meshy = 0;
    }
    void SetChildScale()
    {
        for (int i = 1; i < currentMesh.transform.childCount - 2; i++)
        {
            currentMesh.transform.GetChild(i).transform.GetComponent<InterAction>().Interact(x - 1, y - 1);

        }
        SetHolder();

    }
    void Removeoldobject(int child, int limit)
    {
        if (currentMesh.transform.GetChild(child).childCount > limit)
        {

            while (currentMesh.transform.GetChild(child).childCount > limit)
            {
                Debug.Log(child);
                DestroyImmediate(currentMesh.transform.GetChild(child).transform.GetChild(limit).transform.gameObject);
            }
        }
    }
    void SetHolder()
    {
        int childCount = currentMesh.transform.childCount;
        GameObject col = currentMesh.transform.GetChild(13).GetChild(0).transform.gameObject;
        col.gameObject.layer = 7;
        col.GetComponent<MeshRenderer>().enabled = true;
        Removeoldobject(13, 1);
        Removeoldobject(14, 0);
        float LocalX = (MeshX - 1) * 0.52f;
        float LocalY = (Meshy - 1) * 0.52f;

        for (int i = (MeshX * 2) - 1; i > 0; i--)
        {

            for (int j = (Meshy * 2) - 1; j > 0; j--)
            {
                GameObject h = Instantiate(col, currentMesh.transform.GetChild(13).transform);
                h.transform.localPosition = new Vector3(col.transform.localPosition.x + LocalX, h.transform.localPosition.y, col.transform.localPosition.z + LocalY);
                LocalY -= 0.52f;
                if (cubedatas.Count > 0)
                {
                    cubedata data = cubedatas.Find(x => x.localpositon == h.transform.position);
                    if (data != null)
                    {
                        data.parent = h.transform;
                        GameObject cubeobject = (GameObject)AssetDatabase.LoadAssetAtPath(cubesPath[(int)data.color - 1], typeof(GameObject));
                        data.cube = (GameObject)PrefabUtility.InstantiatePrefab(cubeobject);
                        data.cube.transform.parent = currentMesh.transform.GetChild(14);
                        data.cube.transform.localPosition = data.localpositon;
                        h.GetComponent<MeshRenderer>().enabled = false;
                        h.gameObject.layer = 2;
                    }
                }

            }
            LocalX -= 0.52f;
            LocalY = (Meshy - 1) * 0.52f;
        }
        col.gameObject.layer = 2;
        col.GetComponent<MeshRenderer>().enabled = false;

    }

    void SpawMesh()
    {
        Object BaseFloor = SpawnObject(BasePath);
        if (BaseFloor != null)
        {
            currentMesh = (GameObject)PrefabUtility.InstantiatePrefab(BaseFloor);
            left = currentMesh.transform.GetChild(1).GetComponent<InterAction>();
            right = currentMesh.transform.GetChild(2).GetComponent<InterAction>();
            up = currentMesh.transform.GetChild(3).GetComponent<InterAction>();
            down = currentMesh.transform.GetChild(4).GetComponent<InterAction>();
        }


    }

    Object SpawnObject(string path)
    {
        return AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
    }
    void SetMeshsize()
    {
        MeshX = x;
        Meshy = y;
    }


}
#endif
[System.Serializable]
public record cubedata
{
    public GameObject cube;
    public Vector3 localpositon;
    public Transform parent;

    public matColor color;

}






