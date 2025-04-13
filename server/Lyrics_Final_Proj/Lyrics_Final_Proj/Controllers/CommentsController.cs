using Lyrics_Final_Proj.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lyrics_Final_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        // This function receives artist name and returns all the comments he got
        // GET: api/<CommentsController>
        [HttpGet]
        [Route("GetAllCommentsArtists/{artistName}")]
        public IEnumerable<Comment> GetCommentsToArtist(string artistName)
        {
            Comment comment = new Comment();
            return comment.CommentsByArtist(artistName);
        }

        //This function receives song name and returns all the comments it got
        // GET: api/<CommentsController>
        [HttpGet]
        [Route("GetAllCommentsSongs/{songName}")]
        public IEnumerable<Comment> GetCommentsToSong(string songName)
        {
            Comment comment = new Comment();
            return comment.CommentsBySong(songName);
        }

        // this function receives a comment object (the email, content of comment and artist name are full)
        // and enters it to the database
        // POST api/<CommentsController>
        [HttpPost]
        [Route("CommentToArtist")]
        public int PostCommentToArtist(Comment comment)
        {
            if (comment.Content == "")
            {
                throw new Exception("$Comment is empty$");
            }
                return comment.AddCommentArtist();
        }

        // this function receives a comment object (the email, content of comment and song name are full)
        // and enters it to the database
        // POST api/<CommentsController>
        [HttpPost]
        [Route("CommentToSong")]
        public int PostCommentToSong(Comment comment)
        {
            if (comment.Content == "")
            {
                throw new Exception("$Comment is empty$");
            }
            return comment.AddCommentSong();
        }

        // This function receives an id of comment that the user wants to edit and the new string that the user entered
        // and changes the content of the comment in the database (for artist)
        // PUT api/<CommentsController>/5
        [HttpPut]
        [Route("ChangeCommentToArtist/{idA}")]
        public bool PutContentArtist(int idA, [FromBody] string content)
        {
            if (content == "")
            {
                throw new Exception("$Comment is empty$");
            }
            return Comment.ChangeCommentArtist(idA,content); 
        }

        // This function receives an id of comment that the user wants to edit and the new string that the user entered
        // and changes the content of the comment in the database (for song)
        [HttpPut]
        [Route("ChangeCommentToSong/{idS}")]
        public bool PutContentSong(int idS, [FromBody] string content)
        {
            if (content == "")
            {
                throw new Exception("$Comment is empty$");
            }
            return Comment.ChangeCommentSong(idS, content);
        }

        //This function receives id of a comment written on an artist and deletes it
        // DELETE api/<CommentsController>/5
        [HttpDelete()]
        [Route("DeleteCommentArtistByID/{idA}")]
        public int DeleteCommentArtist(int idA)
        {
            Comment comment = new Comment();
            return comment.DeleteCommentA(idA);
        }

        //This function receives id of a comment written on a song and deletes it
        // DELETE api/<CommentsController>/5
        [HttpDelete()]
        [Route("DeleteCommentSongByID/{idS}")]
        public int DeleteCommentSong(int idS)
        {
            Comment comment = new Comment();
            return comment.DeleteCommentS(idS);
        }
    }
}
