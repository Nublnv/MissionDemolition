using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public enum GameMod
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamicaly")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMod mode = GameMod.idle;
    public string showing = "Show Slingshot";
    // Start is called before the first frame update
    void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject tGO in gos)
            Destroy(tGO);

        castle = Instantiate(castles[level]);
        castle.transform.position = castlePos;


        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMod.playing;
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots taken: " + shotsTaken;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if ((mode == GameMod.playing) && Goal.goalMet)
        {
            mode = GameMod.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }    
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
            eView = uitButton.text;
        showing = eView;
        switch(showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
