using System;

namespace James.Testing.UnitTests
{
    public abstract class BaseTest
    {
        protected void Gulp(Action action)
        {
            try
            {
                action();
            }
            catch (Exception)
            {
            }
        }
    }
}
