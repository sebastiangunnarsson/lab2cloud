using System;

namespace Lab2.Controllers.V1
{
    public class VacationDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Place { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}