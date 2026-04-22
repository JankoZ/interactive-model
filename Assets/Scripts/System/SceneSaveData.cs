using System.Collections.Generic;

namespace System
{
    [Serializable]
    public class SceneSaveData
    {
        public List<ObjectSaveData> Objects = new();
    }
}
