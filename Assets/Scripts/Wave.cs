using System;
using UnityEngine;

namespace TestTask
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Data/Waves"), Serializable]
    public class Wave : ScriptableObject
    {
        public GameObject[] Characters;
    }
}