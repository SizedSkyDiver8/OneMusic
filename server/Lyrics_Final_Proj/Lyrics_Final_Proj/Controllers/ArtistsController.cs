using Lyrics_Final_Proj.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lyrics_Final_Proj.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        //The function returns a list of all artists from the data base      
        // GET: api/<ArtistsController>
        [HttpGet]
        [Route("GetAllArtists")]
        public IEnumerable<string> ArtistsNames()
        {
            Artist artist = new Artist();
            return artist.ArtistsNames();
        }

        //this function receives a username and returns a list of all his favorite artists
        // GET: api/<ArtistsController>
        [HttpGet]
        [Route("TopArtistsByUsername/{username}")]
        public IEnumerable<string> TopArtistsNames(string username)
        {
            Artist artist = new Artist();
            return artist.TopArtistsByUsername(username);
        }

        // this function returns 5 artists that have the most likes
        // returns the name of the artist and number of likes he has
        // GET: api/<ArtistsController>
        [HttpGet]
        [Route("TopArtists")]
        public IEnumerable<Artist> TopArtistsNames()
        {
            Artist artist = new Artist();
            return artist.TopArtists();
        }

        //this function receives a string and returns all the artists that has this string in their name
        // GET api/<ArtistsController>/5
        [HttpGet]
        [Route("ArtistsByWord/{word}")]
        public List<string> GetArtistsByWord(string word)
        {
            Artist artist =new Artist();
            return artist.GetArtistsWord(word);
        }

        // this function receives an artist name and returns the amount of likes he has
        // GET api/<ArtistsController>/5
        [HttpGet]
        [Route("ArtistsLikes/{name}")]
        public int GetArtistsLikes(string name)
        {
            Artist artist = new Artist();
            return artist.ArtistsLikes(name);
        }

        //this function returns all the artists and their number of likes they have
        [HttpGet]
        [Route("GetAllArtistsWithLikes")]
        public List<Artist> GetArtistsLikes()
        {
            return Artist.GetAllArtistsWithLikes();
        }

        //this function checks if a specific user liked specific artist
        [HttpGet]
        [Route("GetIfUserLikedArtist/{email}/{artistName}")]
        public bool GetIfUserLikedArtist(string email, string artistName)
        {
            return Artist.GetIfUserLikedArtist(email,artistName);
        }

        //this function receives an artist name, enters a Dezzer API and returns information for this artist  
        [HttpGet]
        [Route("GetDeezerInfo/{artist}")]
        public Task<string> GetDeezerInfo(string artist)
        {
            return Artist.GetArtistInfo(artist);
        }

        //This function receives a user email and an artist name and checks if the user liked this artist or not.
        //if this combination does not exists in the database (this user didn't do like to this artist yet) it will add this combination to the database (the user likes this artist)
        // else it will delete this combination from the database (the user disliked this artist)
        [HttpPost]
        [Route("AddRemoveLike/{mail}/{name}")]
        public int AddRemove(string mail, string name)
        {
            try
            {
                Artist artist = new Artist();
                return artist.AddRemoveLike(mail,name);
            }
            catch (Exception ex)
            {
                throw new Exception("User not found");
            }
        }
    }
}
