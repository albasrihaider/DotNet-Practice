﻿

namespace RunGroopWebApp.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; }
        public int? Milage { get; set; }
        public string? ProfileImageUrl{ get; set; }
        public IFormFile? Image { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

    }
}
