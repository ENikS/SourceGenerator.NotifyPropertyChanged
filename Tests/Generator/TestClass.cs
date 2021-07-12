using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Generator.UnitTests
{
    public partial class TestClass : INotifyPropertyChanged
    {
        private string _testString;

        public string TestString
        { 
            get => _testString;
            set 
            { 
                _testString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TestString)));
            }
        }

        #pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        #pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
