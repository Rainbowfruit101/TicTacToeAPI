using Models.Enums;

namespace Models;

public class PlayerModel: IHasId
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<SessionModel> Sessions { get; set; }
}