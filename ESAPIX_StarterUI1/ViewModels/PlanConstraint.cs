using ESAPIX.Constraints;
using Prism.Mvvm;

namespace ESAPX_StarterUI.ViewModels
{
    public class PlanConstraint : BindableBase //we use BindableBase to access SetProperty, it says when this SetProperty(value) changes, then I need to update the UI
    {
        public PlanConstraint(IConstraint con)
        {
            this.Constraint = con;
        }


        private IConstraint _constraint;

        public IConstraint Constraint
        {
            get { return _constraint; }
            set { SetProperty(ref _constraint, value); }
        }

        private ConstraintResult _result;

        public ConstraintResult Result
        {
            get { return _result; }
            set { SetProperty(ref _result, value); }
        }
    }
}