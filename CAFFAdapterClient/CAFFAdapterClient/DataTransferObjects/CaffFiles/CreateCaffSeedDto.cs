namespace CAFFAdapterClient.DataTransferObjects.CaffFiles
{
    public class CreateCaffSeedDto
    {
        public int UserId { get; set; }
        public byte[] Caff { get; set; }
        public byte[] Gif { get; set; }
        public string Description { get; set; }
        public string Json { get; set; }
    }
}
