namespace Lab2.Models
{
    public class Sound : Media
    {
        public double Duration { get; set; }
        public string Container { get; set; }
        public string Codec { get; set; }
        public double Bitrate { get; set; }
        public int Channels { get; set; }
        public string SamplingRate { get; set; }
    }
}