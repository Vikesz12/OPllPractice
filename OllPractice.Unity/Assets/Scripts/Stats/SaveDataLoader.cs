using Parser;
using UnityEngine;

namespace Stats
{
    public class SaveDataLoader : MonoBehaviour
    {
        private void Awake() => RotationEqualityTable.LoadData();
    }
}
