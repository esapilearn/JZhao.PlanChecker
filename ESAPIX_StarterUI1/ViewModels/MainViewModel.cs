using ESAPIX.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESAPIX.Common;
using VMS.TPS.Common.Model.API;
using Prism.Commands;
using System.Windows;
using ESAPIX.Extensions;
using ESAPIX.Constraints.DVH;
using System.Collections.ObjectModel;

namespace ESAPX_StarterUI.ViewModels
{
    public class MainViewModel : BindableBase
    {
        AppComThread VMS = AppComThread.Instance;

        public MainViewModel()
        {
            EvaluateCommand = new DelegateCommand(() => //defines what the button click does
            {
                foreach (var pc in Constraints)
                {
                    var result = VMS.GetValue(sc => //when calling this object, it does it on a background thread (frees up the primary processing thread)
                    {
                        //Check if we can constrain first, method CanConstrain checks to see if applying a constraint is valid
                        var canConstrain = pc.Constraint.CanConstrain(sc.PlanSetup);
                        //If not..report why
                        if (!canConstrain.IsSuccess) { return canConstrain; }
                        else
                        {
                            //Can constrain - so do it, method Constrain applies the rule
                            return pc.Constraint.Constrain(sc.PlanSetup);
                        }
                    });
                    //Update UI
                    pc.Result = result;
                }
            });

            CreateConstraints(); //method CreateConstraints creates the constraints...
        }

        private void CreateConstraints()
        {
            Constraints.AddRange(new PlanConstraint[]
            //plan constraint class will hold the actual constraint AND the result, change the strings here in order to change the constraints for different treatment sites
            {
                new PlanConstraint(ConstraintBuilder.Build("PTV", "Max[%] <= 110")),
                new PlanConstraint(ConstraintBuilder.Build("Rectum", "V75Gy[%] <= 15")),
                new PlanConstraint(ConstraintBuilder.Build("Rectum", "V65Gy[%] <= 35")),
                new PlanConstraint(ConstraintBuilder.Build("Bladder", "V80Gy[%] <= 15")),
                new PlanConstraint(new CTDateConstraint())
            });
        }


        public DelegateCommand EvaluateCommand { get; set; } //this is the button relay
        public ObservableCollection<PlanConstraint> Constraints { get; private set; } = new ObservableCollection<PlanConstraint>(); //this is the list of constraints
    }
}
