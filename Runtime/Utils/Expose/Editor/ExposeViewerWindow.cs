using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LuckiusDev.Utils.Expose.Editor
{
    public class ExposeViewerWindow : EditorWindow
    {
        private Vector2 scroll;

        private List<ExposedNode> roots = new();

        private Dictionary<int, bool> foldouts = new();

        [MenuItem("Tools/Expose Viewer")]
        public static void Open()
        {
            GetWindow<ExposeViewerWindow>("Expose Viewer");
        }

        private void OnSelectionChange()
        {
            Rebuild();
            Repaint();
        }

        private void OnHierarchyChange()
        {
            Rebuild();
        }

        private void Update()
        {
            Repaint(); // realtime
        }

        private void Rebuild()
        {
            roots.Clear();

            var go = Selection.activeGameObject;
            if (go == null) return;

            foreach (var comp in go.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (comp == null) continue;

                var root = ExposeTreeBuilder.Build(comp, comp.GetType().Name);
                if (root != null)
                    roots.Add(root);
            }
        }

        private void OnGUI()
        {
            if (roots.Count == 0)
            {
                EditorGUILayout.LabelField("No exposed data.");
                return;
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);

            foreach (var root in roots)
            {
                DrawNode(root, 0);
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawNode(ExposedNode node, int indent)
        {
            int id = node.GetHashCode();

            EditorGUI.indentLevel = indent;

            if (!node.IsLeaf)
            {
                if (!foldouts.ContainsKey(id))
                    foldouts[id] = false;

                foldouts[id] = EditorGUILayout.Foldout(foldouts[id], node.Name, true);

                if (foldouts[id])
                {
                    foreach (var child in node.Children)
                    {
                        DrawNode(child, indent + 1);
                    }
                }
            }
            else
            {
                DrawLeaf(node);
            }
        }

        private void DrawLeaf(ExposedNode node)
        {
            object value = node.Value;

            string display = FormatValue(value);

            EditorGUILayout.LabelField(node.Name, display);
        }

        private string FormatValue(object value)
        {
            if (value == null) return "null";

            switch (value)
            {
                case float f: return f.ToString("0.###");
                case Vector3 v: return v.ToString("F2");
                case IList list: return $"List (Count: {list.Count})";
                default: return value.ToString();
            }
        }
    }
}