namespace Lyrics_Final_Proj.Models;

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime DateRegister { get; set; }


    //This function checks if this user exists in the database
    public int Login()
    {
        DBservices dbs = new DBservices();
        int numCheck = dbs.LoginUser(this);
        return numCheck;
    }

    //This function register a new user to the database
    public bool Register()
    {
        DBservices dbs = new DBservices();
        if (dbs.CheckUserExistEmail(this.Email) == 1) // if there is a user with this email will return 1, else return 0
        {
            throw new Exception("$email taken$");
        }
        else
        {
            if (dbs.CheckUserExistName(this.Name) == 1) // if there is a user with this email will return 1, else return 0
                throw new Exception("$Username taken$");
            else
            {
                dbs.InsertUser(this);
                return true;
            }
        }
    }

    //This function receives email and returns the user that belongs to it
    public static User ReadUserByEmail(string email)
    {
        DBservices dbs = new DBservices();
        return dbs.ReadUserByEmail(email);
    }

    //This function returns all users from database
    public static List<User> GetAllUsers()
    {
        DBservices dbs = new DBservices();
        return dbs.GetAllUsers();
    }

    //This function receives user email and returns all his liked songs
    public static List<Object> GetUserLikedSongs(string email)
    {
        DBservices dbs = new DBservices();
        return dbs.GetUserLikedSongs(email);
    }

    //This function receives user email and returns all his liked artists
    public static List<Object> GetUserLikedArtists(string email)
    {
        DBservices dbs = new DBservices();
        return dbs.GetUserLikedArtist(email);
    }

    //This function returns the amount of songs, artists and users in the database of the site
    public static int[] GetStatisticsAdmin()
    {
        DBservices dbs = new DBservices();
        return dbs.GetStatisticsAdmin();
    }

    //This function receives user email and deletes user  
    public bool DeleteUser(string email)
    {
        DBservices dbs = new DBservices();
        return dbs.DeleteUserByEmail(email);
    }
}
