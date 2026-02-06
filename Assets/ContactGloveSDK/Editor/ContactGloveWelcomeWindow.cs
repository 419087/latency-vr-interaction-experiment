using UnityEditor;
using UnityEngine;

namespace ContactGloveSDK.Editor
{
    public class ContactGloveWelcomeWindow : EditorWindow
    {
        private const string PrefKey = "ContactGloveSDK.Welcome.Hide";
        private const string DocsUrl = "https://docs.diver-x.jp/contact-glove-2-dev/cg2_unity-setup";
        private const string BannerAssetPath = "Assets/ContactGloveSDK/Editor/cg2.png";

        private bool dontShowAgain;
        private Texture2D bannerTexture;

        [InitializeOnLoadMethod]
        private static void ShowOnImport()
        {
            if (EditorPrefs.GetBool(PrefKey, false))
                return;

            EditorApplication.delayCall += () =>
            {
                // Avoid showing during compilation or play mode transitions
                if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
                    return;

                var window = GetWindow<ContactGloveWelcomeWindow>(true, "Contact Glove SDK", true);
                window.minSize = new Vector2(520, 450);
                window.Show();
            };
        }

        private void OnEnable()
        {
            dontShowAgain = EditorPrefs.GetBool(PrefKey, false);
            bannerTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(BannerAssetPath);
        }

        private void OnGUI()
        {
            GUILayout.Space(8);
            GUILayout.Label("Welcome / ようこそ", EditorStyles.boldLabel);
            GUILayout.Space(4);

            EditorGUILayout.HelpBox(
                "Thank you for importing Contact Glove SDK.\n" +
                "Contact Glove SDK をインポートいただきありがとうございます。",
                MessageType.Info);

            if (bannerTexture != null)
            {
                GUILayout.Space(6);
                var rect = GUILayoutUtility.GetRect(1, 250, GUILayout.ExpandWidth(true));
                float aspect = (float)bannerTexture.width / bannerTexture.height;
                float height = Mathf.Min(rect.height, rect.width / aspect);
                rect.height = height;
                GUI.DrawTexture(rect, bannerTexture, ScaleMode.ScaleToFit);
            }

            GUILayout.Space(8);
            GUILayout.Label("Documentation / ドキュメント", EditorStyles.boldLabel);
            GUILayout.Label(
                "Please follow the setup guide to get started.\n" +
                "セットアップ手順はこちらからご確認ください。",
                EditorStyles.wordWrappedLabel);

            GUILayout.Space(6);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Open Docs / ドキュメントを開く", GUILayout.Height(28)))
                {
                    Application.OpenURL(DocsUrl);
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                dontShowAgain = GUILayout.Toggle(dontShowAgain, "Don't show again / 次回以降表示しない");
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Close / 閉じる", GUILayout.Width(120), GUILayout.Height(24)))
                {
                    EditorPrefs.SetBool(PrefKey, dontShowAgain);
                    Close();
                }
            }

            if (GUI.changed)
            {
                EditorPrefs.SetBool(PrefKey, dontShowAgain);
            }
        }
    }
}
