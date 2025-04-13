using Lyrics_Final_Proj.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lyrics_Final_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        //This function returns all songs
        [HttpGet]
        [Route("GetAllSongs")]
        public IEnumerable<Song> GetAllSongs()
        {
            return Song.ReadAllSongs();
        }

        // This function receives an artist name and returns all his songs
        [HttpGet]
        [Route("GetSongsByArtist/{artistName}")]
        public IEnumerable<Song> GetSongsByArtist(string artistName)
        {
            return Song.GetSongsByArtist(artistName);
        }

        //This function receives string and returns all songs that have this string inside their lyric
        [HttpGet]
        [Route("GetSongByWord/{word}")]
        public List<Song> GetSongsByWord(string word)
        {
            Song song = new Song();
            return song.GetSongsWord(word);
        }

        //This function receives a song name and returns the song
        [HttpGet]
        [Route("GetSongBySongName/{songName}")]
        public Song GetSongBySongName(string songName)
        {
            return Song.GetSongBySongName(songName);
        }

        // This function receives a user email and song name and checks if the user liked this song
        [HttpGet]
        [Route("GetIfUserLikedSong/{email}/{songName}")]
        public bool GetIfUserLikedSong(string email,string songName)
        {
            return Song.GetIfUserLikedSong(email,songName);
        }

        //This function receives a user email and an song name and checks if the user liked this song or not.
        //if this combination does not exists in the database (this user didn't do like to this song yet) it will add this combination to the database (the user likes this song)
        //if it already exists in the database it will delete it (user don't like this song)  
        // POST api/<UsersController>
        [HttpPost]
        [Route("UserLikesSong/{email}/{songName}")]
        public int Post(string email, string songName)
        {
            return Song.AddSongToFav(email, songName);

        }
    }
}
