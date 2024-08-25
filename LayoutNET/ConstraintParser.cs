using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LayoutNET
{
    internal static class ConstraintParser
    {
        public static IEnumerable<LayoutConstraint> Parse(string inlineConstraints)
        {
            // TODO: Make this an actual parser, not this awful regex yuck...

            var split = inlineConstraints
                .Trim()
                .Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToArray();

            var regex = new Regex(
                @"
\[(?<property>\w+)\]
\s
(?<relationship>(eq|le|ge))
(\((?<strength>(weak|medium|strong|required))\))?
\s
\[(?<equation>.+)\]",
                RegexOptions.IgnorePatternWhitespace);

            foreach (var constraintString in split)
            {
                var match = regex.Match(constraintString);

                if (!match.Success)
                    throw new ArgumentException("Could not parse " + constraintString);

                var propertyString = match.Groups["property"].Value;
                var relationshipString = match.Groups["relationship"].Value;
                var equationString = match.Groups["equation"].Value;
                var strengthGroup = match.Groups["strength"];
                var strength = strengthGroup.Success
                    ? GetStrength(strengthGroup.Value)
                    : LayoutConstraintStrength.Required;

                var property = GetLayoutProperty(propertyString);

                var relationship = GetLayoutRelationship(relationshipString);

                List<LayoutLinearExpression> layoutLinearExpressions;
                double constant;
                ParseExpressions(
                    equationString,
                    out layoutLinearExpressions,
                    out constant);

                yield return new LayoutConstraint
                {
                    Constant = constant,
                    Expressions = layoutLinearExpressions,
                    Property = property,
                    Relationship = relationship,

                    Strength = strength,
                    Weight = 1d,
                };
            }
        }

        private static LayoutConstraintStrength GetStrength(string strengthString)
        {
            switch (strengthString)
            {
                case "weak":
                    return LayoutConstraintStrength.Weak;
                case "medium":
                    return LayoutConstraintStrength.Medium;
                case "strong":
                    return LayoutConstraintStrength.Strong;
                case "required":
                    return LayoutConstraintStrength.Required;
                default:
                    throw new Exception("Unknown strength " + strengthString);
            }
        }

        private static LayoutProperty GetLayoutProperty(string propertyString)
        {
            LayoutProperty property;
            if (Enum.TryParse(propertyString, out property)) 
                return property;
            throw new Exception("Unknown property " + propertyString);
        }

        private static LayoutRelationship GetLayoutRelationship(
            string relationshipString)
        {
            switch (relationshipString)
            {
                case "eq":
                    return LayoutRelationship.EqualTo;
                case "le":
                    return LayoutRelationship.LessThanOrEqualTo;
                case "ge":
                    return LayoutRelationship.GreaterThanOrEqualTo;
                default:
                    throw new Exception("Unknown relationship " + relationshipString);
            }
        }

        private static void ParseExpressions(
            string equationString,
            out List<LayoutLinearExpression> layoutLinearExpressions,
            out double constant)
        {
            var parts = equationString
                .Trim()
                .Split('+')
                .Select(s => s.Trim())
                .ToArray();

            var constRegex = new Regex(@"^-?[0-9\.]+$");
            var constTerms = parts
                .Where(p => constRegex.IsMatch(p))
                .ToList();
            var constants = constTerms
                .Select(double.Parse)
                .ToList();
            constant = constants.Aggregate(0d, (acc, v) => acc + v);

            var nonConstantTerms = parts
                .Where(p => !constRegex.IsMatch(p))
                .ToList();

            // e.g. -3.0*MainPanel.Left

            var exprRegex = new Regex(
                @"
((?<coefficient>(-?)[0-9\.]+)\s?\*)?
\s?
(?<element>[^\.]+)
\.
(?<property>\w+)",
                RegexOptions.IgnorePatternWhitespace);

            layoutLinearExpressions = nonConstantTerms
                .Select(term => exprRegex.Match(term))
                .Select(
                    m => new
                    {
                        Coefficient = m.Groups["coefficient"].Success
                            ? double.Parse(m.Groups["coefficient"].Value)
                            : 1d,
                        Element = m.Groups["element"].Value,
                        Property = GetLayoutProperty(m.Groups["property"].Value),
                    })
                .Select(
                    o => new LayoutLinearExpression
                    {
                        Multiplier = o.Coefficient,
                        ElementName = o.Element,
                        Property = o.Property,
                    })
                .ToList();
        }
    }
}
