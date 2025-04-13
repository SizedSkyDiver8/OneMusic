namespace Lyrics_Final_Proj.Models
{
    public class Artist
    {
        public string Name { get; set; }
        public int FavoriteCount {get;set; }

        //This function receives user email and artist name. It checks if the user liked this artist or not
        // if yes it removes the combination from the database and now the user doesn't like this artist,
        // else it creates the combination in the database and now the user likes this artist
        public int AddRemoveLike(string mail, string name)
        {
            DBservices dBservices = new DBservices();
            return dBservices.AddRemoveLike(mail,name);

        }

        //This function returns all artists name
        public List<string> ArtistsNames()
        {
            DBservices dbs = new DBservices();
            return dbs.ReturnAllArtists();
        }

        //This function returns 5 artists that have the most likes from users
        public List<Artist> TopArtists()
        {
            DBservices dbs = new DBservices();
            return dbs.ReturnTopArtists();
        }

        //This function receives word and checks if this word appears in artists names
        public List<string> GetArtistsWord(string word)
        {
            DBservices dbs = new DBservices();
            return dbs.SearchArtistsByWord(word);
        }
        
        //This function receives artist name and returns the amount of likes he got
        public int ArtistsLikes(string name)
        {
            DBservices dbs = new DBservices();
            return dbs.GetArtistLikes(name);
        }

        //This function receives username and returns all his liked artists 
        public List<string> TopArtistsByUsername(string username)
        {
            DBservices dbs = new DBservices();
            return dbs.GetTopArtistsByUser(username);
        }

        //This function returns all the artists that have likes
        public static List<Artist> GetAllArtistsWithLikes()
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllArtistsWithLikes();
        }

        //This function receives user email and artist name and checks if the user likes this artist
        public static bool GetIfUserLikedArtist(string email, string artistName)
        {
            DBservices dbs = new DBservices();
            return dbs.GetIfUserLikedArtist(email,artistName);
        }

        //This function uses a Deezer API where it receives artist name and returs akk the information about him
        public static async Task<string> GetArtistInfo(string artistName)
        {
            //string artistName = "Queen";
            string apiUrl = $"https://api.deezer.com/search?q={artistName}";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Make a GET request to the Deezer API
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();

                        // Process the response data (you can use JSON deserialization if needed)
                        Console.WriteLine(responseBody);
                        return responseBody;
                    }
                    else
                    {
                        // Handle the case when the request fails
                        Console.WriteLine($"Request failed with status code: {response.StatusCode}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the request
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}
