using System;
using System.Globalization;
using System.Text;

namespace AtmoSerialize {
    public class TextProperty : IAtmoProperty {
        public string Text { get; set; }

        public TextProperty(string raw) {
            Text = Unescape(raw);
        }

        public string ToRaw() {
            return Escape(Text);
        }

        private static string Escape(string text) {
            var builder = new StringBuilder(text.Length);
            foreach (var character in text) {
                if (char.IsLetterOrDigit(character) || char.IsWhiteSpace(character)) {
                    builder.Append(character);
                } else {
                    builder.Append('/');
                    builder.Append(Convert.ToInt32(character).ToString("X"));
                }
            }
            return builder.ToString();
        }

        private static string Unescape(string text) {
            var builder = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; i++) {
                var current = text[i];
                if (current == '/') {
                    var sub = text.Substring(i + 1, 2);
                    current = Convert.ToChar(int.Parse(sub, NumberStyles.HexNumber));
                    i += 2;
                }
                builder.Append(current);
            }
            return builder.ToString();
        }
    }
}
