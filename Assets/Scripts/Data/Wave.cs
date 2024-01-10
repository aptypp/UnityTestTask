using System;
using TestTask.Units;
using UnityEngine;

namespace TestTask.Data
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Data/Waves"), Serializable]
    public class Wave : ScriptableObject
    {
        public Enemie[] Characters;
    }
}