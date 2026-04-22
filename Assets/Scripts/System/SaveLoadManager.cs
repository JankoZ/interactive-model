using System.IO;
using Objects;
using UI;
using UnityEngine;

namespace System
{
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        
        private string _path;

        private void Awake() => _path = Path.Combine(Application.dataPath, "scene_save.json");

        public void SaveScene()
        {
            SceneSaveData data = new();

            foreach (var obj in uiManager.GetObjects())
            {
                Color color = obj.GetComponent<Renderer>().material.color;
                ObjectSaveData objData = new ObjectSaveData
                {
                    Name = obj.gameObject.name,
                    IsSelected = obj.isSelected,
                    IsVisible = obj.IsVisible(),
                    R = color.r,
                    G = color.g,
                    B = color.b,
                    A = color.a
                };
                data.Objects.Add(objData);
            }
            
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_path, json);
            Debug.Log("Saved " + _path);
        }

        public void LoadScene()
        {
            if (!File.Exists(_path))
            {
                Debug.LogWarning("File not found!");
                return;
            }
            
            string json = File.ReadAllText(_path);
            SceneSaveData data = JsonUtility.FromJson<SceneSaveData>(json);

            foreach (var obj in data.Objects)
            {
                var sceneObj =
                    uiManager.GetObjects().Find(o => o.gameObject.name == obj.Name);

                if (!sceneObj) continue;
                
                sceneObj.isSelected = obj.IsSelected;
                sceneObj.SetVisibility(obj.IsVisible);
                sceneObj.SetColor(new Color(obj.R, obj.G, obj.B, obj.A));
                sceneObj.SetTransparency(obj.A);
            }
            
            uiManager.RefreshAllUIObjects();
            Debug.Log("Loaded " + _path);
        }
    }
}
