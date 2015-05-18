namespace Lab2.Models
{
    public class Video : Media
    {
        public double Duration { get; set; }
        public string Container { get; set; }
        public string VideoCodec { get; set; }
        public double VideoBitrate { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public double FrameRate { get; set; }
        public string AudioCodec { get; set; }
        public double AudioBitrate { get; set; }
        public int Channels { get; set; }
        public string SamplingRate { get; set; }
    }
}