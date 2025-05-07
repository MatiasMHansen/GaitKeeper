using GaitKeeper.WebAssembly.Models;

namespace GaitKeeper.WebAssembly.Helpers
{
    public class Utils
    {
        public Utils()
        {
        }

        public static LabelCategories SeparateLabels(List<string> allLabels)
        {
            var categories = new LabelCategories();

            var modeledNames = new HashSet<string>
            {
                "LHJC", "RHJC", "LKJC", "RKJC", "LAJC", "RAJC",
                "LFJC", "RFJC", "LSJC", "RSJC", "LEJC", "REJC",
                "LHO", "RHO", "CentreOfMass"
            };

            foreach (var label in allLabels)
            {
                if (label.EndsWith("Angles"))
                    categories.AngleLabels.Add(label);
                else if (label.EndsWith("Force") || label.Contains("GroundReactionForce"))
                    categories.ForceLabels.Add(label);
                else if (label.EndsWith("Moment") || label.Contains("GroundReactionMoment"))
                    categories.MomentLabels.Add(label);
                else if (label.EndsWith("Power"))
                    categories.PowerLabels.Add(label);
                else if (modeledNames.Contains(label))
                    categories.ModeledLabels.Add(label);
                else
                    categories.PointLabels.Add(label); // Default to point labels
            }

            return categories;
        }
    }
}
