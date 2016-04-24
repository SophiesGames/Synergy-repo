using UnityEngine;
using System.Collections;

public class DeveloperConsole : MonoBehaviour
{
    private string consoleInput = "";
    private string levelName;

    private bool userHasHitReturn = false;

    public void OnGUI()
    {
        if (Debug.isDebugBuild == true)
        {
            GUI.TextField(new Rect(450, 10, 150, 20), LevelManager.levelManager.levelDetails);


        }
        if (GameManagerC.gameManager.isConsoleActive)
            {
                Event e = Event.current;

                if (e.keyCode == KeyCode.Return)
                {
                    ReadString();
                }

                if (userHasHitReturn == false)
                {
                    consoleInput = GUI.TextField(new Rect(450, 30, 150, 20), consoleInput);
                }
            }
    }

    private bool ReadString()
    {
        int levelNumb;
        if (int.TryParse(consoleInput, out levelNumb))
        {
            consoleInput = "";
            GameManagerC.gameManager.LoadLevelNumber(levelNumb);
            return true;
        }
        else
        {
            consoleInput = "";
            return false;
        }
    }
}
