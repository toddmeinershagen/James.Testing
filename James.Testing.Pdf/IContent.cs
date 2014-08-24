namespace James.Testing.Pdf
{
    public interface IContent
    {
        bool IsPdf();
        bool IsPdf(double version);
        IHasNumberOf Has(int number);
        IPage Page(int number);
    }
}