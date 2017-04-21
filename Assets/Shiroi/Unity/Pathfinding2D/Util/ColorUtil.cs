using UnityEngine;

namespace Shiroi.Unity.Pathfinding2D.Util {
    public static class ColorUtil {
        public static Color FromHex(string hex, byte a = 255) {
            var r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }

        public static Color Random(byte a = 255) {
            return new Color(1 - UnityEngine.Random.value, 1 - UnityEngine.Random.value, 1 - UnityEngine.Random.value,
                a);
        }

        public static Color Invert(this Color color, byte a = 255) {
            var r = 1 - color.r / 255;
            var g = 1 - color.g / 255;
            var b = 1 - color.b / 255;
            return new Color(r, g, b, a);
        }
    }
}