using Lyrics_Final_Proj.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lyrics_Final_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //This function receives an email and returns the user
        [HttpGet]
        [Route("GetUserByEmail/{email}")]
        public User GetUserByEmail(string email)
        {
           return Lyrics_Final_Proj.Models.User.ReadUserByEmail(email);
        }

        //This function returns all users
        [HttpGet]
        [Route("GetAllUsers")]
        public List<User> GetAllUsers()
        {
            return Lyrics_Final_Proj.Models.User.GetAllUsers();
        }

        //This function receives user email and returns the his favorite songs
        [HttpGet]
        [Route("GetUserLikedSongs/{email}")]
        public List<Object> GetUserLikedSongs(string email)
        {
            return Lyrics_Final_Proj.Models.User.GetUserLikedSongs(email);
        }

        //This function receives user email and returns the his favorite artists
        [HttpGet]
        [Route("GetUserLikedArtists/{email}")]
        public List<Object> GetUserLikedArtists(string email)
        {
            return Lyrics_Final_Proj.Models.User.GetUserLikedArtists(email);
        }

        // This function counts how much songs, artists and users are in the database and returns it (for admin)
        [HttpGet]
        [Route("GetStatisticsAdmin")]
        public int[] GetStatisticsAdmin()
        {
            return Lyrics_Final_Proj.Models.User.GetStatisticsAdmin();
        }

        // This function receives user and checks if user exists
        [HttpPost]
        [Route("Login")]
        public int Login(User user)
        {
            try
            {
                return user.Login();
            }
            catch (Exception ex)
            {
                throw new Exception("User not found");
            }
        }

        //This function receives user details and register him
        [HttpPost]
        [Route("Register")]
        public bool Register([FromBody] User user)
        {
            try
            {

                return user.Register();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //This function receives email of user and deletes the user
        // DELETE api/<UsersController>/5
        [HttpDelete]
        [Route("Delete/{email}")]
        public bool Delete(string email)
        {
            User user = new User();
            return user.DeleteUser(email);
        }
    }
}
