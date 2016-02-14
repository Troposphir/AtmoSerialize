using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AtmoSerialize {
    public class TriggerProperty : IAtmoProperty {
        private const string AndChar = "&";
        private const string OrChar = "|";
        private const string NotChar = "!";
        private static readonly Regex ConditionRegex = new Regex(@"
            \((?<x>-?\d+),(?<y>-?\d+),(?<z>-?\d+)\).
            (?<layer>\d+).
            (?<type>[a-zA-Z_][a-zA-Z0-9_]+).
            (?<property>[a-zA-Z_][a-zA-Z0-9_]+)",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled
        );

        public bool Negated { get; set; }
        public Operator Operator { get; set; }
        public IList<TriggerCondition> Conditions { get; }

        public TriggerProperty(string raw) {
            if (raw.StartsWith(NotChar)) {
                Negated = true;
                raw = raw.Substring(1);
            }
            var conditions = raw
                .Split(new [] { AndChar, OrChar }, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => {
                    var match = ConditionRegex.Match(c);
                    if (!match.Success) return null;
                    var x = float.Parse(match.Groups["x"].Value);
                    var y = float.Parse(match.Groups["y"].Value);
                    var z = float.Parse(match.Groups["z"].Value);
                    return new TriggerCondition {
                        CellPosition = new Vector(x, y, z),
                        Layer = int.Parse(match.Groups["layer"].Value),
                        Type = match.Groups["type"].Value,
                        Property = match.Groups["property"].Value
                    };
                })
                .Where(tc => tc != null);
            
            Conditions = conditions.ToList();
        }

        public string ToRaw() {
            var separator = Operator == Operator.And ? AndChar : OrChar;
            return string.Join(separator, Conditions.Select(condition => {
                var position = $"({condition.CellPosition.X:G},{condition.CellPosition.Y:G},{condition.CellPosition.Z:G}).";
                var trigger = $"{condition.Layer}.{condition.Type}.{condition.Property}";
                return position + trigger;
            }));
        }
    }
}