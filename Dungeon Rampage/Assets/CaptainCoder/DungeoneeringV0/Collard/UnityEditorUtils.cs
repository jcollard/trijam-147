#if UNITY_EDITOR
namespace CaptainCoder
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class UnityEditorUtils
    {
        private static readonly Dictionary<Color, GUIStyle> ColorLabels = new Dictionary<Color, GUIStyle>();
        private static GUIStyle _MonoSpacedTextArea;

        public static GUIStyle GetColorLabel(Color color)
        {
            if (!ColorLabels.TryGetValue(color, out GUIStyle style))
            {
                style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = color;
            }

            return style;
        }

        public static GUIStyle MonoSpacedTextArea
        {
            get
            {
                if (_MonoSpacedTextArea == null)
                {
                    _MonoSpacedTextArea = new GUIStyle(EditorStyles.textArea);
                    Font f = Resources.Load<Font>("Fonts/MonoSpaced");
                    if (f == null)
                    {
                        Debug.LogWarning("Attempted to load font from 'Resources/Fonts/MonoSpaced' but no file was found. Defaulting to system font.");
                    }
                    else
                    {
                        _MonoSpacedTextArea.font = f;
                        _MonoSpacedTextArea.fontSize = 18;
                    }
                }
                return _MonoSpacedTextArea;
            }
        }
    }
}
#endif