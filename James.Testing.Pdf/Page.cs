using Spire.Pdf;

namespace James.Testing.Pdf
{
    internal class Page : IPage
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

            using (var document = new PdfDocument(_content.Buffer))
            {
                var page = document.Pages[_number - 1];
                _pageText = page.ExtractText();
            }
            
            return _pageText;
        }
    }
}