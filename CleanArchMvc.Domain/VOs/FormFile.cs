using System.IO;

namespace CleanArchMvc.Domain.VOs
{
    public class FormFile
    {
        public FormFile(Stream fileContent, string name, string fileName, long length, string contentType)
        {
            FileContent = fileContent;
            Name = name;
            FileName = fileName;
            Length = length;
            ContentType = contentType;
        }

        public string ContentType { get; }

        public Stream FileContent { get; }

        public string Name { get; }

        public string FileName { get; }

        public long Length { get; }
    }
}
