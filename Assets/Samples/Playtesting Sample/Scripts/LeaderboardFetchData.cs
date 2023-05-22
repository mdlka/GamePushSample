using System;
using UnityEngine.Scripting;

namespace Agava.GamePush.Samples
{
    [Serializable]
    public class LeaderboardFetchData
    {
        [field: Preserve] public string avatar;
        [field: Preserve] public int id;
        [field: Preserve] public int score;
        [field: Preserve] public string name;
        [field: Preserve] public int position;
    }
}