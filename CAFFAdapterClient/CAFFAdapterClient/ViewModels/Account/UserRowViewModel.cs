﻿using CAFFAdapterClient.Domain.Enums;

namespace CAFFAdapterClient.ViewModels.Account
{
    public class UserRowViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserRoles Role { get; set; }
    }
}
