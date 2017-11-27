using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class Menu : MonoBehaviour
    {

        private Sprite[] Strings;
        private GameObject[,] MainMenu = new GameObject[5, 2];
        private GameObject[,] OptionsMenu = new GameObject[3, 2];
        private GameObject[,] NewGameMenu = new GameObject[2, 2];

        private int SelectedOption = 0;
        private bool FreshMenu = false;

        private Action Handler;

        void Start()
        {
            this.Strings = Resources.LoadAll<Sprite>("menu_strings");

            this.CreateMenus();
            this.ShowMenu(this.MainMenu);
            this.Handler = new Action(this.MainMenuHandler);
        }

        void Update()
        {
            this.Handler();
        }

        private void MainMenuHandler()
        {
            int previousSelection = this.SelectedOption;

            if (Input.GetKeyDown(KeyCode.UpArrow)) { this.SelectedOption -= 1; }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { this.SelectedOption += 1; }
            this.SelectedOption = Mathf.Clamp(this.SelectedOption, 0, this.MainMenu.GetLength(0) - 1);

            if (this.SelectedOption != previousSelection || this.FreshMenu)
            {
                this.FreshMenu = false;
                this.UnboldMenuItem(this.MainMenu, previousSelection);
                this.BoldMenuItem(this.MainMenu, this.SelectedOption);
            }

            if (this.SelectedOption == 0 && Input.GetKeyDown(KeyCode.Return))
            {
                this.HideMenu(this.MainMenu);
                this.ShowMenu(this.NewGameMenu);
                this.Handler = this.NewGameMenuHandler;
            }

            if (this.SelectedOption == 1 && Input.GetKeyDown(KeyCode.Return))
            {
                Tino.Save.SaveLoadGame.LoadGame();
                UnityEngine.SceneManagement.SceneManager.LoadScene(Tino.Save.SaveLoadGame.SavedGame.CurrentScene);
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
            this.SelectedOption = Mathf.Clamp(this.SelectedOption, 0, this.OptionsMenu.GetLength(0) - 1);

            if (this.SelectedOption != previousSelection || this.FreshMenu)
            {
                this.FreshMenu = false;
                this.UnboldMenuItem(this.OptionsMenu, previousSelection);
                this.BoldMenuItem(this.OptionsMenu, this.SelectedOption);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.HideMenu(this.OptionsMenu);
                this.ShowMenu(this.MainMenu);
                this.Handler = this.MainMenuHandler;
            }
        }

        private void NewGameMenuHandler()
        {
            int previousSelection = this.SelectedOption;
            if (Input.GetKeyDown(KeyCode.UpArrow)) { this.SelectedOption -= 1; }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { this.SelectedOption += 1; }
            this.SelectedOption = Mathf.Clamp(this.SelectedOption, 0, this.NewGameMenu.GetLength(0) - 1);

            if (this.SelectedOption != previousSelection || this.FreshMenu)
            {
                this.FreshMenu = false;
                this.UnboldMenuItem(this.NewGameMenu, previousSelection);
                this.BoldMenuItem(this.NewGameMenu, this.SelectedOption);
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (this.SelectedOption == 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("CharacterTest");
                }
                else if (this.SelectedOption == 1)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("ceo_grass");
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.HideMenu(this.NewGameMenu);
                this.ShowMenu(this.MainMenu);
                this.Handler = this.MainMenuHandler;
            }
        }

        private void CreateMenus()
        {
            this.MainMenu[0, 0] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[0]);
            this.MainMenu[0, 1] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[9]);
            this.MainMenu[1, 0] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[1]);
            this.MainMenu[1, 1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[10]);
            this.MainMenu[2, 0] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[2]);
            this.MainMenu[2, 1] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[11]);
            this.MainMenu[3, 0] = this.CreateMenuItem(new Vector3(0, -100, 0), this.Strings[3]);
            this.MainMenu[3, 1] = this.CreateMenuItem(new Vector3(0, -100, 0), this.Strings[12]);
            this.MainMenu[4, 0] = this.CreateMenuItem(new Vector3(0, -200, 0), this.Strings[4]);
            this.MainMenu[4, 1] = this.CreateMenuItem(new Vector3(0, -200, 0), this.Strings[13]);
            this.HideMenu(this.MainMenu);
            this.OptionsMenu[0, 0] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[6]);
            this.OptionsMenu[0, 1] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[15]);
            this.OptionsMenu[1, 0] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[7]);
            this.OptionsMenu[1, 1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[16]);
            this.OptionsMenu[2, 0] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[8]);
            this.OptionsMenu[2, 1] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[17]);
            this.HideMenu(this.OptionsMenu);
            this.NewGameMenu[0, 0] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[18]);
            this.NewGameMenu[0, 1] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[21]);
            this.NewGameMenu[1, 0] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[19]);
            this.NewGameMenu[1, 1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[22]);
            this.HideMenu(this.NewGameMenu);
        }

        private void ShowMenu(GameObject[,] menu, int option = 0)
        {
            this.FreshMenu = true;
            this.SelectedOption = option;
            for (int i = 0; i < menu.GetLength(0); i++)
            {
                this.UnboldMenuItem(menu, i);
            }
        }

        private void HideMenu(GameObject[,] menu)
        {
            for (int i = 0; i < menu.GetLength(0); i++)
            {
                menu[i, 0].SetActive(false);
                menu[i, 1].SetActive(false);
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

        private void BoldMenuItem(GameObject[,] menu, int i)
        {
            menu[i, 0].SetActive(false);
            menu[i, 1].SetActive(true);
        }

        private void UnboldMenuItem(GameObject[,] menu, int i)
        {
            menu[i, 0].SetActive(true);
            menu[i, 1].SetActive(false);
        }
    }
}