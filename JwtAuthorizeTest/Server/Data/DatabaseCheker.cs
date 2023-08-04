using Microsoft.EntityFrameworkCore;

namespace JwtAuthorizeTest.Server.Data;

public class DatabaseCheker
{
    private  readonly ApplicationDbContext _context;

    public DatabaseCheker(ApplicationDbContext context)
    {
        _context = context;
    }


    public  async Task Checkdatabase()
    {
       bool canconnect =await  _context.Database.CanConnectAsync();
        if (!canconnect)
        {
           await _context.Database.MigrateAsync();
        }
    }
}
