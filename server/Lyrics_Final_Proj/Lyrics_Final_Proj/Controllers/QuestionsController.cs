using Lyrics_Final_Proj.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lyrics_Final_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        //This function creates a question (it returns a name of an artist as the question and 4 answers as name of song - only 1 song is his)
        // GET api/<QuestionsController>/5
        [HttpGet]
        [Route("GetQuestionArtist")]
        public Question GetQByArtist()
        {
            Question question = new Question();
            question.makeQuestionByArtist();
            return question;
        }

        //This function creates a question (it returns a name of a song as the question and 4 answers as name of artists - only 1 artist sings this song)
        // GET api/<QuestionsController>/5
        [HttpGet]
        [Route("GetQuestionSong")]
        public Question GetQBySong()
        {
            Question question = new Question();
            question.makeQuestionBySong();
            return question;
        }

        //This function creates a question (it returns a string of words from a song and 4 answers as name of song - the lyrics belong to 1 song)
        // GET api/<QuestionsController>/5
        [HttpGet]
        [Route("GetQuestionLyrics")]
        public Question GetQByLyric()
        {
            Question question = new Question();
            question.makeQuestionByLyric();
            return question;
        }

        // The function receives a name of artist and a name of a song and checks if the answer is right (this function works for artist quiz and song quiz)
        // POST api/<QuestionsController>
        [HttpPost]
        [Route("CheckAnswerSongForArtist/{artist}/{song}")]
        public bool Post( string artist, string song)
        {
            return Question.checkQuestionByArtist(artist, song);

        }

        // The function receives a string (words from song) and a name of a song and checks if the answer lyrics of the song include this words 
        // POST api/<QuestionsController>
        [HttpPost]
        [Route("CheckAnswerLyricSong/{lyric}/{song}")]
        public bool PostCheckLyric(string lyric, string song)
        {
            return Question.checkQuestionLyric(lyric, song);

        }
    }
}
