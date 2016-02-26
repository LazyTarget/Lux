namespace Lux
{
    public class Assignable<T>
    {
        private T _value;

        public Assignable()
        {
            
        }

        public Assignable(T value)
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
