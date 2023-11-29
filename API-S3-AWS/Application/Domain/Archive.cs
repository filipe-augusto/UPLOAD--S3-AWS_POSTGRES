namespace API_S3_AWS.Application.Domain
{
    public class Archive
    {

        public string Title { get; set; }
        public string Path { get; set; }
        public Archive()
        {

        }

        public Archive(string title, string path)
        {
            Title = title;
            Path = path;
        }
    }
}
