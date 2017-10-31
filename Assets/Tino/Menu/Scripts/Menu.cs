using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    private Sprite[] Strings;
    private GameObject[] MainMenu = new GameObject[5];
    private GameObject[] OptionsMenu = new GameObject[3];

    private int SelectedOption = 0;

    private Action Handler;
    
	void Start () {
        this.Strings = Resources.LoadAll<Sprite>("menu_strings");

        this.CreateMenus();
        this.ShowMenu(this.MainMenu);
        this.Handler = new Action(this.MainMenuHandler);
    }
    
    void Update () {
        this.Handler();
    }

    private void MainMenuHandler()
    {
        int previousSelection = this.SelectedOption;

        if (Input.GetKeyDown(KeyCode.UpArrow)) { this.SelectedOption -= 1; }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { this.SelectedOption += 1; }
        this.SelectedOption = Mathf.Clamp(this.SelectedOption, 0, this.MainMenu.Length - 1);

        if (this.SelectedOption != previousSelection)
        {
            this.UnboldMenuItem(this.MainMenu, previousSelection, previousSelection);
            this.BoldMenuItem(this.MainMenu, this.SelectedOption, this.SelectedOption);
        }
        
        if (this.SelectedOption == 0 && Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterTest");
        }

        if (this.SelectedOption == 2 && Input.GetKeyDown(KeyCode.Return))
        {
            this.HideMenu(this.MainMenu);
            this.ShowMenu(this.OptionsMenu);
            this.Handler = this.OptionsMenuHandler;
        }

        if ((this.SelectedOption == 5 && Input.GetKeyDown(KeyCode.Return)) ||
            Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OptionsMenuHandler()
    {
        int previousSelection = this.SelectedOption;
        if (Input.GetKeyDown(KeyCode.UpArrow)) { this.SelectedOption -= 1; }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) { this.SelectedOption += 1; }
        this.SelectedOption = Mathf.Clamp(this.SelectedOption, 0, this.OptionsMenu.Length - 1);

        if(this.SelectedOption != previousSelection)
        {
            this.UnboldMenuItem(this.OptionsMenu, previousSelection, previousSelection + 6);
            this.BoldMenuItem(this.OptionsMenu, this.SelectedOption, this.SelectedOption + 6);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.HideMenu(this.OptionsMenu);
            this.ShowMenu(this.MainMenu);
            this.Handler = this.MainMenuHandler;
        }
    }

    private void CreateMenus()
    {
        this.MainMenu[0] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[0]);
        this.MainMenu[1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[1]);
        this.MainMenu[2] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[2]);
        this.MainMenu[3] = this.CreateMenuItem(new Vector3(0, -100, 0), this.Strings[3]);
        this.MainMenu[4] = this.CreateMenuItem(new Vector3(0, -200, 0), this.Strings[4]);
        this.HideMenu(this.MainMenu);
        this.OptionsMenu[0] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[6]);
        this.OptionsMenu[1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[7]);
        this.OptionsMenu[2] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[8]);
        this.HideMenu(this.OptionsMenu);
    }

    private void ShowMenu(GameObject[] menu)
    {
        for(int i = 0; i < menu.Length; i++)
        {
            menu[i].SetActive(true);
        }
    }
    
    private void HideMenu(GameObject[] menu)
    {
        for (int i = 0; i < menu.Length; i++)
        {
            menu[i].SetActive(false);
        }
    }

    private GameObject CreateMenuItem(Vector3 pos, Sprite sprite)
    {
        GameObject g = new GameObject();
        g.transform.position = pos;
        g.AddComponent<SpriteRenderer>();
        SpriteRenderer n = g.GetComponent<SpriteRenderer>();
        if (n != null)
        {
            n.sprite = sprite;
        }
        return g;
    }

    private void BoldMenuItem(GameObject[] menu, int i, int stringIndex)
    {
        if(stringIndex < 0 || (stringIndex + 9) >= this.Strings.Length) { return; }
        SpriteRenderer renderer = menu[i].GetComponent<SpriteRenderer>();
        renderer.sprite = this.Strings[stringIndex + 9];
    }

    private void UnboldMenuItem(GameObject[] menu, int i, int stringIndex)
    {
        if (stringIndex < 0 || stringIndex >= this.Strings.Length) { return; }
        SpriteRenderer renderer = menu[i].GetComponent<SpriteRenderer>();
        renderer.sprite = this.Strings[stringIndex];
    }
}
