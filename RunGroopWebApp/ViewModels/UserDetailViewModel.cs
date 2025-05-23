﻿namespace RunGroopWebApp.ViewModels
{
    internal class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; }
        public int? Milage { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}