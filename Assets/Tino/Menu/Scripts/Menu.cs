using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class Menu : MonoBehaviour
    {
        public string NewGameScene;

        private Sprite[] Strings;
        private GameObject[,] MainMenu = new GameObject[5, 2];
        private GameObject[,] OptionsMenu = new GameObject[2, 2];
        private GameObject[,] DifficultyMenu = new GameObject[3, 2];
        private GameObject[,] HealthMenu = new GameObject[5, 2];

        private int SelectedOption = 0;
        private bool FreshMenu = false;

        private Action Handler;

        void Start()
        {
            this.Strings = Resources.LoadAll<Sprite>("menu_strings");

            this.CreateMenus();
            this.ShowMenu(this.MainMenu);
            this.Handler = new Action(this.MainMenuHandler);

            Tino.PlayerState.StartingHealth = 30;
            Tino.WorldState.Difficulty = Difficulty.MEDIUM;
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
                UnityEngine.SceneManagement.SceneManager.LoadScene(this.NewGameScene);
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

            if (this.SelectedOption == 3 && Input.GetKeyDown(KeyCode.Return))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
            }

            if ((this.SelectedOption == 4 && Input.GetKeyDown(KeyCode.Return)) ||
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

            if (this.SelectedOption == 0 && Input.GetKeyDown(KeyCode.Return))
            {
                this.HideMenu(this.OptionsMenu);
                this.ShowMenu(this.DifficultyMenu);
                this.Handler = this.DifficultyMenuHandler;
            }
            
            if (this.SelectedOption == 1 && Input.GetKeyDown(KeyCode.Return))
            {
                this.HideMenu(this.OptionsMenu);
                this.ShowMenu(this.HealthMenu);
                this.Handler = this.HealthMenuHandler;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.HideMenu(this.OptionsMenu);
                this.ShowMenu(this.MainMenu);
                this.Handler = this.MainMenuHandler;
            }
        }
        
        private void DifficultyMenuHandler()
        {
            Difficulty[] difficultyIndex = { Difficulty.EASY, Difficulty.MEDIUM, Difficulty.HARD };

            int previousSelection = this.SelectedOption;

            if (Input.GetKeyDown(KeyCode.UpArrow)) { this.SelectedOption -= 1; }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { this.SelectedOption += 1; }
            this.SelectedOption = Mathf.Clamp(this.SelectedOption, 0, this.DifficultyMenu.GetLength(0) - 1);

            Tino.WorldState.Difficulty = difficultyIndex[this.SelectedOption];

            if (this.SelectedOption != previousSelection || this.FreshMenu)
            {
                this.FreshMenu = false;
                this.UnboldMenuItem(this.DifficultyMenu, previousSelection);
                this.BoldMenuItem(this.DifficultyMenu, this.SelectedOption);
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.HideMenu(this.DifficultyMenu);
                this.ShowMenu(this.OptionsMenu);
                this.Handler = this.OptionsMenuHandler;
            }
        }

        private void HealthMenuHandler()
        {
            int[] healthIndex = { 10, 20, 30, 40, 50 };

            int previousSelection = this.SelectedOption;

            if (Input.GetKeyDown(KeyCode.UpArrow)) { this.SelectedOption -= 1; }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) { this.SelectedOption += 1; }
            this.SelectedOption = Mathf.Clamp(this.SelectedOption, 0, this.HealthMenu.GetLength(0) - 1);

            Tino.PlayerState.StartingHealth = healthIndex[this.SelectedOption];

            if (this.SelectedOption != previousSelection || this.FreshMenu)
            {
                this.FreshMenu = false;
                this.UnboldMenuItem(this.HealthMenu, previousSelection);
                this.BoldMenuItem(this.HealthMenu, this.SelectedOption);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                this.HideMenu(this.HealthMenu);
                this.ShowMenu(this.OptionsMenu);
                this.Handler = this.OptionsMenuHandler;
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
            this.OptionsMenu[0, 1] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[7]);
            this.OptionsMenu[1, 0] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[20]);
            this.OptionsMenu[1, 1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[21]);
            this.HideMenu(this.OptionsMenu);
            this.DifficultyMenu[0, 0] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[8]);
            this.DifficultyMenu[0, 1] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[15]);
            this.DifficultyMenu[1, 0] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[16]);
            this.DifficultyMenu[1, 1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[17]);
            this.DifficultyMenu[2, 0] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[18]);
            this.DifficultyMenu[2, 1] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[19]);
            this.HideMenu(this.DifficultyMenu);
            this.HealthMenu[0, 0] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[22]);
            this.HealthMenu[0, 1] = this.CreateMenuItem(new Vector3(0, 200, 0), this.Strings[23]);
            this.HealthMenu[1, 0] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[24]);
            this.HealthMenu[1, 1] = this.CreateMenuItem(new Vector3(0, 100, 0), this.Strings[25]);
            this.HealthMenu[2, 0] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[26]);
            this.HealthMenu[2, 1] = this.CreateMenuItem(new Vector3(0, 0, 0), this.Strings[27]);
            this.HealthMenu[3, 0] = this.CreateMenuItem(new Vector3(0, -100, 0), this.Strings[28]);
            this.HealthMenu[3, 1] = this.CreateMenuItem(new Vector3(0, -100, 0), this.Strings[29]);
            this.HealthMenu[4, 0] = this.CreateMenuItem(new Vector3(0, -200, 0), this.Strings[30]);
            this.HealthMenu[4, 1] = this.CreateMenuItem(new Vector3(0, -200, 0), this.Strings[31]);
            this.HideMenu(this.HealthMenu);
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