using ESAPIX.Constraints;
using ESAPIX.Extensions;
using System;
using VMS.TPS.Common.Model.API;

namespace ESAPX_StarterUI.ViewModels
{
    public class CTDateConstraint : IConstraint
    {
        public string Name => "CT Date Checker";
        public string FullName => "CT Date < 60 Days";

        public ConstraintResult CanConstrain(PlanningItem pi)
        {
            var pq = new PQAsserter(pi); //Plan Quality asserter is a helper function for the CanConstrain method
            return pq.HasImage().CumulativeResult; //HasImage() runs through a full list of properties that it checks and then attribute .CumulativeResult displays the combined result
        }

        public ConstraintResult Constrain(PlanningItem pi)
        {
            var image = pi.GetImage(); //equivalent ESAPI function is pi.StructureSet.Image, extra ESAPIX methods will appear as an icon with a small arrow next to it
            return Constrain(image.CreationDateTime); //take the image and pass it into a different method to evaluate CreationDateTime in order to make this step testable
        }

        public ConstraintResult Constrain(DateTime? creationDate)
        {
            var diffDays = (DateTime.Now - creationDate).Value.TotalDays;
            var msg = $"CT is {diffDays} days old";

            if (diffDays <= 60)
            {
                return new ConstraintResult(this, ResultType.PASSED, msg);
            }
            else
            {
                return new ConstraintResult(this, ResultType.ACTION_LEVEL_3, msg);
            }
        }
    }
}