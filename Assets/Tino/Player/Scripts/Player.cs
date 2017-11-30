using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tino
{
    public class Player : MonoBehaviour
    {

        public void Awake()
        {
            if (Tino.Save.SaveLoadGame.SavedGame != null)
            {
                Tino.Save.Vector3 pos = Tino.Save.SaveLoadGame.SavedGame.PlayerPosition;
                this.gameObject.transform.position = new Vector3(pos.X, pos.Y, pos.Z);
            }
        }
    }
}