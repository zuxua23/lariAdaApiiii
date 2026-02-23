using System.ComponentModel.DataAnnotations;

namespace InventoryControl.DTO;

public class UserDto
{
    public string UserId { get; set; } = null!;      
    public string Fullname { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
public class UpdateUserDto
{
    public string UserId { get; set; } = null!;      
    public string Fullname { get; set; } = null!;
    public string Username { get; set; } = null!;
}
public class UserResponseDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Fullname { get; set; }
    public string Username { get; set; }
}

