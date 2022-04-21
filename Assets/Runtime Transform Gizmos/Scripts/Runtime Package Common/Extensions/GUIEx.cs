using UnityEngine;
using System.Collections.Generic;

namespace RTG
{
    public static class GUIEx
    {
        private static Stack<Color> _colorStack = new Stack<Color>();

        public static void PushColor(Color color)
        {
            _colorStack.Push(UnityEngine.GUI.color);
            UnityEngine.GUI.color = color;
        }

        public static void PopColor()
        {
            if (_colorStack.Count > 0) UnityEngine.GUI.color = _colorStack.Pop();
        }
    }
}
