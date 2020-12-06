using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<GameManager>();
                if (singleton == null)
                {
                    singleton = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }
            return singleton;
        }
    }

    private static GameManager singleton = null;

    int score = 0;

    public FishSwarm sardineSwarm;
    public GUIStyle MyFont1;
    public GUIStyle MyFont2;
    public GUIStyle MyToggle;
    bool isOpenSetting = false;


    public void RestartGame()
    {
        score = 0;
        FishSwarm[] swarms = FindObjectsOfType<FishSwarm>();
        foreach (var s in swarms)
        {
            s.InitialSwarm();
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void AddScore(int add)
    {
        score += add;
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Score: " + score, MyFont1);
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                {
                    isOpenSetting = GUILayout.Toggle(isOpenSetting, "", MyToggle);
                    GUILayout.Label("Sardine Swarm (200) Setting", MyFont1);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Space(20);
                    GUILayout.BeginVertical();
                    {
                        if (isOpenSetting)
                        {
                            DrawMySlider("Cohesion with Swarm Weight", ref sardineSwarm.cohesionWithSwarmWeight, MyFont2);
                            DrawMySlider("Cohesion Weight", ref sardineSwarm.cohesionWeight, MyFont2);
                            DrawMySlider("Separation Weight", ref sardineSwarm.separationWeight, MyFont2);
                            DrawMySlider("Alignment Weight", ref sardineSwarm.alignmentWeight, MyFont2);
                            DrawMySlider("Random Weight", ref sardineSwarm.randomWeight, MyFont2);
                            DrawMySlider("Viewing Angle", ref sardineSwarm.viewingAngle, MyFont2, 10, 360);
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    void DrawMySlider(string name, ref float value, GUIStyle myFont, float minVal = 0f, float maxVal = 50f,float nameWidth = 340f,float sliderWidth = 100f)
    {

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label(name, myFont, GUILayout.Width(nameWidth));
            value = GUILayout.HorizontalSlider(value, minVal, maxVal, GUILayout.Width(sliderWidth));
            GUILayout.Label(string.Format("{0:00.00}", value), myFont);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
}
