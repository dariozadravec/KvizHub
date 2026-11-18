namespace KvizHub.Api.Identity
{
    // Authorization policy names, one per user role
    public static class Policies
    {
        public const string Quizmaster = nameof(Quizmaster);
        public const string TeamLeader = nameof(TeamLeader);
        public const string Admin = nameof(Admin);
    }
}
