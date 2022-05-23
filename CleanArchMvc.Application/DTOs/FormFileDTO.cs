namespace CleanArchMvc.Application.DTOs
{
    public class FormFileDTO
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
    }
}
