using System.Text;

namespace CAFFAdapterClient.Framework
{
    public class GlobalConfigurations
    {
        public static byte[] SecurityKey = Encoding.UTF8.GetBytes("H8j3X+E/j7qNyb8Zu2c6udY/Ugg7W0+sZqcpkNyUTgMDcwT0I284KuXvdLXUZKSoo1C6qSyH4NjgFCPHE1aygtUrXvX7v6CCNfNjKJ8g409JwpOgLMwNnsZXH1NUnST6jeUiXbmc9Rmz2hUtO9m6cuZYKixiqtKidwPbRY9xSfR/6Vu1/FwGZtekkqihvo6k7ywIcBWYIsL4jy3s2oc/D0Nr+di+FS05VdOvwESGeQRdJVTuJD+tY+DgcD9rFW9WM2aQrZmiaYfuoAc1STU04ubyhTRKbqa7p/9jWsHJXmrTbg/iW8tY2AMdK0NRJnLnKDgGdTXLmwK+YWI1R68fVA==");
        public static bool IsDevelopment { get; set; } = true;
        public static int TokenExpiresDays { get; set; } = 7;
    }
}
