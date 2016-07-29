using System;
using System.IO;
using System.Text;
using System.Threading;

namespace James.Testing.Pdf
{
    public class Content : IContent
    {
        private static readonly ThreadLocal<IContent> CurrentContent = new ThreadLocal<IContent>();
        public byte[] Buffer { get; private set; }

        private Content(byte[] buffer)
        {
            Buffer = buffer;
            CurrentContent.Value = this;
        }

        public static IContent From(string path)
        {
            return new Content(File.ReadAllBytes(path));
        }

        public static IContent From(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException();

            if (buffer.Length < 5)
                throw new ArgumentException();

            return new Content(buffer);
        }

        public static IContent From(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException();

            return new Content(ReadFully(stream));
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static IContent Current()
        {
            return CurrentContent.Value;
        }

        private const string PdfHeaderFormat = "%PDF-{0}";

        public bool IsPdf()
        {
            var expectedHeader = string.Format(PdfHeaderFormat, string.Empty);
            return HasExpectedHeader(expectedHeader);
        }

        public bool IsPdf(double version)
        {
            var expectedHeader = string.Format(PdfHeaderFormat, version);
            return HasExpectedHeader(expectedHeader);
        }

        private bool HasExpectedHeader(string expectedHeader)
        {
            var header = Encoding.ASCII.GetString(Buffer);
            return header.StartsWith(expectedHeader);
        }

        public IHasNumberOf Has(int number)
        {
            return new HasNumberOf(this, number);
        }

        public IPage Page(int number)
        {
            return new Page(this, number);
        }
    }
}
