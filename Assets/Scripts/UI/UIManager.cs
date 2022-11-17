using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Selected Image 
    /// </summary>
    [Header("IMAGEAREA")]
    [SerializeField] Image[] ImageList;
    short oldindex = 1;

    [Header("RECTAREA")]

    [SerializeField] RectTransform Screenrect;
    [SerializeField] int[] RectList;


    [Header("LEVELSAREA")]
    /// <summary>
    ///  Content object
    /// </summary>
    [SerializeField] TextMeshProUGUI[] ContentLevelsText;
    /// <summary>
    /// LevelState
    /// </summary>
    public int level;
    public static int currentlevel = 0;
    /// <summary>
    /// Selected Menu Transtation 
    /// </summary>
    [Header("MENU")]
    [SerializeField] List<menupass> Menulist;


    private void Awake()
    {
        level = PlayerPrefs.GetInt("level", 0);


    }

    private void Start()
    {
        UIManager.currentlevel = level;
        SetContentlevel();
    }

    public void MenuTranstation(int i)
    {
        Menu menu = (Menu)i;
        menupass men = Menulist.Find(x => x.menu == menu);
        if (men == null) return;
        men.init();
    }
    public void ScreenChanged(Vector2Int pos)
    {
        pos.x = Mathf.Abs(pos.x);
        ImageList[oldindex].color = new Color(ImageList[oldindex].color.r, ImageList[oldindex].color.g, ImageList[oldindex].color.b, 0f);
        ImageList[pos.x].color = new Color(ImageList[oldindex].color.r, ImageList[oldindex].color.g, ImageList[oldindex].color.b, 0.2f);
        oldindex = (short)pos.x;
    }
    public void ButtonChanged(int index)
    {
        int value = RectList[index];
        Screenrect.offsetMin = new Vector2(-value, Screenrect.offsetMin.y);
        Screenrect.offsetMax = new Vector2(-value, Screenrect.offsetMax.y);
        ImageList[oldindex].color = new Color(ImageList[oldindex].color.r, ImageList[oldindex].color.g, ImageList[oldindex].color.b, 0f);
        ImageList[index].color = new Color(ImageList[oldindex].color.r, ImageList[oldindex].color.g, ImageList[oldindex].color.b, 0.2f);
        oldindex = (short)index;

    }

    public void SetContentlevel()
    {

        int startlevel = currentlevel + 1;
        if (currentlevel == 0) return;
        if (currentlevel == 1)
        {
            ContentLevelsText[10].transform.parent.gameObject.SetActive(true);
            ContentLevelsText[10].text = (startlevel - 1).ToString();
        }
        else
        {
            ContentLevelsText[10].transform.parent.gameObject.SetActive(true);
            ContentLevelsText[10].text = (startlevel - 1).ToString();

            ContentLevelsText[11].transform.parent.gameObject.SetActive(true);
            ContentLevelsText[11].text = (startlevel - 2).ToString();
        }
        for (int i = 0; i < 10; i++)
        {
            ContentLevelsText[i].text = startlevel.ToString();
            startlevel++;
        }
    }



}
[System.Serializable]
public class menupass
{
    public Menu menu;
    public List<GameObject> ActiveObject;
    public List<GameObject> DeactiveObject;

    public void init()
    {
        ActiveObject.ForEach(x => x.SetActive(true));
        DeactiveObject.ForEach(y => y.SetActive(false));
    }

}


public enum Menu
{
    main,
    game,
    next,
    defeat
}




