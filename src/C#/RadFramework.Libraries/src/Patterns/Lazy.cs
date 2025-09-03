namespace RadFramework.Libraries.DataTypes
{
    internal class Lazy<TValue> where TValue : class
    {
        private object threadLock = new();
        private TValue defaultValue = default(TValue);
        private Func<TValue> factory;
        private TValue value;

        public TValue Value
        {
            get
            {
                if (value != defaultValue)
                {
                    return value;
                }

                lock (threadLock)
                {
                    // if another thread created the value
                    if (value != defaultValue)
                    {
                        return value;
                    }

                    // run factory delegate
                    value = factory();

                    return value;
                }
            }
        }

        public Lazy(Func<TValue> factory)
        {
            this.factory = factory;
        }

        public Lazy(TValue value)
        {
            this.value = value;
        }
    }
}
