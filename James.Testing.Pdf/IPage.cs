namespace James.Testing.Pdf
{
    public interface IPage
    {
        bool Contains(string text);
        bool Contains(int number);
        bool Contains(double number);
        string Text();
    }
}