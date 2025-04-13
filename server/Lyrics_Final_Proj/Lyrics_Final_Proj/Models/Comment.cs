namespace Lyrics_Final_Proj.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string Whom { get; set; }
        public DateTime Date { get; set; }

        //This function add comment by user to an artist in the database
        public int AddCommentArtist()
        {
            DBservices dbs = new DBservices();
            return dbs.AddCommentArtist(this);
        }

        //This function add comment by user to a song in the database
        public int AddCommentSong()
        {
            DBservices dbs = new DBservices();
            return dbs.AddCommentSong(this);
        }

        //This function receives comment id and delete this comment that was wrote on an artist
        public int DeleteCommentA(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteCommentArtist(id);
        }

        //This function receives comment id and delete this comment that was wrote on a song
        public int DeleteCommentS(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.DeleteCommentSong(id);
        }

        //This function receives artist name and returns all the comments that was written about him
        public List<Comment> CommentsByArtist(string artistName)
        {
            DBservices dbs = new DBservices();
            return dbs.ReturnCommentsToArtist(artistName);
        }

        //This function receives song name and returns all the comments that was written about it
        public List<Comment> CommentsBySong(string song)
        {
            DBservices dbs = new DBservices();
            return dbs.ReturnCommentsToSong(song);
        }
        
        //This function receives comment id and a new content of comment that the user wants to change in a comment that already exists on an artist
        //This method changes the content and returns if the content of comments was changed or not
        public static bool ChangeCommentArtist(int id, string comment)
        {
            DBservices dbs = new DBservices();
            return dbs.ChangeCommentArtist(id, comment);
        }

        //This function receives comment id and a new content of comment that the user wants to change in a comment that already exists on an song
        //This method changes the content and returns if the content of comments was changed or not
        public static bool ChangeCommentSong(int id, string comment)
        {
            DBservices dbs = new DBservices();
            return dbs.ChangeCommentSong(id, comment);
        }

    }
}
