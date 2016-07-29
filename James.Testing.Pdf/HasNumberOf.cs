using Spire.Pdf;

namespace James.Testing.Pdf
{
    internal class HasNumberOf : IHasNumberOf
    {
        private readonly int _number;
        private readonly Content _content;

        internal HasNumberOf(Content content, int number)
        {
            _number = number;
            _content = content;
        }

        public bool Pages()
        {
            using (var document = new PdfDocument(_content.Buffer))
            {
                return document.Pages.Count == _number;
            }
        }
    }
}