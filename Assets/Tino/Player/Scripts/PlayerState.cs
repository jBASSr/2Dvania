using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tino
{
    public static class PlayerState 
    {
        public static int Health;
        public static int MaxHealth;
        public static int ExtraJumps;

        static PlayerState()
        {
        }
    }

    public static class WorldState
    {
        public static string ComingFromDoor;
        public static string ComingFromScene;

        //Map scene + door to a new scene + door
        public static Dictionary<SceneDoorTuple, SceneDoorTuple> NewScenePosition;
        //Keeps track of which items in a scene are available.
        //i.e. If you pick up the max. hp item, you don't want it to come back next time you load the game.
        public static Dictionary<SceneItemTuple, bool> SceneItemState;

        static WorldState()
        {
            WorldState.NewScenePosition = new Dictionary<SceneDoorTuple, SceneDoorTuple>()
            {
                { new Tino.SceneDoorTuple("Tino", "Door 1"), new SceneDoorTuple("Tino2", "Door 1")},
                { new Tino.SceneDoorTuple("Tino", "Door 2"), new SceneDoorTuple("Tino2", "Door 2")},

                { new Tino.SceneDoorTuple("Tino2", "Door 1"), new SceneDoorTuple("Tino", "Door 1")},
                { new Tino.SceneDoorTuple("Tino2", "Door 2"), new SceneDoorTuple("Tino", "Door 2")}
            };

            WorldState.SceneItemState = new Dictionary<SceneItemTuple, bool>()
            {
                { new Tino.SceneItemTuple("Tino", "HPRefill 1"), true },
                { new Tino.SceneItemTuple("Tino", "HPMaxUp 1"), true },
                { new Tino.SceneItemTuple("Tino", "DoubleJump 1"), true },
                { new Tino.SceneItemTuple("Tino2", "HPRefill 1"), true },
                { new Tino.SceneItemTuple("Tino2", "HPMaxUp 1"), true },
                { new Tino.SceneItemTuple("Tino2", "DoubleJump 1"), true }
            };
        }

        public static string GetDoorName()
        {
            SceneDoorTuple s = new SceneDoorTuple(WorldState.ComingFromScene, WorldState.ComingFromDoor);
            if(WorldState.NewScenePosition.ContainsKey(s))
            {
                return WorldState.NewScenePosition[s].Door;
            }
            else
            {
                return null;
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
            if(this.Scene == null || this.Door == null) { return 0; }
            return this.Scene.GetHashCode() ^ this.Door.GetHashCode();
        }
    }

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
            if(this.Scene == null || this.Item == null) { return 0; }
            return this.Scene.GetHashCode() ^ this.Item.GetHashCode();
        }
    }
}