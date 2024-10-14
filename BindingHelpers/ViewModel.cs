using System.ComponentModel;
using HealthApp.Database;

namespace HealthApp.BindingHelpers
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _greeting;
        private string _water;

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
    }

}
