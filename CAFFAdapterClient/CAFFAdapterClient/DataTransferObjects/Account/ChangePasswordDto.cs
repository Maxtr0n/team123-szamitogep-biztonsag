namespace CAFFAdapterClient.DataTransferObjects.Account
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}