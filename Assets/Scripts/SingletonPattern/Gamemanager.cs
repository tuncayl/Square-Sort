using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class Gamemanager : MonoBehaviour
{
    /// <summary>
    /// The singleton, initalised on Awake.
    /// </summary>
    public static Gamemanager Instance;


    /// <summary>
    /// The UIMANAGER, UI CONTROL.
    /// </summary>
    [Header("UIMANAGER")]
    public UIManager UImanager;
    /// <summary>
    /// The SwipeController, Swipe CONTROL.
    /// </summary>
    [Header("SwipeController")]
    public SwipeController Swipe;
    /// <summary>
    /// BaseObject,in game main object.
    /// </summary>
    [Header("BASEOBJECT")]
    [HideInInspector] public GameObject BaseObject;
    [SerializeField] GameObject BasePrefab;

    /// <summary>
    /// ListCubes,Listing game cubes.
    /// </summary>
    [Header("LİstCubes")]
    public List<Rigidbody> CubesRigidbody = new List<Rigidbody>();
    /// <summary>
    /// ListCubes,Listing game walls.
    /// </summary>
    [Header("Walls")]
    public List<BoxCollider> Walls = new List<BoxCollider>();

    public static int xLength, yLength;

    public GameObject camholder;




    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(this.gameObject);
        Application.targetFrameRate=60;

    }


    public void Play(int menu)
    {
        int level = UIManager.currentlevel;
        if (BaseObject != null) Destroy(BaseObject);
        new LevelControl(level, BasePrefab, out BaseObject, out CubesRigidbody, out Walls);
        UImanager.MenuTranstation(menu);
        Swipe._SwipeCountProperty = SwipeController._SwipeCount;
        Swipe.enabled = true;
        Swipeinput.NextSwipe = true;
        SwipeController.oldvector = Vector3.zero;

    }
    public void home(int menu)
    {
        Swipe.enabled = false;
        Destroy(BaseObject ?? null);
        UImanager.MenuTranstation(menu);
    }


    public void LevelClear()
    {
        UIManager.currentlevel += 1;
        PlayerPrefs.SetInt("level", UIManager.currentlevel);
        Swipe.enabled = false;
        UImanager.SetContentlevel();
        UImanager.MenuTranstation((int)Menu.next);
    }





    public void cameraRotate(Vector3 direction)
    {
        Gamemanager.Instance.camholder.transform.DORotate(reverseVector(direction) * 7, 0.4f);

    }




    public static Vector3 reverseVector(Vector3 vector)
    {
        return vector = new Vector3(vector.z, vector.y, vector.x);
    }
}

public class LevelControl : MonoBehaviour
{
    InterAction left, right, up, down;
    int[] layerGroup = { 12, 13, 14, 15 };
    int x = 0, y = 0;
    GameObject currentMesh;
    List<cubedata> cubedatas = new List<cubedata>();
    List<Rigidbody> cuberigidbody = new List<Rigidbody>();
    List<BoxCollider> _walls = new List<BoxCollider>();

    public static string[] cubesPath ={
        "CubePrefabs/1GreenCube",
        "CubePrefabs/2PurpleCube",
        "CubePrefabs/3RedCube",
        "CubePrefabs/4YellowCube"
    };
    public LevelControl(int level, GameObject baseprefab, out GameObject baseobject, out List<Rigidbody> CubeRigidbody, out List<BoxCollider> walls)
    {
        baseobject = null;
        LevelData[] data = Resources.LoadAll<LevelData>("LevelData");

        Debug.Log(data.Length);

        if (data.Length > 0)
        {
            foreach (LevelData path in data)
            {

                if (level == path.level)
                {

                    LevelData loadedlevel = path;
                    Debug.Log(loadedlevel);
                    x = Gamemanager.xLength = loadedlevel.x;
                    y = Gamemanager.yLength = loadedlevel.y;
                    foreach (CubeVirutalData i in loadedlevel.savedObjects)
                    {
                        cubedatas.Add(new cubedata
                        {
                            localpositon = i.localpositon,
                            color = i.color

                        });
                    }
                    currentMesh = baseobject = Instantiate(baseprefab);
                    SpawnBase();
                    currentMesh.transform.GetChild(0).localScale =
                    new Vector3(x, 1, y);
                    SetChildScale();
                    SetColor(loadedlevel.cornersColor);
                    for (int i = 1; i < 5; i++) _walls.Add(baseobject.transform.GetChild(i).GetComponent<BoxCollider>());
                    SwipeController._SwipeCount = loadedlevel.SwipeCount;
                    break;
                }
            }
        }
        walls = _walls;
        CubeRigidbody = cuberigidbody;
    }
    void SetColor(int[] colors)
    {
        left.Icolor = (matColor)colors[0];
        right.Icolor = (matColor)colors[1];
        up.Icolor = (matColor)colors[2];
        down.Icolor = (matColor)colors[3];
        for (int i = 1; i < 5; i++)
        {
            currentMesh.transform.GetChild(i).gameObject.layer = layerGroup[colors[i - 1] - 1];
        }
    }

    void SetChildScale()
    {
        for (int i = 1; i < currentMesh.transform.childCount - 2; i++)
        {
            currentMesh.transform.GetChild(i).transform.GetComponent<InterAction>().Interact(x - 1, y - 1);

        }
        SetHolder(currentMesh);

    }
    void SetHolder(GameObject currentMesh)
    {
        int childCount = currentMesh.transform.childCount;
        GameObject col = currentMesh.transform.GetChild(13).GetChild(0).transform.gameObject;
        col.gameObject.layer = 7;
        col.GetComponent<MeshRenderer>().enabled = true;
        Removeoldobject(14, 0);
        float LocalX = (x - 1) * 0.52f;
        float LocalY = (y - 1) * 0.52f;
        for (int i = (x * 2) - 1; i > 0; i--)
        {

            for (int j = (y * 2) - 1; j > 0; j--)
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
                        GameObject cubeobject = Resources.Load (cubesPath[(int)data.color - 1]) as GameObject;
                        data.cube = Instantiate(cubeobject);
                        data.cube.transform.parent = currentMesh.transform.GetChild(14);
                        data.cube.transform.localPosition = data.localpositon;
                        cuberigidbody.Add(data.cube.GetComponent<Rigidbody>());
                        data.cube.layer = layerGroup[(int)data.color - 1] - 4;
                    }
                }
                Destroy(h);

            }
            LocalX -= 0.52f;
            LocalY = (y - 1) * 0.52f;
        }
        col.gameObject.layer = 2;
        col.GetComponent<MeshRenderer>().enabled = false;
        Destroy(col);
    }
    void Removeoldobject(int child, int limit)
    {
        if (currentMesh.transform.GetChild(child).childCount > limit)
        {

            while (currentMesh.transform.GetChild(child).childCount > limit)
            {
                Destroy(currentMesh.transform.GetChild(child).transform.GetChild(limit).transform.gameObject);
            }
        }
    }
    void SpawnBase()
    {
        left = currentMesh.transform.GetChild(1).GetComponent<InterAction>();
        right = currentMesh.transform.GetChild(2).GetComponent<InterAction>();
        up = currentMesh.transform.GetChild(3).GetComponent<InterAction>();
        down = currentMesh.transform.GetChild(4).GetComponent<InterAction>();
    }
}
