namespace Lyrics_Final_Proj.Models
{
    public class Song
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Lyrics { get ; set; }
        public string Link { get ; set; }
        public string ArtistName { get; set; }
        public int FavoriteCount { get; set; }

        //This function returns all songs
        public static List<Song> ReadAllSongs()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadAllSongs();
        }

        //This function returns ll songs that belong to a specific artist
        public static List<Song> GetSongsByArtist(string artistName)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSongsByArtist(artistName);
        }

        //This function receives a song name and returns the song
        public static Song GetSongBySongName(string songName)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSongsBySongName(songName);
        }

        //This function receives word and returns all songs that include this word inside them
        public List<Song> GetSongsWord(string word)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSongsByWord(word);
        }

        //This function receives user email and song name. It checks if the user liked this song or not
        // if yes it removes the combination from the database and now the user doesn't like this song,
        // else it creates the combination in the database and now the user likes this song
        public static int AddSongToFav(string email, string songName)
        {
            DBservices dbs = new DBservices();
            return dbs.UserSong(email, songName);
        }
        
        //This function receives user email and song name and checks if the user likes this song or not
        public static bool GetIfUserLikedSong(string email, string songName)
        {
            DBservices dbs = new DBservices();
            return dbs.GetIfUserLikedSong(email, songName);

        }
    }


}
