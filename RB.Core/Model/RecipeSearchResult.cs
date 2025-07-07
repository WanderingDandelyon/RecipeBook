namespace RB.Core.Model
{
    public class RecipeSearchResult
    {
        public Recipe Recipe { get; set; }
        public double MatchScore { get; set; }

        public RecipeSearchResult(Recipe recipe, double matchScore)
        {
            Recipe = recipe;
            MatchScore = matchScore;
        }
    }
}
