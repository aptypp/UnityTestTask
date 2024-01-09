using System;
using UnityEngine;

namespace TeskTask
{
    [CreateAssetMenu(fileName = "Wave", menuName = "Data/Waves"), Serializable]
    public class Wave : ScriptableObject
    {
        public GameObject[] Characters;
    }
}