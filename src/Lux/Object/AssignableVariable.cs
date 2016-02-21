namespace Lux
{
    public class AssignableVariable<T>
    {
        private T _value;

        public AssignableVariable()
        {
            
        }

        public AssignableVariable(T value)
        {
            Value = value;
        } 


        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Assigned = true;
            }
        }

        public bool Assigned { get; set; }
    }
}
