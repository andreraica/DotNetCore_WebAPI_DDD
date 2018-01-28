namespace Infrastructure.CrossCutting.Interfaces
{
    public interface IJWTTokenDecrypt
    {
        string Get(string token, string item);
    }
}