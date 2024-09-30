using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.PackageManager.UI;

namespace Game.Editor
{
    /// <summary>
    /// MVP 积己 磊悼拳
    /// </summary>
    public class CreateMVP : EditorWindow
    {
        string _scriptName = "";
        [MenuItem("Custom/Create/UI_MVP")]
        private static void ShowWindow() {
           CreateMVP window  = GetWindow<CreateMVP>("MVP Generater");
            window.minSize = new Vector2(330, 400);
            window.maxSize = new Vector2(330, 400);
            
        }

        private void OnGUI() {
            // GUI
            GUILayout.BeginHorizontal("box");
            GUILayout.Label("UI MVP", EditorStyles.boldLabel, GUILayout.Width(60f));
            _scriptName = EditorGUILayout.TextField("", _scriptName, GUILayout.Width(150f));
            if (GUILayout.Button("Generate", GUILayout.Width(80f)) && !string.IsNullOrEmpty(_scriptName)) {
                GenerateUIMVP(_scriptName);
                Close();
            }
            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// UI MVP 颇老 积己
        /// </summary>
        /// <param name="fileName"></param>
        private void GenerateUIMVP(string fileName) {
            Debug.Log(fileName);
            string path = Application.dataPath + "/02.Scripts/Mono/UI/";
            string viewPath = path + "View/" + fileName +"_UI_View.cs";
            string prePath = path + "Presenter/" + fileName + "_UI_Presenter.cs"; ;
            string modelPath = path + "Model/" + fileName + "_UI_Model.cs"; ;

            string vText;
            string pText;
            string mText;

            string frontText = 
@"//MVP Generater
using UnityEngine;
namespace Game.Mono.UI
{
";
            string endText = 
@"

    }
}" + "\r\n";
            vText = frontText + string.Format("\tpublic class {0}_UI_View : UI_View<{0}_UI_Presenter>, IView\r\n\t{{", fileName) + endText;
            pText = frontText + string.Format("\tpublic class {0}_UI_Presenter : UI_Presenter<{0}_UI_View, {0}_UI_Model>, IPresenter\r\n\t{{\t// internal", fileName) + endText;
            mText = frontText + string.Format("\tpublic class {0}_UI_Model : UI_Model<{0}_UI_Presenter>, IModel\r\n\t{{\t// internal", fileName) + endText;

            File.WriteAllText(viewPath, vText);
            File.WriteAllText(prePath, pText);
            File.WriteAllText(modelPath, mText);
           
            AssetDatabase.Refresh();
        }
    }
}
