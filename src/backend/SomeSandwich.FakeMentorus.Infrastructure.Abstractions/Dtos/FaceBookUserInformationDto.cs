using Newtonsoft.Json;

namespace SomeSandwich.FakeMentorus.Infrastructure.Abstractions.Dtos;

/// <summary>
/// Facebook user information.
/// </summary>
public class FaceBookUserInformationDto
{
    /// <summary>
    /// Email address associated with the Facebook user.
    /// </summary>
    [JsonProperty(PropertyName = "email")]
    public string? Email { get; set; }
}
