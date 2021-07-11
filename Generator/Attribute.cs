using System;

namespace SourceGenerator.NotifyPropertyChanged
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : Attribute
    {
        #region Constructors

        public NotifyPropertyChangedAttribute(string? name = null) 
            => Name = name;

        #endregion


        #region Properties

        public string? Name { get; }
        
        public bool IsVirtual { get; set; } = false;

        public bool CacheEvent { get; set; } = false;
        
        #endregion
    }
}
