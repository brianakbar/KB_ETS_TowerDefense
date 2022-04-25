using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject helpMenu;
    [SerializeField] GameObject aboutMenu;
    [SerializeField] GameObject selectLevelMenu;
    [SerializeField] GameObject quitMenu;

    GameObject currentMenu;

    void Start() {
        currentMenu = mainMenu;
    }

    public void LoadMainMenu() {
        LoadMenu(mainMenu);
    }

    public void LoadHelpMenu() {
        LoadMenu(helpMenu);
    }

    public void LoadAboutMenu() {
        LoadMenu(aboutMenu);
    }

    public void LoadSelectLevelMenu() {
        LoadMenu(selectLevelMenu);
    }

    public void LoadQuitMenu() {
        LoadMenu(quitMenu);
    }

    void LoadMenu(GameObject menuToGo) {
        currentMenu.SetActive(false);
        currentMenu = menuToGo;
        currentMenu.SetActive(true);
    }
}
