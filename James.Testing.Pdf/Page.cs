using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace James.Testing.Pdf
{
    public class Page : IPage
    {
        private readonly Content _content;
        private readonly int _number;

        internal Page(Content content, int number)
        {
            _content = content;
            _number = number;
        }

        public bool Contains(string text)
        {
            return Text().Contains(text);
        }

        public bool Contains(int number)
        {
            return Text().Contains(number.ToString());
        }

        public bool Contains(double number)
        {
            return Text().Contains(number.ToString());
        }

        private string _pageText;

        public string Text()
        {
            if (_pageText != null) 
                return _pageText;
            
            using (var pdfReader = new PdfReader(_content.Buffer))
            {
                var strategy = new SimpleTextExtractionStrategy();
                var pageText = PdfTextExtractor.GetTextFromPage(pdfReader, _number, strategy);
                pageText =
                    Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8,
                        Encoding.Default.GetBytes(pageText)));

                _pageText = pageText;
            }

            return _pageText;
        }
    }
}