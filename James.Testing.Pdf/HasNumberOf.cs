using iTextSharp.text.pdf;

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
            using (var reader = new PdfReader(_content.Buffer))
            {
                return reader.NumberOfPages == _number;
            }
        }
    }
}