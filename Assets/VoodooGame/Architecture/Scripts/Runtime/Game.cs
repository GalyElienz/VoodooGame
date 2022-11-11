using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using UnityEngine;

namespace Runtime
{
    public static class Game
    {
        public static DatabaseReference Reference;
        public static StorageReference StorageReference;
        public static FirebaseAuth Auth;
        public static FirebaseUser User;
        public static string UserId { get; set; }

        public static Texture EnemyImage;
        public static string EnemyName;
        public static string DollIndex;
        public static bool IsSingIn;
    }
}