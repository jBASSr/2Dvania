using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino.Save
{
    [System.Serializable]
    public class Game
    {
        public string CurrentScene;
        public Tino.Save.Vector3 PlayerPosition;
        
    }

    [System.Serializable]
    public struct Vector3
    {
        public float X, Y, Z;
        public Vector3(float x, float y, float z) { this.X = x; this.Y = y; this.Z = z; }
    };
}