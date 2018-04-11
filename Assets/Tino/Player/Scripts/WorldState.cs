using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tino
{
    using D = Tino.SceneDoorTuple;
    using I = Tino.SceneItemTuple;

    public static class WorldState
    {
        public static Difficulty Difficulty = Difficulty.EASY;
        public static string ComingFromDoor;
        public static string ComingFromScene;

        //Map scene + door to a new scene + door
        public static Dictionary<D, D> NewScenePosition;
        //Keeps track of which items in a scene are available.
        //i.e. If you pick up the max. hp item, you don't want it to come back next time you load the game.
        public static Dictionary<I, bool> SceneItemState;

        static WorldState()
        {
            WorldState.NewScenePosition = new Dictionary<SceneDoorTuple, SceneDoorTuple>()
            {				
				{ new D("DesertLevel1", "Door_Cave"),  new D("cave", "Door1") }
				/*
				{ new D("cave", "Door 1"),  new D("DesertLevel1", "Door_Cave") },
				{ new D("DesertLevel1", "Door_Locked"),  new D("boss", "Door") },
				{ new D("boss", "DoorExit"),  new D("scenes1", "DoorEnter") },
				{ new D("scenes1", "Door2"),  new D("bad_robot", "Door1") },

				{  new D("DesertLevel2", "DoorDL2Enter"), new D("scenes1", "Door2") },
				{ new D("DesertLevel2", "Door2"),  new D("scenes1", "Door3") },
                { new D("Tino", "Door 1"),  new D("Tino2", "Door 1") },
                { new D("Tino", "Door 2"),  new D("Tino2", "Door 2") },
                { new D("Tino2", "Door 1"), new D("Tino", "Door 1") },
                { new D("Tino2", "Door 2"), new D("Tino", "Door 2") },
                { new D("Tino3", "MetroidDoor"), new D("Tino3", "MF_Doors_23") },
                { new D("Tino3", "MF_Doors_23"), new D("Tino3", "MetroidDoor") },
                { new D("scenes1", "MetroidDoor"), new D("scenes1", "MF_Doors_23") },
                { new D("scenes1", "MF_Doors_23"), new D("scenes1", "MetroidDoor") }
                */
            };

            WorldState.SceneItemState = new Dictionary<SceneItemTuple, bool>()
            {
                { new I("Tino", "HPRefill 1"), true },
                { new I("Tino", "HPMaxUp 1"), true },
                { new I("Tino", "DoubleJump 1"), true },
                { new I("Tino2", "HPRefill 1"), true },
                { new I("Tino2", "HPMaxUp 1"), true },
                { new I("Tino2", "DoubleJump 1"), true },
                { new I("Tino3", "DoubleJump"), true },
                { new I("Tino3", "HPMaxUp"), true },
                { new I("Tino4", "DoubleJump"), true },
                { new I("Tino4", "HPMaxUp"), true },
                 { new I("DesertLevel", "DoubleJump"), true },
                { new I("DesertLevel", "HPMaxUp"), true },
                  { new I("scenes1", "DoubleJump"), true },
                { new I("scenes1", "HPMaxUp"), true },
                 { new I("DesertLevel 2", "DoubleJump"), true },
                  { new I("DesertLevel 2", "DoubleJump1"), true },
                { new I("DesertLevel 2", "HPMaxUp"), true }
            };
        }

        public static string GetDoorName()
        {
            SceneDoorTuple s = new D(WorldState.ComingFromScene, WorldState.ComingFromDoor);
            if (WorldState.NewScenePosition.ContainsKey(s))
            {
                return WorldState.NewScenePosition[s].Door;
            }
            else
            {
                return null;
            }
        }

        public static string GetSceneName()
        {
            SceneDoorTuple s = new D(WorldState.ComingFromScene, WorldState.ComingFromDoor);
            if (WorldState.NewScenePosition.ContainsKey(s))
            {
                return WorldState.NewScenePosition[s].Scene;
            }
            else
            {
                return null;
            }
        }

        private static void ToggleItem(string sceneName, string itemName, bool on)
        {
            I key = new I(sceneName, itemName);
            if (WorldState.SceneItemState.ContainsKey(key))
            {
                WorldState.SceneItemState[new I(sceneName, itemName)] = on;
            }
        }

        public static void TurnOffItem(string sceneName, string itemName)
        {
            WorldState.ToggleItem(sceneName, itemName, false);
        }

        public static void TurnOnItem(string sceneName, string itemName)
        {
            WorldState.ToggleItem(sceneName, itemName, true);
        }

        public static bool IsItemOn(string sceneName, string itemName)
        {
            I key = new I(sceneName, itemName);
            if (WorldState.SceneItemState.ContainsKey(key))
            {
                return WorldState.SceneItemState[new I(sceneName, itemName)];
            }
            else
            {
                return false;
            }
        }
    }

    public struct SceneDoorTuple
    {
        public string Scene { get; private set; }
        public string Door { get; private set; }

        public SceneDoorTuple(string scene, string door)
        {
            this.Scene = scene;
            this.Door = door;
        }

        public override int GetHashCode()
        {
            if (this.Scene == null || this.Door == null) { return 0; }
            return this.Scene.GetHashCode() ^ this.Door.GetHashCode();
        }
    }

    [Serializable]
    public struct SceneItemTuple
    {
        public string Scene { get; private set; }
        public string Item { get; private set; }

        public SceneItemTuple(string scene, string item)
        {
            this.Scene = scene;
            this.Item = item;
        }

        public override int GetHashCode()
        {
            if (this.Scene == null || this.Item == null) { return 0; }
            return this.Scene.GetHashCode() ^ this.Item.GetHashCode();
        }
    }

    public enum Difficulty { EASY, MEDIUM, HARD };
}
