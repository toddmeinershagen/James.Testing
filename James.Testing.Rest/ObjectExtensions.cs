namespace James.Testing.Rest
{
    public static class ObjectExtensions
    {
        public static string GetProperty(this object self, string property)
        {
            var info = self.GetType().GetProperty(property);
            if (info == null) return null;
            return info.GetValue(self, null).ToString();
        }
    }
}