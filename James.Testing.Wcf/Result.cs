namespace James.Testing.Wcf
{
    public struct Result
    {
        public static readonly Result Empty = new Result();

        public static bool operator ==(Result result1, Result result2)
        {
            return result1.Equals(result2);
        }

        public static bool operator !=(Result result1, Result result2)
        {
            return !result1.Equals(result2);
        }
    }
}