using System;
using System.ComponentModel;

namespace PdfRpt.Core.FunctionalTests.Models
{
    public class PersonnelReadModel
    {
        public string DisplayName { get; set; }

        public UserStaus? Status { get; set; }

        public PersonnelRoles? Roles { get; set; }
    }

    public enum UserStaus
    {
        [Description("فعال")]
        Active,

        [Description("غيرفعال")]
        Disabled
    }

    [Flags]
    public enum PersonnelRoles
    {
        [Description("هيچكدام")]
        None = 0,

        [Description("نگهبان")]
        Guard = 1 << 0,

        [Description("راننده")]
        Driver = 1 << 1,

        [Description("كارمند")]
        Employee = 1 << 3
    }
}