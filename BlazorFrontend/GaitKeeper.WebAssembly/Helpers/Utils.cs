using GaitKeeper.WebAssembly.Models;
using Shared.DTOs;
using Shared.DTOs.RawGaitData;

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

        public static CreateGaitSessionDTO RawGaitDataToSaveGaitDataDTO(RawGaitSessionDTO raw, string description, string sex, int age)
        {
            var createDto = new CreateGaitSessionDTO
            {
                // Brugerdefinerede felter
                Description = description,
                Sex = sex,
                Age = age,

                // Raw data fra Python
                FileName = raw.FileName,
                SubjectId = raw.SubjectId,
                PointFreq = raw.PointFreq,
                AnalogFreq = raw.AnalogFreq,
                StartFrame = raw.StartFrame,
                EndFrame = raw.EndFrame,
                TotalFrames = raw.TotalFrames,

                AngleLabels = raw.AngleLabels,
                ForceLabels = raw.ForceLabels,
                ModeledLabels = raw.ModeledLabels,
                MomentLabels = raw.MomentLabels,
                PowerLabels = raw.PowerLabels,
                PointLabels = raw.PointLabels,

                Biometrics = raw.Biometrics,
                SystemInfo = raw.SystemInfo,
                LGaitCycles = raw.LGaitCycles,
                RGaitCycles = raw.RGaitCycles,
                GaitAnalyses = raw.GaitAnalyses
            };

            return createDto;
        }
    }
}
