using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        [DataType(DataType.Password)]
        public string UserPass { get; set; } = "";

        [Range(0, 2, ErrorMessage = "Admin level must be between 0 (regular) and 2 (superadmin).")]
        public int Admin { get; set; } = 0; // 0 = regular, 1 = admin, 2 = superadmin
    }
}
