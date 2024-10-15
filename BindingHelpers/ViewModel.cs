using System.ComponentModel;
using HealthApp.Database;

namespace HealthApp.BindingHelpers
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _greeting;
        private string _water;
        private string _bmi;
        private string _weight;
        private string _height;
        private string _target;

        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel()
        {
            UpdateData();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateData()
        {
            Greeting = DataForBindings.GetGreetingMessage();
            Name = DataForBindings.GetUserName();
            Water = DataForBindings.GetWaterValue();
            BMI = DataForBindings.GetBMIValue();
            Weight = DataForBindings.GetWeightValue();
            Height = DataForBindings.GetHeightValue();
            Target = DataForBindings.GetTargetValue();
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Greeting
        {
            get => _greeting;
            set
            {
                _greeting = value;
                OnPropertyChanged(nameof(Greeting));
            }
        }

        public string Water
        {
            get => _water;
            set
            {
                _water = value;
                OnPropertyChanged(nameof(Water));
            }
        }

        public string BMI
        {
            get => _bmi;
            set
            {
                _bmi = value;
                OnPropertyChanged(nameof(BMI));
            }
        }

        public string Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public string Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public string Target
        {
            get => _target;
            set
            {
                _target = value;
                OnPropertyChanged(nameof(Target));
            }
        }
    }

}
