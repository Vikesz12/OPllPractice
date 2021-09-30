using Parser;
using UnityEngine;

namespace Stats
{
    public class SaveDataLoader : MonoBehaviour
    {
        private void Awake() => RubikStats.LoadData();
    }
}
