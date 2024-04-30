using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Ui
{
    public static class UiUtil
    {
        public static void UpdateText(TextMeshProUGUI textComponent, string newText)
        {
            textComponent.text = newText;
        }
        
        public static Vector3 ConvertPositionToCanvas(Vector3 objectPosition, Canvas canvas, Camera cam3d, Camera canvasCam)
        {
            Vector3 screenPosition = cam3d.WorldToScreenPoint(objectPosition);
        
            screenPosition.z = canvas.planeDistance;

            Vector3 canvasPosition = canvasCam.ScreenToWorldPoint(screenPosition);

            return canvasPosition;
        }

        public static string ConvertNumberToAbbreviatedFormat(int amount)
        {
            const int billionThreshold = 1000000000;
            const int millionThreshold = 1000000;
            const int kiloThreshold = 1000;

            string result;

            if (amount >= billionThreshold)
            {
                float value = (float)amount / billionThreshold;
                result = FormatNumber(value) + "B";
            }
            else if (amount >= millionThreshold)
            {
                float value = (float)amount / millionThreshold;
                result = FormatNumber(value) + "M";
            }
            else if (amount >= kiloThreshold)
            {
                float value = (float)amount / kiloThreshold;
                result = FormatNumber(value) + "K";
            }
            else
            {
                result = amount.ToString();
            }

            return result;
        }

        private static string FormatNumber(float value)
        {
            string formatted = value.ToString("0.##");
            if (formatted.EndsWith(".00"))
            {
                formatted = formatted.Substring(0, formatted.Length - 3);
            }
            return formatted;
        }
        
        public static int GetNumericPart(string value)
        {
            var chars = value.ToCharArray();
            var digitChars = chars.Where(char.IsDigit).ToList();
    
            // Check if there are any numeric digits in the input
            if (digitChars.Count == 0)
                throw new ArgumentException("The input string does not contain any numeric digits.");

            // Convert the numeric characters to their corresponding integer values and combine them
            int numericValue = 0;
            for (var i = 0; i < digitChars.Count; i++)
            {
                var digitChar = digitChars[i];
                numericValue = numericValue * 10 + (digitChar - '0');
            }

            return numericValue;
        }
        
        public static string FormatTime(int totalSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);
            return $"{timeSpan:mm\\:ss}";
        }
    }
}