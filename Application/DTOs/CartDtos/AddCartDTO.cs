using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CartDtos
{
    public class AddCartDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }
    }
}
